using Caliburn.Micro;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace ManipulaImagem.ViewModels
{
    /// <summary>
    /// Tela de edição de uma manipulação
    /// </summary>
    public class EditarManipulacaoViewModel : Screen
    {
        #region Declarações

        // Serviços
        private readonly Services.INavegacao _navegacao;
        private readonly Services.ISelecaoArquivo _selecaoArquivo;
        private readonly Services.ITratamentoImagem _tratamentoImagem;

        /// <summary>
        /// Ações que fazem parte da manipulação
        /// </summary>
        private readonly BindableCollection<DataBase.Acao> _acoes = new BindableCollection<DataBase.Acao>();

        #endregion

        #region Propriedades

        /// <summary>
        /// Informações do banco
        /// </summary>
        public DataBase.Manipulacao Manipulacao { get; set; }

        /// <summary>
        /// Nome da manipulação
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Ações que fazem parte da manipulação
        /// </summary>
        public IObservableCollection<DataBase.Acao> Acoes => _acoes;

        /// <summary>
        /// Ação atualmente selecionada
        /// </summary>
        public DataBase.Acao AcaoSelecionada { get; set; } = null;

        /// <summary>
        /// Imagem original
        /// </summary>
        public IMagickImage MagickImageOriginal { get; set; }

        /// <summary>
        /// Imagem processada
        /// </summary>
        public IMagickImage MagickImageProcessada { get; set; }

        /// <summary>
        /// Fonte para exibição da imagem original
        /// </summary>
        public ImageSource ImagemOriginal => MagickImageOriginal?.ToBitmapSource();

        /// <summary>
        /// Fonte para exibição da imagem processada
        /// </summary>
        public ImageSource ImagemProcessada => (MagickImageProcessada ?? MagickImageOriginal)?.ToBitmapSource();

        /// <summary>
        /// Nome da tela
        /// </summary>
        public override string DisplayName
        {
            get => Nome;
            set => base.DisplayName = value;
        }

        /// <summary>
        /// Se pode salvar
        /// </summary>
        public bool CanSalvar => !string.IsNullOrWhiteSpace(Nome) && Nome.Length >= 3;

        /// <summary>
        /// Se pode adicionar ação
        /// </summary>
        public bool CanAcaoAdicionar =>
            MagickImageOriginal != null;

        /// <summary>
        /// Se pode subir a ação selecionada
        /// </summary>
        public bool CanAcaoSubir =>
            AcaoSelecionada != null &&
            AcaoSelecionada.Ordem > 0;

        /// <summary>
        /// Se pode descer a ação selecionada
        /// </summary>
        public bool CanAcaoDescer =>
            AcaoSelecionada != null &&
            AcaoSelecionada.Ordem < _acoes.Count - 1;

        /// <summary>
        /// Se pode editar a ação selecionada
        /// </summary>
        public bool CanAcaoEditar => 
            AcaoSelecionada != null &&
            MagickImageOriginal != null;

        /// <summary>
        /// Se pode excluir a ação selecionada
        /// </summary>
        public bool CanAcaoExcluir => AcaoSelecionada != null;

        #endregion

        #region Construtores

        public EditarManipulacaoViewModel(
            Services.INavegacao navegacao,
            Services.ISelecaoArquivo selecaoArquivo,
            Services.ITratamentoImagem tratamentoImagem)
        {
            _navegacao = navegacao;
            _selecaoArquivo = selecaoArquivo;
            _tratamentoImagem = tratamentoImagem;

            PropertyChanged += EditarManipulacaoViewModel_PropertyChanged;
        }

        #endregion

        #region Funções públicas

        /// <summary>
        /// Sobe a ação selecionada
        /// </summary>
        public void AcaoSubir()
        {
            // Calcula a nova ordem da açao atual
            var ordem = AcaoSelecionada.Ordem - 1;

            // Modifica a ação imediatamente superior a atual e desce-a um nível
            _acoes.Where(a => a.Ordem == ordem).First().Ordem++;
            AcaoSelecionada.Ordem--;

            // Modifica a posição da ação selecionda
            var acao = AcaoSelecionada;
            _acoes.Remove(acao);
            _acoes.Insert(acao.Ordem, acao);
            AcaoSelecionada = acao;
        }

        /// <summary>
        /// Desce a ação selecionada
        /// </summary>
        public void AcaoDescer()
        {
            // Calcula a nova ordem da açao atual
            var ordem = AcaoSelecionada.Ordem + 1;

            // Modifica a ação imediatamente inferior a atual e sobe-a um nível
            _acoes.Where(a => a.Ordem == ordem).First().Ordem--;
            AcaoSelecionada.Ordem++;

            // Modifica a ação imediatamente superior a atual e desce-a um nível
            var acao = AcaoSelecionada;
            _acoes.Remove(acao);
            _acoes.Insert(acao.Ordem, acao);
            AcaoSelecionada = acao;
        }

        /// <summary>
        /// Adiciona uma nova ação
        /// </summary>
        public void AcaoAdicionar()
        {
            NavegarAcao(new DataBase.AcaoEscala()
            {
                Ordem = _acoes.Count
            });
        }

        /// <summary>
        /// Edita a ação selecionada
        /// </summary>
        public void AcaoEditar()
        {
            NavegarAcao(AcaoSelecionada);
        }

        /// <summary>
        /// Exclui a ação selecionada
        /// </summary>
        public void AcaoExcluir()
        {
            // Recupera a ordem da ação atual
            var ordem = AcaoSelecionada.Ordem;

            // Remove a ação atual
            _acoes.Remove(AcaoSelecionada);

            // Corrige a ordem das ações que estavam abaixo da ordem atual
            foreach(var acao in _acoes.Where(a => a.Ordem > ordem))
            {
                acao.Ordem--;
            }

            // Selecione a ação que tomou a posição da selecionada ou a última ação
            AcaoSelecionada = _acoes.Where(a => a.Ordem == ordem).FirstOrDefault() ?? _acoes.LastOrDefault();
        }

        /// <summary>
        /// Cancela a edição
        /// </summary>
        public void Cancelar() => NavegarListagem();

        /// <summary>
        /// Salva a edição no banco
        /// </summary>
        public async void Salvar()
        {
            // Recupera o contexto do banco
            using(var db = new DataBase.ManipulaImagemContext())
            {
                // Valida se é um novo registro ou se é uma atualização
                var dbManipulacao = db.Manipulacoes
                    .Where(m => m.ManipulacaoId == Manipulacao.ManipulacaoId)
                    .FirstOrDefault();

                // Adiciona o novo registro
                if (dbManipulacao == null)
                {
                    dbManipulacao = new DataBase.Manipulacao()
                    {
                        Nome = Nome
                    };

                    db.Entry(dbManipulacao).State = EntityState.Added;
                    await db.SaveChangesAsync();
                }

                // Atualiza o registro atual
                else if(dbManipulacao.Nome != Nome)
                {
                    dbManipulacao.Nome = Nome;

                    db.Entry(dbManipulacao).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                // Remove as ações anteriores
                db.RemoveRange(db.Acoes.Where(a => a.ManipulacaoId == dbManipulacao.ManipulacaoId));

                // Adiciona as novas ações
                foreach(var a in Acoes)
                {
                    a.ManipulacaoId = dbManipulacao.ManipulacaoId;

                    db.Entry(a).State = EntityState.Added;
                }
                await db.SaveChangesAsync();
            }

            NavegarListagem();
        }

        /// <summary>
        /// Abre a imagem para processamento
        /// </summary>
        public async void AbrirImagem()
        {
            var arquivo = await _selecaoArquivo.Abrir("Selecione a imagem original", new System.Collections.Generic.Dictionary<string, string[]>()
            {
                { "Imagem", new string[] { "*.jpg", "*.jpeg", "*.png", "*.gif", "*.tif", "*.tiff", "*.bmp" } }
            });

            if (arquivo != null)
            {
                MagickImageOriginal = new MagickImage(File.ReadAllBytes(arquivo));
            }
        }

        /// <summary>
        /// Salva a imagem processada
        /// </summary>
        public async void SalvarImagem()
        {
            var arquivo = await _selecaoArquivo.Salvar("Selecione onde salvar a imagem", new System.Collections.Generic.Dictionary<string, string[]>()
            {
                { "Imagem", new string[] { "*.jpg", "*.jpeg", "*.png", "*.gif", "*.tif", "*.tiff", "*.bmp" } }
            });

            if (arquivo != null)
            {
                MagickImageProcessada.Write(arquivo);
            }
        }

        #endregion

        #region Funções privadas

        /// <summary>
        /// Navega para a listagem de manipulações
        /// </summary>
        private void NavegarListagem()
            => _navegacao.Navegar<SelecionarManipulacaoViewModel>();

        /// <summary>
        /// Exibe a edição de uma ação
        /// </summary>
        /// <param name="acao">Ação a editar</param>
        private void NavegarAcao(DataBase.Acao acao)
        {
            // Guarda uma cópia da ação original
            var edicao = acao.Clone();

            // Cria a tela de edição
            var viewModel = new EditarAcaoViewModel(_tratamentoImagem)
            {
                Acao = edicao,
                DisplayName = DisplayName,
                MagickImageOriginal = ProcessarImagemAteAcao(edicao.Ordem - 1)
            };

            viewModel.Aceito += (s, e) =>
            {
                // Remove a ação antiga
                var antigo = _acoes.Where(a => a.Ordem == edicao.Ordem).FirstOrDefault();
                if (antigo != null)
                {
                    _acoes.Remove(antigo);
                }

                // Adiciona a nova ação
                viewModel.Acao.Ordem = edicao.Ordem;
                _acoes.Insert(viewModel.Acao.Ordem, viewModel.Acao);

                // Trata a selação de ação
                AcaoSelecionada = viewModel.Acao;

                // Volta para a tela anterior
                _navegacao.Navegar(this);
            };

            // Volta para a tela anterior
            viewModel.Cancelado += (s, e) => _navegacao.Navegar(this);

            // Exibe a edição de ação
            _navegacao.Navegar(viewModel);
        }

        /// <summary>
        /// Atualiza a imagem processada conforme a ação selecionada
        /// </summary>
        private void AtualizarImagemProcessada()
            => MagickImageProcessada = ProcessarImagemAteAcao(AcaoSelecionada != null ? AcaoSelecionada.Ordem : -1);

        /// <summary>
        /// Processa a imagem original com todas as ações até a ação de ordem informada
        /// </summary>
        /// <param name="ordemAcao">Ordem da última ação a processar</param>
        /// <returns>Imagem processada</returns>
        private IMagickImage ProcessarImagemAteAcao(int ordemAcao)
        {
            if (MagickImageOriginal == null)
            {
                return null;
            }

            var p = MagickImageOriginal.Clone();

            foreach (var acao in _acoes
                .Where(a => a.Ordem <= ordemAcao)
                .OrderBy(a => a.Ordem))
            {
                _tratamentoImagem.ProcessarAcao(acao, p);
            }

            return p;
        }

        #endregion

        #region Resposta a eventos

        /// <summary>
        /// Evento chamado quando alguma proprieade é modificada
        /// </summary>
        private void EditarManipulacaoViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Seleciona a ação segundo a propriedade modificada
            switch (e.PropertyName)
            {
                case nameof(Manipulacao):
                    // Limpa as ações
                    _acoes.Clear();

                    // Caso a manipulação possua algum valor
                    if (Manipulacao != null)
                    {
                        // Atualiza o nome
                        Nome = Manipulacao.Nome;

                        // Atualiza as ações
                        using (var db = new DataBase.ManipulaImagemContext())
                        {
                            _acoes.AddRange(db.Acoes.Where(a => a.ManipulacaoId == Manipulacao.ManipulacaoId).OrderBy(a => a.Ordem));
                        }

                    }
                    else
                    {
                        AcaoSelecionada = null;
                    }

                    break;

                case nameof(ImagemOriginal):
                case nameof(AcaoSelecionada):
                    AtualizarImagemProcessada();
                    break;
            }
        }

        #endregion
    }
}
