using System.ComponentModel.DataAnnotations.Schema;

namespace ManipulaImagem.DataBase
{
    /// <summary>
    /// Uma manipulação específica
    /// </summary>
    [Table(nameof(Acao))]
    public abstract class Acao
    {
        /// <summary>
        /// Id da manipulação a qual essa ação pertence
        /// </summary>
        public int ManipulacaoId { get; set; }

        /// <summary>
        /// Ordem da ação
        /// </summary>
        public int Ordem { get; set; }

        /// <summary>
        /// Tipo da ação
        /// </summary>
        protected abstract int TipoInterno { get; }

        public int Tipo { get; set; }


        /// <summary>
        /// Referência da manipulação a qual essa ação pertence
        /// </summary>
        public Manipulacao Manipulacao { get; set; }

        public Acao()
        {
            Tipo = TipoInterno;
        }

        protected abstract Acao CriarClone();

        public Acao Clone()
        {
            var c = CriarClone();

            c.ManipulacaoId = ManipulacaoId;
            c.Ordem = Ordem;

            c.Manipulacao = Manipulacao;

            return c;
        }
    }
}
