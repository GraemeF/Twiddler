using System.Reflection;
using Caliburn.PresentationFramework.ApplicationModel;
using Twiddler.Screens.Interfaces;

namespace Twiddler
{
    public partial class App : CaliburnApplication
    {
        protected override object CreateRootModel()
        {
            return Container.GetInstance<IShellScreen>();
        }

        protected override Assembly[] SelectAssemblies()
        {
            return new[] {Assembly.GetExecutingAssembly()};
        }
    }
}