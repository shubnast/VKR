using Caliburn.Micro;
using System.Windows;
using TemplaterView.ViewModels;
using TemplaterView.Views;

namespace TemplaterView
{
    class TemplateBootstrapper : BootstrapperBase
    {
        public TemplateBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<TemplateWindowViewModel>();
        }
    }
}
