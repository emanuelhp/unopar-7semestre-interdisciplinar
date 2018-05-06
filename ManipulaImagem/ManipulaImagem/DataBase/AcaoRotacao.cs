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

        /// <summary>
        /// Descrição textual do objeto
        /// </summary>
        /// <returns>Descrição</returns>
        public override string ToString()
        {
            return $"Rotacionar {Angulo}º";
        }

        /// <summary>
        /// Cria um clone em memória do objeto atual
        /// </summary>
        /// <returns>Clone</returns>
        protected override Acao CriarClone()
        {
            return new AcaoRotacao()
            {
                Angulo = Angulo
            };
        }
    }
}
