using System;
using System.Diagnostics;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;
using Twiddler.Screens.Interfaces;

namespace Twiddler.Screens
{
    [PerRequest(typeof (ILinkScreen))]
    public class LinkScreen : Screen<Uri>, ILinkScreen
    {
        public LinkScreen(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; private set; }

        public void OpenLink()
        {
            Process.Start(Uri.ToString(), "");
        }
    }
}