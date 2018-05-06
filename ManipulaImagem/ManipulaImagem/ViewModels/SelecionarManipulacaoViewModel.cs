using Caliburn.Micro;
using System.Linq;

namespace ManipulaImagem.ViewModels
{
    /// <summary>
    /// Tela listagem das manipulações
    /// </summary>
    public class SelecionarManipulacaoViewModel : Screen
    {
        #region Declarações

        private readonly Services.INavegacao _navegacao;

        /// <summary>
        /// Manipulações a serem exibidas
        /// </summary>
        private readonly IObservableCollection<ManipulacaoItemViewModel> _manipulacoes = new BindableCollection<ManipulacaoItemViewModel>();

        #endregion

        #region Propriedades

        /// <summary>
        /// Manipulações a serem exibidas
        /// </summary>
        public IObservableCollection<ManipulacaoItemViewModel> Manipulacoes => _manipulacoes;

        /// <summary>
        /// Nome da nova manipulação
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Se pode adicionar a nova manipulação
        /// </summary>
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

        /// <summary>
        /// Função chamada quando a tela é exibida
        /// </summary>
        protected override void OnActivate()
        {
            _manipulacoes.Clear();

            // Recupera uma referência do banco
            using (var db = new DataBase.ManipulaImagemContext())
            {
                // Carrega as manipulações cadastradas
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

        /// <summary>
        /// Adiciona uma nova manipulação com o nome informado
        /// </summary>
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
