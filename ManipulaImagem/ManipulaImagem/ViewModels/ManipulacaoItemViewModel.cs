using Caliburn.Micro;
using System;
using System.Linq;

namespace ManipulaImagem.ViewModels
{
    public class ManipulacaoItemViewModel : Screen
    {
        #region Eventos

        public event EventHandler Excluido;

        #endregion

        #region Declarações

        private readonly Services.INavegacao _navegacao;

        #endregion

        #region Propriedade

        public DataBase.Manipulacao Manipulacao { get; set; }

        public string Nome => Manipulacao?.Nome;

        #endregion

        #region Construtor

        public ManipulacaoItemViewModel(Services.INavegacao navegacao)
        {
            _navegacao = navegacao;
        }

        #endregion

        #region Funções públicas

        public void Excluir()
        {
            // Exclui a manipulação atual
            using (var db = new DataBase.ManipulaImagemContext())
            {
                db.Manipulacoes.Remove(
                    db.Manipulacoes.Where(m=>m.ManipulacaoId==Manipulacao.ManipulacaoId).First());

                db.SaveChanges();
            }

            // Informa a exclusão
            Excluido?.Invoke(this, EventArgs.Empty);
        }

        public void Editar()
        {
            // Exibe a tela de edição de manipulação
            var e = _navegacao.Navegar<EditarManipulacaoViewModel>();
            e.Manipulacao = Manipulacao;
        }

        #endregion
    }
}
