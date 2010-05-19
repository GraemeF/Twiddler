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
    }
}