namespace Twiddler.AcceptanceTests.TestEntities
{
    #region Using Directives

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Core;
    using Core.UIItems.WindowItems;

    using MbUnit.Framework;

    using Twiddler.AcceptanceTests.TestEntities.Properties;

    using Desktop = Fluid.Desktop;

    #endregion

    public class TwiddlerApplication : IDisposable
    {
        private readonly Application _application;

        #region Path to the Twiddler app.

#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif

        private const string ApplicationPath = @"..\..\..\Twiddler\bin\" + Configuration + @"\Twiddler.exe";

        #endregion

        /// <summary>
        /// 	Initializes a new instance of the <see cref="TwiddlerApplication"/> class. 
        /// 	Launches an instance of Twiddler for testing.
        /// </summary>
        /// <param name="newStore">
        /// 	The new Store.
        /// </param>
        /// <returns>
        /// 	The launched instance.
        /// </returns>
        public TwiddlerApplication(bool newStore)
        {
            var args = new StringBuilder();

            args.Append(" /inMemory");
            args.AppendFormat(" /service={0}", DefaultService);

            string path = Path.GetFullPath(ApplicationPath);

            var startInfo = new ProcessStartInfo(path, args.ToString())
                                {
                                    UseShellExecute = false
                                };
            _application = Application.Launch(startInfo);

            try
            {
                Shell = new Shell(GetShellWindow());
            }
            catch (Exception)
            {
                if (!_application.HasExited)
                    _application.Kill();
                throw;
            }
        }

        public TwiddlerApplication()
            : this(true)
        {
        }

        private Window GetShellWindow()
        {
            // wait for the window to appear if it hasn't already
            for (int i = 0; i < 100; i++)
            {
                if (_application.GetWindows().Any())
                    break;
                Thread.Sleep(100);
            }

            return _application.GetWindows().First();
        }

        /// <summary>
        /// 	Gets the shell window.
        /// </summary>
        /// <value>The shell window.</value>
        public Shell Shell { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_application.HasExited)
                return;

            try
            {
                Retry.
                    WithPolling(1000).
                    Repeat(10).
                    DoBetween(AttemptToClose).
                    Until(() => _application.HasExited);
            }
            catch (Exception ex)
            {
                if (!_application.HasExited)
                {
                    _application.Kill();
                    throw new ApplicationException("The application did not close after 10 seconds.", ex);
                }

                throw;
            }
        }

        private void AttemptToClose()
        {
            try
            {
                Shell.Close();
            }
            catch
            {
            }
        }

        #endregion

        public bool HasExited
        {
            get { return _application.HasExited; }
        }

        public AuthorizationWindow AuthorizationWindow
        {
            get
            {
                return new AuthorizationWindow(Desktop.
                                                   Window.
                                                   OwnedBy(_application.Process).
                                                   Titled("OAuthDialog"));
            }
        }

        public static Uri DefaultService
        {
            get { return new Uri(Settings.Default.DefaultService); }
        }

        public string AuthorizationStatus
        {
            get { return Shell.AuthorizationStatus; }
        }

        public void Authorize()
        {
            Shell.ClickAuthorizeButton();
            AuthorizationWindow.ClickAuthorizeAtTwitterButton();
            AuthorizationWindow.Pin = "1234567";
            AuthorizationWindow.ClickOKButton();
        }
    }
}