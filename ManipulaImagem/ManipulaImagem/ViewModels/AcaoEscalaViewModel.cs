using Caliburn.Micro;

namespace ManipulaImagem.ViewModels
{
    public class AcaoEscalaViewModel : Screen
    {
        #region Declarações

        private readonly EditarAcaoViewModel _editarAcaoViewModel;

        #endregion

        #region Propriedades

        public int EscalaPercentagem { get; set; }

        public EditarAcaoViewModel EditarAcaoViewModel => _editarAcaoViewModel;

        #endregion

        #region Construtor

        public AcaoEscalaViewModel(
            EditarAcaoViewModel editarAcaoViewModel)
        {
            _editarAcaoViewModel = editarAcaoViewModel;

            PropertyChanged += AcaoEscalaViewModel_PropertyChanged;

            EscalaPercentagem = ((DataBase.AcaoEscala)_editarAcaoViewModel.Acao).Percentagem;
        }

        #endregion

        #region Resposta a eventos

        private void AcaoEscalaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EscalaPercentagem):
                    ((DataBase.AcaoEscala)_editarAcaoViewModel.Acao).Percentagem = EscalaPercentagem;
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
            }
        }

        #endregion
    }
}
