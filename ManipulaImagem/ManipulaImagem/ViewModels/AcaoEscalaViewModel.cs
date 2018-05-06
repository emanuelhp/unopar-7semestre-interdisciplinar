using Caliburn.Micro;

namespace ManipulaImagem.ViewModels
{
    /// <summary>
    /// Tela de edição da ação de escala
    /// </summary>
    public class AcaoEscalaViewModel : Screen
    {
        #region Declarações

        private readonly EditarAcaoViewModel _editarAcaoViewModel;

        #endregion

        #region Propriedades

        /// <summary>
        /// Percentagem de escala
        /// </summary>
        public int EscalaPercentagem { get; set; }

        public EditarAcaoViewModel EditarAcaoViewModel => _editarAcaoViewModel;

        #endregion

        #region Construtor

        public AcaoEscalaViewModel(
            EditarAcaoViewModel editarAcaoViewModel)
        {
            _editarAcaoViewModel = editarAcaoViewModel;

            PropertyChanged += AcaoEscalaViewModel_PropertyChanged;

            // Recupera os valores iniciais
            EscalaPercentagem = ((DataBase.AcaoEscala)_editarAcaoViewModel.Acao).Percentagem;
        }

        #endregion

        #region Resposta a eventos

        /// <summary>
        /// Evento disparado quando alguma propriedade é modificada
        /// </summary>
        private void AcaoEscalaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(EscalaPercentagem):
                    // Transmite a alteração da propriedade para a ação
                    ((DataBase.AcaoEscala)_editarAcaoViewModel.Acao).Percentagem = EscalaPercentagem;

                    // Informa que os parâmetros da ação foram modificados
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
            }
        }

        #endregion
    }
}
