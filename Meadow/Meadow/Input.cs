using System;
using Meadow.Devices;
using Meadow.Foundation.Sensors.Buttons;

namespace Meadow
{
    public class Input
    {
        private const string DeviceName = "Meadow";
        private readonly Counter _counter;

        public event EventHandler InputReceived;

        public Input(F7Micro device, Counter counter)
        {
            _counter = counter;

            var downButton = new PushButton(device, device.Pins.D03);
            downButton.Clicked += DownButtonOnClicked;

            var upButton = new PushButton(device, device.Pins.D04);
            upButton.Clicked += UpButtonOnClicked;
        }

        private void DownButtonOnClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Down button clicked");
         
            _counter.Down(DeviceName);
            InputReceived?.Invoke(this, e);
        }

        private void UpButtonOnClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Up button clicked");

            _counter.Up(DeviceName);
            InputReceived?.Invoke(this, e);
        }
    }
}