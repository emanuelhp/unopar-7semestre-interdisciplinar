using Caliburn.Micro;

namespace ManipulaImagem.ViewModels
{
    public class AcaoTranslacaoViewModel : Screen
    {
        #region Declarações

        private readonly EditarAcaoViewModel _editarAcaoViewModel;

        #endregion

        #region Propriedades

        public int TralacaoX { get; set; }
        public int TralacaoY { get; set; }

        public EditarAcaoViewModel EditarAcaoViewModel => _editarAcaoViewModel;

        #endregion

        #region Construtor

        public AcaoTranslacaoViewModel(
            EditarAcaoViewModel editarAcaoViewModel)
        {
            _editarAcaoViewModel = editarAcaoViewModel;

            PropertyChanged += AcaoEscalaViewModel_PropertyChanged;

            TralacaoX = ((DataBase.AcaoTranslacao)_editarAcaoViewModel.Acao).X;
            TralacaoY = ((DataBase.AcaoTranslacao)_editarAcaoViewModel.Acao).Y;
        }

        #endregion

        #region Resposta a eventos

        private void AcaoEscalaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TralacaoX):
                    ((DataBase.AcaoTranslacao)_editarAcaoViewModel.Acao).X = TralacaoX;
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
                case nameof(TralacaoY):
                    ((DataBase.AcaoTranslacao)_editarAcaoViewModel.Acao).Y = TralacaoY;
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
            }
        }

        #endregion
    }
}
