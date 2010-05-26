using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twiddler.Screens.Interfaces;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.Screens;

namespace Twiddler.Screens
{
    [PerRequest(typeof(IRequestMeterScreen))]
    public class RequestMeter : Screen, IRequestMeterScreen
    {
    }
}
