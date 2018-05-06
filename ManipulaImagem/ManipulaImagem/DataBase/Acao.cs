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

        /// <summary>
        /// Construtor
        /// </summary>
        public Acao()
        {
            Tipo = TipoInterno;
        }

        /// <summary>
        /// Cria um umbeto para clonagem
        /// </summary>
        /// <returns>Clone</returns>
        protected abstract Acao CriarClone();

        /// <summary>
        /// Clona o objeto atual
        /// </summary>
        /// <returns>Cria um clone em memória do objeto atual</returns>
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
