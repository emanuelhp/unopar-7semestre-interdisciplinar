using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ManipulaImagem
{
    public class AppBootstrapper : BootstrapperBase
    {
        SimpleContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void StartRuntime()
        {
            // Garante a criação do banco de dados
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            using(var db = new DataBase.ManipulaImagemContext())
            {
                db.Database.Migrate();
            }

            base.StartRuntime();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            // Classes do Caliburn.Micro
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();

            // Tela inicial
            var main = new ViewModels.MainViewModel();
            container.Instance<IShell>(main);

            // Serviços
            container.Instance<Services.INavegacao>(main);
            container.Singleton<Services.ISelecaoArquivo, Services.Implementations.SelecaoArquivo>();
            container.Singleton<Services.ITratamentoImagem, Services.Implementations.TratamentoImagem>();

            // Telas
            container.PerRequest<ViewModels.SelecionarManipulacaoViewModel>();
            container.PerRequest<ViewModels.EditarManipulacaoViewModel>();
            container.PerRequest<ViewModels.ManipulacaoItemViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}