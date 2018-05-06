using Caliburn.Micro;

namespace ManipulaImagem.ViewModels
{
    /// <summary>
    /// Tela de edição da ação de translação
    /// </summary>
    public class AcaoTranslacaoViewModel : Screen
    {
        #region Declarações

        private readonly EditarAcaoViewModel _editarAcaoViewModel;

        #endregion

        #region Propriedades

        /// <summary>
        /// Valor da translação em X
        /// </summary>
        public int TralacaoX { get; set; }

        /// <summary>
        /// Valor da translação em Y
        /// </summary>
        public int TralacaoY { get; set; }

        public EditarAcaoViewModel EditarAcaoViewModel => _editarAcaoViewModel;

        #endregion

        #region Construtor

        public AcaoTranslacaoViewModel(
            EditarAcaoViewModel editarAcaoViewModel)
        {
            _editarAcaoViewModel = editarAcaoViewModel;

            PropertyChanged += AcaoEscalaViewModel_PropertyChanged;

            // Recupera os valores iniciais
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
                    // Transmite a alteração da propriedade para a ação
                    ((DataBase.AcaoTranslacao)_editarAcaoViewModel.Acao).X = TralacaoX;

                    // Informa que os parâmetros da ação foram modificados
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
                case nameof(TralacaoY):
                    // Transmite a alteração da propriedade para a ação
                    ((DataBase.AcaoTranslacao)_editarAcaoViewModel.Acao).Y = TralacaoY;

                    // Informa que os parâmetros da ação foram modificados
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
            }
        }

        #endregion
    }
}
