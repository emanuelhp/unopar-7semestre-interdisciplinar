using Caliburn.Micro;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ManipulaImagem.ViewModels
{
    public class EditarAcaoViewModel : Screen
    {
        #region Constantes

        private const int INT_DELAY_PROCESSAMENTO_MS = 100;

        #endregion

        #region Eventos

        public event EventHandler Cancelado;
        public event EventHandler Aceito;

        #endregion

        #region Declarações estáticas

        private static readonly Dictionary<int,string> _acaoTipos = new Dictionary<int, string>
        {
            { DataBase.AcaoEscala.INT_TIPO, "Escala" },
            { DataBase.AcaoRotacao.INT_TIPO, "Rotação" },
            { DataBase.AcaoTranslacao.INT_TIPO, "Translação" },
        };

        #endregion

        #region Declarações

        private readonly Services.ITratamentoImagem _tratamentoImagem;

        private DateTime _ultimaModificacao;
        private Task _processamento;

        #endregion

        #region Propriedades

        public IEnumerable<KeyValuePair<int, string>> AcaoTipos => _acaoTipos;

        public KeyValuePair<int, string> AcaoTipo { get; set; }

        public DataBase.Acao Acao { get; set; }

        public IScreen AcaoViewModel
        {
            get
            {
                switch (Acao.Tipo)
                {
                    case DataBase.AcaoEscala.INT_TIPO:
                        return new AcaoEscalaViewModel(this);
                    case DataBase.AcaoRotacao.INT_TIPO:
                        return new AcaoRotacaoViewModel(this);
                    case DataBase.AcaoTranslacao.INT_TIPO:
                        return new AcaoTranslacaoViewModel(this);
                    default:
                        return null;
                }
            }
        }

        public IMagickImage MagickImageOriginal { get; set; }

        public IMagickImage MagickImageProcessada { get; set; }

        public ImageSource ImagemOriginal => MagickImageOriginal?.ToBitmapSource();

        public ImageSource ImagemProcessada => (MagickImageProcessada ?? MagickImageOriginal)?.ToBitmapSource();

        #endregion

        #region Construtor

        public EditarAcaoViewModel(Services.ITratamentoImagem tratamentoImagem)
        {
            _tratamentoImagem = tratamentoImagem;

            PropertyChanged += EditarAcaoViewModel_PropertyChanged;
        }

        #endregion

        #region Funções privadas

        private async Task AtualizarImagem()
        {
            double ms;
            while ((ms = DateTime.Now.Subtract(_ultimaModificacao).TotalMilliseconds) < INT_DELAY_PROCESSAMENTO_MS)
            {
                await Task.Delay(INT_DELAY_PROCESSAMENTO_MS - (int)ms);
            }

            DateTime processamento;
            do
            {
                processamento = DateTime.Now;
                var p = MagickImageOriginal.Clone();

                _tratamentoImagem.ProcessarAcao(Acao, p);

                MagickImageProcessada = p;
            }
            while (processamento < _ultimaModificacao);
        }

        #endregion

        #region Funções públicas

        public void Salvar() => Aceito?.Invoke(this, EventArgs.Empty);

        public void Cancelar() => Cancelado?.Invoke(this, EventArgs.Empty);

        public void ParametrosModificados()
        {
            _ultimaModificacao = DateTime.Now;
            if (_processamento == null || _processamento.IsCompleted || _processamento.IsCanceled || _processamento.IsFaulted)
            {
                _processamento = Task.Run(AtualizarImagem);
            }
        }

        #endregion

        #region Resposta a eventos

        private void EditarAcaoViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AcaoTipo):
                    if (AcaoTipo.Key != Acao.Tipo)
                    {
                        switch (AcaoTipo.Key)
                        {
                            case DataBase.AcaoEscala.INT_TIPO:
                                Acao = new DataBase.AcaoEscala();
                                break;
                            case DataBase.AcaoRotacao.INT_TIPO:
                                Acao = new DataBase.AcaoRotacao();
                                break;
                            case DataBase.AcaoTranslacao.INT_TIPO:
                                Acao = new DataBase.AcaoTranslacao();
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    break;
                case nameof(Acao):
                    if (AcaoTipo.Key != Acao.Tipo)
                    {
                        AcaoTipo = _acaoTipos.Where(i => i.Key == Acao.Tipo).FirstOrDefault();
                    }
                    break;
            }
        }

        #endregion
    }
}
