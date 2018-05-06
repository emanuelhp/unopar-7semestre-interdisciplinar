using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManipulaImagem.DataBase
{
    /// <summary>
    /// Agrupa ações para uma determinada manipulação
    /// </summary>
    [Table(nameof(Manipulacao))]
    public class Manipulacao
    {
        /// <summary>
        /// Id gerado automaticamente
        /// </summary>
        public int ManipulacaoId { get; set; }

        /// <summary>
        /// Nome da manipulação
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Nome { get; set; }

        /// <summary>
        /// Ações da manipulação
        /// </summary>
        public List<Acao> Acoes { get; set; } = new List<Acao>();
    }
}
