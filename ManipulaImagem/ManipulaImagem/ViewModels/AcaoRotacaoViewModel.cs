using Caliburn.Micro;
using System;

namespace ManipulaImagem.ViewModels
{
    /// <summary>
    /// Tela de edição da ação de rotacao
    /// </summary>
    public class AcaoRotacaoViewModel : Screen
    {
        #region Declarações

        private readonly EditarAcaoViewModel _editarAcaoViewModel;

        #endregion

        #region Propriedades

        /// <summary>
        /// Ângulo da rotação
        /// </summary>
        public int AnguloRotacao { get; set; }

        /// <summary>
        /// Se permite apenas multiplos de trinta graus para a rotação
        /// </summary>
        public bool Apenas30Graus { get; set; }

        /// <summary>
        /// Ângulo máximo de rotação
        /// </summary>
        public int AnguloRotacaoMaximo => Apenas30Graus ? 330 : 359;

        public EditarAcaoViewModel EditarAcaoViewModel => _editarAcaoViewModel;

        #endregion

        #region Construtores

        public AcaoRotacaoViewModel(EditarAcaoViewModel editarAcaoViewModel)
        {
            _editarAcaoViewModel = editarAcaoViewModel;

            PropertyChanged += AcaoRotacionarViewModel_PropertyChanged;

            // Recupera os valores iniciais
            AnguloRotacao = ((DataBase.AcaoRotacao)_editarAcaoViewModel.Acao).Angulo;
        }

        #endregion

        #region Resposta a eventos

        /// <summary>
        /// Evento disparado quando alguma propriedade é modificada
        /// </summary>
        private void AcaoRotacionarViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AnguloRotacao):
                    // Transmite a alteração da propriedade para a ação
                    ((DataBase.AcaoRotacao)_editarAcaoViewModel.Acao).Angulo = AnguloRotacao;

                    // Informa que os parâmetros da ação foram modificados
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
                case nameof(Apenas30Graus):
                    // Caso seja modificada a opção restringido o ângulo de rotação
                    if (Apenas30Graus)
                    {
                        // Arredonda o valor atual para o múltiplo de 30 mais próximo
                        AnguloRotacao = (int)(30 * Math.Round(AnguloRotacao / 30.0));

                        // Trata o ângulo máximo de rotação
                        if (AnguloRotacao > AnguloRotacaoMaximo)
                        {
                            AnguloRotacao = AnguloRotacaoMaximo;
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
