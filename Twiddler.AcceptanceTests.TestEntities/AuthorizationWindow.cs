﻿namespace Twiddler.AcceptanceTests.TestEntities
{
    #region Using Directives

    using System.Linq;

    using Fluid;

    #endregion

    public class AuthorizationWindow
    {
        private readonly Window _window;

        public AuthorizationWindow(Window window)
        {
            _window = window;
        }

        public bool HasError
        {
            get
            {
                return Window.
                    In(_window).
                    Matching(x => x.Current.Name == "Error").
                    Any();
            }
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