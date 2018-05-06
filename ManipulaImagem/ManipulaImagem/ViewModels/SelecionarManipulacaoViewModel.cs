using Caliburn.Micro;
using System.Linq;

namespace ManipulaImagem.ViewModels
{
    public class SelecionarManipulacaoViewModel : Screen
    {
        #region Declarações

        private readonly Services.INavegacao _navegacao;

        private readonly IObservableCollection<ManipulacaoItemViewModel> _manipulacoes = new BindableCollection<ManipulacaoItemViewModel>();

        #endregion

        #region Propriedades

        public IObservableCollection<ManipulacaoItemViewModel> Manipulacoes => _manipulacoes;

        public string Nome { get; set; }

        public bool CanAdicionar => !string.IsNullOrWhiteSpace(Nome) && Nome.Length >= 3;

        #endregion

        #region Construtores

        public SelecionarManipulacaoViewModel(Services.INavegacao navegacao)
        {
            _navegacao = navegacao;

            DisplayName = "Manipulações";
        }

        #endregion

        #region Sobrescrições

        protected override void OnActivate()
        {
            using (var db = new DataBase.ManipulaImagemContext())
            {
                _manipulacoes.AddRange(
                    db.Manipulacoes.ToArray().Select(m =>
                    {
                        var i = IoC.Get<ManipulacaoItemViewModel>();
                        i.Manipulacao = m;
                        i.Excluido += (s, e) => _manipulacoes.Remove(i);
                        return i;
                    }));
            }
        }

        #endregion

        #region Funções públicas

        public void Adicionar()
        {
            var editar = _navegacao.Navegar<EditarManipulacaoViewModel>();
            editar.Manipulacao = new DataBase.Manipulacao()
            {
                Nome = Nome
            };
        }

        #endregion
    }
}
