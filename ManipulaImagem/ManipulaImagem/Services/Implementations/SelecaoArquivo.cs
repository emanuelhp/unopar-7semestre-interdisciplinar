using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ManipulaImagem.Services.Implementations
{
    /// <summary>
    /// Implementa a selação de arquivos utilizando System.Windows
    /// </summary>
    class SelecaoArquivo : ISelecaoArquivo
    {
        /// <summary>
        /// Converte um dicionário de tipos de arquivos para o
        /// formato reconhecido pelo windows
        /// </summary>
        /// <param name="tiposArquivo">
        /// Dicionário com os tipos de arquivos, sendo a chave
        /// a descrição do tipo de arquivo e o valor um array de
        /// extensões
        /// </param>
        /// <returns>Informações de tipos de arquivos</returns>
        private string MontarTiposArquivo(Dictionary<string,string[]> tiposArquivo)
        {
            return string.Join("|",
                    tiposArquivo
                    .Select(t => string.Concat(t.Key, "|", string.Join(";", t.Value)))
                    );
        }

        /// <summary>
        /// Exibe o diálogo para seleção de arquivo para leitura
        /// </summary>
        /// <param name="titulo">Título do diálogo</param>
        /// <param name="tiposArquivo">Tipos de arquivos aceitos</param>
        /// <returns>
        /// Retorna o caminho completo do arquivo selecionado ou nulo se
        /// o usuário cancelar a seleção
        /// </returns>
        public async Task<string> Abrir(string titulo, Dictionary<string, string[]> tiposArquivo)
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var ofd = new OpenFileDialog()
                {
                    AddExtension = true,
                    CheckPathExists = true,
                    Filter = MontarTiposArquivo(tiposArquivo),
                    Multiselect = false,
                    Title = titulo
                };

                if (ofd.ShowDialog(Application.Current.MainWindow) == true)
                {
                    return ofd.FileName;
                }

                return null;
            });
        }

        /// <summary>
        /// Exibe o diálogo para seleção de arquivo para escrita
        /// </summary>
        /// <param name="titulo">Título do diálogo</param>
        /// <param name="tiposArquivo">Tipos de arquivos aceitos</param>
        /// <returns>
        /// Retorna o caminho completo do arquivo selecionado ou nulo se
        /// o usuário cancelar a seleção
        /// </returns>
        public async Task<string> Salvar(string titulo, Dictionary<string, string[]> tiposArquivo)
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var ofd = new SaveFileDialog()
                {
                    AddExtension = true,
                    CheckPathExists = true,
                    Filter = MontarTiposArquivo(tiposArquivo),
                    Title = titulo
                };

                if (ofd.ShowDialog(Application.Current.MainWindow) == true)
                {
                    return ofd.FileName;
                }

                return null;
            });
        }
    }
}
