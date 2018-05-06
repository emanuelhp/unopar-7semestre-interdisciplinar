using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ManipulaImagem.Services.Implementations
{
    class SelecaoArquivo : ISelecaoArquivo
    {
        private string MontarTiposArquivo(Dictionary<string,string[]> tiposArquivo)
        {
            return string.Join("|",
                    tiposArquivo
                    .Select(t => string.Concat(t.Key, "|", string.Join(";", t.Value)))
                    );
        }

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
