using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManipulaImagem.Services
{
    /// <summary>
    /// Serviço que permite a seleção de arquivos para leitura ou escrita
    /// </summary>
    public interface ISelecaoArquivo
    {
        /// <summary>
        /// Exibe o diálogo para seleção de arquivo para leitura
        /// </summary>
        /// <param name="titulo">Título do diálogo</param>
        /// <param name="tiposArquivo">Tipos de arquivos aceitos</param>
        /// <returns>
        /// Retorna o caminho completo do arquivo selecionado ou nulo se
        /// o usuário cancelar a seleção
        /// </returns>
        Task<string> Abrir(string titulo, Dictionary<string, string[]> tiposArquivo);

        /// <summary>
        /// Exibe o diálogo para seleção de arquivo para escrita
        /// </summary>
        /// <param name="titulo">Título do diálogo</param>
        /// <param name="tiposArquivo">Tipos de arquivos aceitos</param>
        /// <returns>
        /// Retorna o caminho completo do arquivo selecionado ou nulo se
        /// o usuário cancelar a seleção
        /// </returns>
        Task<string> Salvar(string titulo, Dictionary<string, string[]> tiposArquivo);
    }
}
