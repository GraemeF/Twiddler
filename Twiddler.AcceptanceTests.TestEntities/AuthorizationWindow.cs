using System.Linq;
using Fluid;

namespace Twiddler.AcceptanceTests.TestEntities
{
    public class AuthorizationWindow
    {
        private readonly Window _window;

        public AuthorizationWindow(Window window)
        {
            _window = window;
        }

        public string Pin
        {
            get { return PinEditBox.Text; }
            set { PinEditBox.Text = value; }
        }

        private EditBox PinEditBox
        {
            get
            {
                return EditBox.
                    In(_window).
                    Called("pinTextBox").First();
            }
        }

        public void ClickAuthorizeAtTwitterButton()
        {
            Button.
                In(_window).
                Called("AuthorizeDesktopBtn").First().
                Click();
        }

        public void ClickOKButton()
        {
            Button.
                In(_window).
                Called("okBtn").First().
                Click();
        }
    }
}