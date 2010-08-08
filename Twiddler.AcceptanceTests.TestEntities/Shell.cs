using System.Linq;
using Fluid;

namespace Twiddler.AcceptanceTests.TestEntities
{
    public class Shell
    {
        private readonly Window _window;

        public Shell(Window window)
        {
            _window = window;
        }

        public bool IsUserAuthorized
        {
            get
            {
                return TextBlock.
                    In(_window, "Status").
                    Called("Authorization").
                    First().
                    Text == "Authorized";
            }
        }
    }
}