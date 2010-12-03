using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Fluid;
using Gallio.Framework;
using Twiddler.AcceptanceTests.TestEntities.Properties;

namespace Twiddler.AcceptanceTests.TestEntities
{
    public class TwiddlerApplication : IDisposable
    {
        private readonly Process _process;
        private readonly Window _shell;

        #region Path to the Twiddler app.

#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif
        private const string ApplicationPath = @"..\..\..\Twiddler\bin\" + Configuration + @"\Twiddler.exe";

        #endregion

        /// <summary>
        /// Launches an instance of Twiddler for testing.
        /// </summary>
        /// <returns>The launched instance.</returns>
        public TwiddlerApplication(bool newStore)
        {
            var args = new StringBuilder();
            if (newStore)
                DeleteTemporaryStore();

            args.AppendFormat(" /store=\"{0}\"", TemporaryStorePath);
            args.AppendFormat(" /service={0}", DefaultService);

            string path = Path.GetFullPath(ApplicationPath);
            TestLog.WriteLine("Starting \"{0}\" {1}", path, args.ToString());

            var startInfo = new ProcessStartInfo(path, args.ToString()) {UseShellExecute = false};
            _process = Process.Start(startInfo);

            try
            {
                _shell = Desktop.
                    Window.
                    OwnedBy(_process).
                    Titled("Twiddler");
            }
            catch (Exception)
            {
                if (!_process.HasExited)
                    _process.Kill();
                throw;
            }
        }

        public TwiddlerApplication()
            : this(true)
        {
        }

        private static void DeleteTemporaryStore()
        {
            while (Directory.Exists(TemporaryStorePath))
                try
                {
                    Directory.Delete(TemporaryStorePath, true);
                }
                catch (IOException)
                {
                    Thread.Sleep(200);
                }
        }

        /// <summary>
        /// Gets the shell window.
        /// </summary>
        /// <value>The shell window.</value>
        public Shell Shell
        {
            get { return new Shell(_shell); }
        }

        public AuthorizationWindow AuthorizationWindow
        {
            get
            {
                return new AuthorizationWindow(Desktop.
                                                   Window.
                                                   OwnedBy(_process).
                                                   Titled("OAuthDialog"));
            }
        }

        private static string TemporaryStorePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                    @"TwiddlerTest");
            }
        }

        public static Uri DefaultService
        {
            get { return new Uri(Settings.Default.DefaultService); }
        }

        /// <summary>
        /// Releases all resources used by an instance of the <see cref="TwiddlerApplication" /> class.
        /// </summary>
        /// <remarks>
        /// This method calls the virtual <see cref="Dispose(bool)" /> method, passing in <strong>true</strong>, and then suppresses 
        /// finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources before an instance of the <see cref="TwiddlerApplication" /> class is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// This method releases unmanaged resources by calling the virtual <see cref="Dispose(bool)" /> method, passing in <strong>false</strong>.
        /// </remarks>
        ~TwiddlerApplication()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by an instance of the <see cref="TwiddlerApplication" /> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><strong>true</strong> to release both managed and unmanaged resources; <strong>false</strong> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    _process.CloseMainWindow();
                }
                catch (InvalidOperationException)
                {
                }

                _process.Dispose();
            }
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