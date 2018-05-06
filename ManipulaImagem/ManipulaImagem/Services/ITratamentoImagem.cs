using ImageMagick;

namespace ManipulaImagem.Services
{
    public interface ITratamentoImagem
    {
        DataBase.Acao RecuperarDetalhes(DataBase.Acao acao);
        void ProcessarAcao(DataBase.Acao acao, IMagickImage imagem);
    }
}
