using nanoFramework.Hardware.Esp32;
using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Runtime.Native;
using System.Device.Gpio;

namespace nFgraphics_test
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");
            Debug.WriteLine($"Main FW: {SystemInfo.Version}");

            // Screen init 
            int chipSelect;
            int dataCommand;
            int reset;
            int backLightPin;

            const bool wrover = true;

            // TODO: Set correct phisical display size
            const int screenWidth = 240;
            const int screenHeight = 320;
            const int screenBufferSize = 240 * 320 * 2; //16bpp


            // TODO: Update Your display pinout!
            if (wrover)
            {
                backLightPin = 5;
                chipSelect = 22;
                dataCommand = 21;
                reset = 4;
            }
            else
            {
                backLightPin = 32;
                chipSelect = 14;
                dataCommand = 27;
                reset = 33;
            }
            Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);

            // optional PWM control for backlight
            //Configuration.SetPinFunction(backLightPin, DeviceFunction.PWM1);


            DisplayControl.Initialize(new SpiConfiguration(1, chipSelect, dataCommand, reset, -1), new ScreenConfiguration(0, 0, screenWidth, screenHeight), screenBufferSize);
            Debug.WriteLine("Screen initialized");

            // TODO: for first test use default orientation change to true to change orientation
            if (false)
            {
                DisplayControl.ChangeOrientation(DisplayOrientation.LANDSCAPE180);
                Debug.WriteLine("Orientation set LANDSCAPE180");
            }

            // TODO: make sure that Your bp (backlight pin) configured properly.
            var bp = new GpioController().OpenPin(backLightPin, PinMode.Output);
            bp.Write(PinValue.High);

            // Set test font to use
            Font DisplayFont = Resource.GetFont(Resource.FontResources.lcon16b);


            // Write something on display:
            DisplayControl.Write($"Hello from nF!",
                (ushort)0,
                (ushort)(DisplayFont.Height * 0),
                (ushort)DisplayControl.ScreenWidth,
                (ushort)DisplayControl.ScreenHeight,
                DisplayFont, Color.Blue, Color.Black);

            DisplayControl.Write($"Main FW: {SystemInfo.Version}",
                (ushort)0,
                (ushort)(DisplayFont.Height * 1),
                (ushort)DisplayControl.ScreenWidth,
                (ushort)DisplayControl.ScreenHeight,
                DisplayFont, Color.Red, Color.Black);

            DisplayControl.Write($"My first text on display!!!",
                (ushort)0,
                (ushort)(DisplayFont.Height * 3),
                (ushort)DisplayControl.ScreenWidth,
                (ushort)DisplayControl.ScreenHeight,
                DisplayFont, Color.Green, Color.Black);


            DisplayControl.Write($"0123456789abcdef0123456789ABCDE",
                (ushort)0,
                (ushort)(DisplayFont.Height * 5),
                (ushort)DisplayControl.ScreenWidth,
                (ushort)DisplayControl.ScreenHeight,
                DisplayFont, Color.Red, 0);

            Debug.WriteLine("Display test done.");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
