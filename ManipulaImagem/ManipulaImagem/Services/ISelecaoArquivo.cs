using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManipulaImagem.Services
{
    public interface ISelecaoArquivo
    {
        Task<string> Abrir(string titulo, Dictionary<string, string[]> tiposArquivo);
        Task<string> Salvar(string titulo, Dictionary<string, string[]> tiposArquivo);
    }
}
