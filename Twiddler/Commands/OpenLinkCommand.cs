using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Caliburn.Core.IoC;
using Twiddler.Commands.Interfaces;
using Twiddler.Core;

namespace Twiddler.Commands
{
    [Singleton(typeof (IOpenLinkCommand))]
    [Export(typeof (IOpenLinkCommand))]
    [NoCoverage]
    public class OpenLinkCommand : IOpenLinkCommand
    {
        #region IOpenLinkCommand Members

        public void Execute(object parameter)
        {
            Process.Start((parameter).ToString(), "");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}