﻿using System.Windows.Input;
using Caliburn.PresentationFramework.Screens;

namespace Twiddler.Screens.Interfaces
{
    public interface ITweetScreen : IScreen
    {
        string Id { get; }
        ICommand MarkAsReadCommand { get; }
    }
}