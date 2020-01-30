using System;
using System.Threading;
using Meadow.Devices;

namespace Meadow
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Console.WriteLine("Initializing");

            var counter = new Counter();
            var display = new Display(Device, counter);
            var input = new Input(Device, counter);
            input.InputReceived += (sender, args) => display.Update();

            // TODO: Connect to IoT Hub when network is implemented
        }

        public void RunAndBlock()
        {
            Console.WriteLine("Running");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
