using Caliburn.Micro;

namespace ManipulaImagem.Services
{
    /// <summary>
    /// Serviço que permite a navegação entre as telas do sistema
    /// </summary>
    public interface INavegacao
    {
        /// <summary>
        /// Cria e navega para uma tela de um tipo específico
        /// </summary>
        /// <typeparam name="T">Tipo da tela a nevagar</typeparam>
        /// <returns>Tela criada</returns>
        T Navegar<T>() where T : IScreen;

        /// <summary>
        /// Navega para uma tela já existente
        /// </summary>
        /// <param name="tela">Tela a navegar</param>
        void Navegar(IScreen tela);
    }
}
