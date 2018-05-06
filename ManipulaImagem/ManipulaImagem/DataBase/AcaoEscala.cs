using System.ComponentModel.DataAnnotations;

namespace ManipulaImagem.DataBase
{
    /// <summary>
    /// Informações de uma ação de scala
    /// </summary>
    public class AcaoEscala : Acao
    {
        public const int INT_TIPO = 1;

        /// <summary>
        /// Porcentagem da escal
        /// </summary>
        [Required]
        public int Percentagem { get; set; } = 100;

        protected override int TipoInterno => INT_TIPO;

        /// <summary>
        /// Descrição textual do objeto
        /// </summary>
        /// <returns>Descrição</returns>
        public override string ToString()
        {
            return $"Escalar em {Percentagem}%";
        }

        /// <summary>
        /// Cria um clone em memória do objeto atual
        /// </summary>
        /// <returns>Clone</returns>
        protected override Acao CriarClone()
        {
            return new AcaoEscala()
            {
                Percentagem = Percentagem
            };
        }
    }
}
