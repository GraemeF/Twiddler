using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Fluid;

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
        public static TwiddlerApplication Launch(bool newStore, string store, Uri service)
        {
            var args = new StringBuilder();
            if (newStore)
                if (Directory.Exists(TemporaryStorePath))
                    Directory.Delete(TemporaryStorePath, true);

            if (store != null)
                args.AppendFormat(" /store=\"{0}\"", store);
            if (service != null)
                args.AppendFormat(" /service={0}", service.ToString());

            var path = Path.GetFullPath(ApplicationPath);
            Gallio.Framework.TestLog.WriteLine("Starting \"{0}\" {1}", path, args.ToString());

            var startInfo = new ProcessStartInfo(path, args.ToString()) { UseShellExecute = false };
            Process process = Process.Start(startInfo);

            try
            {
                return new TwiddlerApplication(process, Desktop.Window.OwnedBy(process).Titled("Twiddler"));
            }
            catch (Exception)
            {
                if (!process.HasExited)
                    process.Kill();
                throw;
            }
        }

        /// <summary>
        /// Launches an instance of Twiddler for testing.
        /// </summary>
        /// <returns>The launched instance.</returns>
        public static TwiddlerApplication Launch()
        {
            return Launch(true, TemporaryStorePath, DefaultService);
        }

        /// <summary>
        /// Launches an instance of Twiddler for testing.
        /// </summary>
        /// <returns>The launched instance.</returns>
        public static TwiddlerApplication Launch(bool newStore)
        {
            return Launch(newStore, null, null);
        }

        private TwiddlerApplication(Process process, Window shell)
        {
            _process = process;
            _shell = shell;
        }

        /// <summary>
        /// Gets the shell window.
        /// </summary>
        /// <value>The shell window.</value>
        public Shell Shell
        {
            get { return new Shell(_shell); }
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
            get { return new Uri(Properties.Settings.Default.DefaultService); }
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
                _process.CloseMainWindow();
                _process.Dispose();
            }
        }

        public bool IsUserAuthorized
        {
            get
            {
                return Shell.IsUserAuthorized;
            }
        }
    }
}