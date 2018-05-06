using Caliburn.Micro;

namespace ManipulaImagem.Services
{
    public interface INavegacao
    {
        T Navegar<T>() where T : IScreen;
        void Navegar(IScreen tela);
    }
}
