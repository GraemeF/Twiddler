﻿using System;
using Caliburn.Core.IoC;
using Twiddler.Services.Interfaces;

namespace Twiddler.Services
{
    [Singleton(typeof(IClock))]
    public class Clock : IClock
    {
        public DateTime Now
        {
            get { return DateTime.UtcNow; }
        }
    }
}