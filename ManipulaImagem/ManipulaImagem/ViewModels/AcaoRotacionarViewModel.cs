using Caliburn.Micro;
using System;

namespace ManipulaImagem.ViewModels
{
    public class AcaoRotacionarViewModel : Screen
    {
        #region Declarações

        private readonly EditarAcaoViewModel _editarAcaoViewModel;

        #endregion

        #region Propriedades

        public int AnguloRotacao { get; set; }
        public bool Apenas30Graus { get; set; }

        public int AnguloRotacaoMaximo => Apenas30Graus ? 330 : 359;

        public EditarAcaoViewModel EditarAcaoViewModel => _editarAcaoViewModel;

        #endregion

        #region Construtores

        public AcaoRotacionarViewModel(EditarAcaoViewModel editarAcaoViewModel)
        {
            _editarAcaoViewModel = editarAcaoViewModel;

            PropertyChanged += AcaoRotacionarViewModel_PropertyChanged;

            AnguloRotacao = ((DataBase.AcaoRotacao)_editarAcaoViewModel.Acao).Angulo;
        }

        #endregion

        #region Resposta a eventos

        private void AcaoRotacionarViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AnguloRotacao):
                    ((DataBase.AcaoRotacao)_editarAcaoViewModel.Acao).Angulo = AnguloRotacao;
                    _editarAcaoViewModel.ParametrosModificados();
                    break;
                case nameof(Apenas30Graus):
                    if (Apenas30Graus)
                    {
                        AnguloRotacao = (int)(30 * Math.Round(AnguloRotacao / 30.0));
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
