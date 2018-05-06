using Microsoft.EntityFrameworkCore;

namespace ManipulaImagem.DataBase
{
    /// <summary>
    /// Definições do banco de dados
    /// </summary>
    public class ManipulaImagemContext : DbContext
    {
        public DbSet<Manipulacao> Manipulacoes { get; set; }
        public DbSet<Acao> Acoes { get; set; }
        public DbSet<AcaoEscala> AcoesEscala { get; set; }
        public DbSet<AcaoEscala> AcoesRotacionar { get; set; }
        public DbSet<AcaoTranslacao> AcoesTranslacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Define o arquivo de banco de dados
            optionsBuilder.UseSqlite("Data source=manipula_imagem.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define o relacionamento entre Ação e Manipulação
            modelBuilder.Entity<Acao>()
                .HasOne(a => a.Manipulacao)
                .WithMany(m => m.Acoes);

            // Define a chave primária da tabela Ação
            modelBuilder.Entity<Acao>()
                .HasKey(a => new { a.ManipulacaoId, a.Ordem });

            // Configura o modelo de herança
            modelBuilder.Entity<Acao>()
                .HasDiscriminator<int>(nameof(Acao.Tipo))
                .HasValue<AcaoEscala>(AcaoEscala.INT_TIPO)
                .HasValue<AcaoRotacao>(AcaoRotacao.INT_TIPO)
                .HasValue<AcaoTranslacao>(AcaoTranslacao.INT_TIPO);
        }
    }
}
