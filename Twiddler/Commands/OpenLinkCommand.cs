namespace Twiddler.Commands
{
    #region Using Directives

    using System;
    using System.Diagnostics;

    using Twiddler.Commands.Interfaces;

    #endregion

    public class OpenLinkCommand : IOpenLinkCommand
    {
        #region IOpenLinkCommand Members

        public void Execute(object parameter)
        {
            Process.Start(parameter.ToString(), string.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        #endregion
    }
}