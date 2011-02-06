using System.Linq;
using Fluid;

namespace Twiddler.AcceptanceTests.TestEntities
{
    public class Shell
    {
        private readonly Window _window;

        public Shell(Core.UIItems.WindowItems.Window window)
        {
            _window = new Window {AutomationElement = window.AutomationElement};
        }

        public string AuthorizationStatus
        {
            get
            {
                return TextBlock.
                    In(Status).
                    Called("Authorization").First().
                    Text;
            }
        }

        private Container Status
        {
            get
            {
                return Container.
                    In(_window).
                    Called("Status").First();
            }
        }

        public void ClickAuthorizeButton()
        {
            Button.
                In(Status).
                Called("Authorize").First().
                Click();
        }

        public void Close()
        {
            _window.AutomationElement.GetWindowPattern().Close();
        }
    }
}