using ImageMagick;

namespace ManipulaImagem.Services
{

    /// <summary>
    /// Serviço que permite o tratamento de imagem
    /// </summary>
    public interface ITratamentoImagem
    {
        /// <summary>
        /// Realiza uma determinada ação na imagem
        /// </summary>
        /// <param name="acao">Ação a executar</param>
        /// <param name="imagem">Imagem</param>
        void ProcessarAcao(DataBase.Acao acao, IMagickImage imagem);
    }
}
