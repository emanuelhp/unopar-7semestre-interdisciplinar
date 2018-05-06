using Caliburn.Micro;
using ManipulaImagem.Services;

namespace ManipulaImagem.ViewModels
{
    /// <summary>
    /// Tela principal
    /// </summary>
    public class MainViewModel : Screen, IShell, INavegacao
    {
        #region Propriedades

        /// <summary>
        /// Tela atual
        /// </summary>
        public IScreen TelaAtual { get; set; }

        #endregion

        #region Construtores

        public MainViewModel()
        {
            // Define o título
            DisplayName = "Manipulador de Imagens";
        }

        #endregion

        #region Sobrescrições

        /// <summary>
        /// Executa as ações de inicialização
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Navega para a listagem de manipulações
            Navegar<SelecionarManipulacaoViewModel>();
        }

        #endregion

        #region Funções públicas

        /// <summary>
        /// Navega para uma tela específica
        /// </summary>
        /// <typeparam name="T">Tela a navegar</typeparam>
        /// <returns>Tela criada</returns>
        public T Navegar<T>()
            where T : IScreen
        {
            // Cria a tela
            var n = IoC.Get<T>();

            // Navega para a tela criada
            Navegar(n);

            // Retorna a tela criada
            return n;
        }

        /// <summary>
        /// Navega para uma tela específica
        /// </summary>
        /// <param name="tela">Tela a navegar</param>
        public void Navegar(IScreen tela)
        {
            // Ativa a nova tela
            tela.Activate();
            TelaAtual = tela;
        }

        #endregion
    }
}
