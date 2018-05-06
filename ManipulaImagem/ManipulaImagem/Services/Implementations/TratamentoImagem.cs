using ImageMagick;
using ManipulaImagem.DataBase;
using System;
using System.Linq;

namespace ManipulaImagem.Services.Implementations
{
    /// <summary>
    /// Classe que permite o tratamento das imagens
    /// </summary>
    class TratamentoImagem : ITratamentoImagem
    {
        /// <summary>
        /// Realiza uma determinada ação na imagem
        /// </summary>
        /// <param name="acao">Ação a executar</param>
        /// <param name="imagem">Imagem</param>
        public void ProcessarAcao(Acao acao, IMagickImage imagem)
        {
            if(acao is AcaoEscala escala)
            {
                imagem.Scale(new Percentage(escala.Percentagem));
            }
            else if (acao is AcaoRotacao rotacionar)
            {
                imagem.Rotate(rotacionar.Angulo);
            }
            else if (acao is AcaoTranslacao translacao)
            {
                using (var imgOr = imagem.Clone())
                {
                    imagem.Colorize(MagickColors.Black, new Percentage(100));
                    imagem.Composite(imgOr, translacao.X, translacao.Y);
                }
            }
            else
            {
                ProcessarAcao(RecuperarDetalhes(acao), imagem);
            }
        }

        /// <summary>
        /// Recupera as informações complementares de uma ação
        /// </summary>
        /// <param name="acao">Ação base</param>
        /// <returns>Ação detalhada</returns>
        public Acao RecuperarDetalhes(Acao acao)
        {
            using (var db = new ManipulaImagemContext())
            {
                switch (acao.Tipo)
                {
                    case AcaoEscala.INT_TIPO:
                        return db.AcoesEscala.Where(a => a.ManipulacaoId == acao.ManipulacaoId && a.Ordem == acao.Ordem).First();
                    case AcaoRotacao.INT_TIPO:
                        return db.AcoesRotacionar.Where(a => a.ManipulacaoId == acao.ManipulacaoId && a.Ordem == acao.Ordem).First();
                    case AcaoTranslacao.INT_TIPO:
                        return db.AcoesTranslacao.Where(a => a.ManipulacaoId == acao.ManipulacaoId && a.Ordem == acao.Ordem).First();
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
