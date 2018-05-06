using Caliburn.Micro;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace ManipulaImagem.ViewModels
{
    public class EditarManipulacaoViewModel : Screen
    {
        #region Declarações

        private readonly Services.INavegacao _navegacao;
        private readonly Services.ISelecaoArquivo _selecaoArquivo;
        private readonly Services.ITratamentoImagem _tratamentoImagem;

        private readonly BindableCollection<DataBase.Acao> _acoes = new BindableCollection<DataBase.Acao>();

        #endregion

        #region Propriedades

        public DataBase.Manipulacao Manipulacao { get; set; }

        public string Nome { get; set; }

        public IObservableCollection<DataBase.Acao> Acoes => _acoes;

        public DataBase.Acao AcaoSelecionada { get; set; } = null;

        public IMagickImage MagickImageOriginal { get; set; }

        public IMagickImage MagickImageProcessada { get; set; }

        public ImageSource ImagemOriginal => MagickImageOriginal?.ToBitmapSource();

        public ImageSource ImagemProcessada => (MagickImageProcessada ?? MagickImageOriginal)?.ToBitmapSource();

        public override string DisplayName
        {
            get => Nome;
            set => base.DisplayName = value;
        }

        public bool CanSalvar => !string.IsNullOrWhiteSpace(Nome) && Nome.Length >= 3;

        public bool CanAcaoAdicionar =>
            MagickImageOriginal != null;

        public bool CanAcaoSubir =>
            AcaoSelecionada != null &&
            AcaoSelecionada.Ordem > 0;

        public bool CanAcaoDescer =>
            AcaoSelecionada != null &&
            AcaoSelecionada.Ordem < _acoes.Count - 1;

        public bool CanAcaoEditar => 
            AcaoSelecionada != null &&
            MagickImageOriginal != null;

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

        public void AcaoSubir()
        {
            var ordem = AcaoSelecionada.Ordem - 1;
            _acoes.Where(a => a.Ordem == ordem).First().Ordem++;
            AcaoSelecionada.Ordem--;
            var acao = AcaoSelecionada;
            _acoes.Remove(acao);
            _acoes.Insert(acao.Ordem, acao);
            AcaoSelecionada = acao;
            NotifyOfPropertyChange(nameof(AcaoSelecionada));
        }

        public void AcaoDescer()
        {
            var ordem = AcaoSelecionada.Ordem + 1;
            _acoes.Where(a => a.Ordem == ordem).First().Ordem--;
            AcaoSelecionada.Ordem++;
            var acao = AcaoSelecionada;
            _acoes.Remove(acao);
            _acoes.Insert(acao.Ordem, acao);
            AcaoSelecionada = acao;
            NotifyOfPropertyChange(nameof(AcaoSelecionada));
        }

        public void AcaoAdicionar()
        {
            NavegarAcao(new DataBase.AcaoEscala()
            {
                Ordem = _acoes.Count
            });
        }

        public void AcaoEditar()
        {
            NavegarAcao(AcaoSelecionada);
        }

        public void AcaoExcluir()
        {
            var ordem = AcaoSelecionada.Ordem;

            _acoes.Remove(AcaoSelecionada);
            foreach(var acao in _acoes.Where(a => a.Ordem > ordem))
            {
                acao.Ordem--;
            }

            AcaoSelecionada = _acoes.Where(a => a.Ordem == ordem).FirstOrDefault();
        }

        public void Cancelar() => NavegarListagem();

        public async void Salvar()
        {
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

        private void NavegarListagem()
            => _navegacao.Navegar<SelecionarManipulacaoViewModel>();

        private void NavegarAcao(DataBase.Acao acao)
        {
            var edicao = acao.Clone();

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

        private void AtualizarImagemProcessada()
            => MagickImageProcessada = ProcessarImagemAteAcao(AcaoSelecionada != null ? AcaoSelecionada.Ordem : -1);

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
