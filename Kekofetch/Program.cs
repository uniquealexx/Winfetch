using System.Management;
using System.Net;

namespace Kekofetch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int x = Console.GetCursorPosition().Left;
            int y = Console.GetCursorPosition().Top;

            HardwareInfo hardware = new(x, y);

            hardware.RenderLogo();
            hardware.RenderInfo();
        }
    }
}
