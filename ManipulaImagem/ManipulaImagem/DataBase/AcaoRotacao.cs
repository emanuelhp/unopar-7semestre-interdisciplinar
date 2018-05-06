using System.ComponentModel.DataAnnotations;

namespace ManipulaImagem.DataBase
{
    /// <summary>
    /// Informações de uma ação de rotação
    /// </summary>
    public class AcaoRotacao : Acao
    {
        public const int INT_TIPO = 2;
     
        /// <summary>
        /// Ângulo em graus a rotacionar a imagem
        /// </summary>
        [Required]
        public int Angulo { get; set; }

        protected override int TipoInterno => INT_TIPO;

        public override string ToString()
        {
            return $"Rotacionar {Angulo}º";
        }

        protected override Acao CriarClone()
        {
            return new AcaoRotacao()
            {
                Angulo = Angulo
            };
        }
    }
}
