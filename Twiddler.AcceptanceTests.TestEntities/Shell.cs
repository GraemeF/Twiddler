using System.Linq;
using Fluid;

namespace Twiddler.AcceptanceTests.TestEntities
{
    public class Shell
    {
        private readonly Window _window;

        public Shell(Window window)
        {
            _window = window;
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
    }
}