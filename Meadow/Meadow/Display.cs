using System.Threading.Tasks;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;

namespace Meadow
{
    public class Display
    {
        private readonly Counter _counter;
        private readonly RgbPwmLed _rgbPwmLed;
        private readonly GraphicsLibrary _graphics;

        public Display(F7Micro device, Counter counter)
        {
            _counter = counter;
            _rgbPwmLed = new RgbPwmLed(device,
                device.Pins.OnboardLedRed,
                device.Pins.OnboardLedGreen,
                device.Pins.OnboardLedBlue);

            var ssd1306 = new Ssd1306(device.CreateI2cBus());
            _graphics = new GraphicsLibrary(ssd1306);
            DrawUi();
        }

        public void Update()
        {
            FlashLeds();
            DrawUi();
        }

        private void DrawUi()
        {
            _graphics.Clear();
            _graphics.CurrentFont = new Font8x12();
            _graphics.DrawRectangle(0, 0, (int)_graphics.Width, (int)_graphics.Height);
            _graphics.DrawText(12, 12, "Value:");
            _graphics.DrawText(80, 12, $"{_counter.Value, 5}");
            _graphics.DrawText(12, 28, "Device:");
            _graphics.DrawText(12, 42, _counter.DeviceName);
            _graphics.Show();
        }

        private void FlashLeds()
        {
            if (_counter.Direction == CounterDirection.Up) FlashLed(Color.Green);
            if (_counter.Direction == CounterDirection.Down) FlashLed(Color.Blue);
        }

        private void FlashLed(Color ledColor)
        {
            _rgbPwmLed.SetColor(ledColor);
            Task
                .Delay(100)
                .ContinueWith(task => _rgbPwmLed.Stop());
        }
    }
}