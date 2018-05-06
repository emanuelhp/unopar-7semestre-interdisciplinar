using System.ComponentModel.DataAnnotations;

namespace ManipulaImagem.DataBase
{
    /// <summary>
    /// Informações da ação de translação
    /// </summary>
    public class AcaoTranslacao : Acao
    {
        public const int INT_TIPO = 3;

        /// <summary>
        /// Valor da translação no eixo X
        /// </summary>
        [Required]
        public int X { get; set; }

        /// <summary>
        /// Valor da translação no eixo Y
        /// </summary>
        [Required]
        public int Y { get; set; }

        protected override int TipoInterno => INT_TIPO;

        /// <summary>
        /// Descrição textual do objeto
        /// </summary>
        /// <returns>Descrição</returns>
        public override string ToString()
        {
            return $"Transladar para ({X},{Y})";
        }

        /// <summary>
        /// Cria um clone em memória do objeto atual
        /// </summary>
        /// <returns>Clone</returns>
        protected override Acao CriarClone()
        {
            return new AcaoTranslacao()
            {
                X = X,
                Y = Y
            };
        }
    }
}
