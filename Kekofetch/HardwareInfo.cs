using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kekofetch
{
    internal class HardwareInfo
    {
        #region Variables
        private const int NullIndex = 0;

        private const int SpaceValue = 5;

        private static int indexValue = 0;

        private const int windows11Version = 22000;

        private static string url = Environment.OSVersion.Version.Build >= windows11Version ?
            "https://pastebin.com/raw/zkMs93g7" : "https://pastebin.com/raw/KYTP7WCj";

        private static string version = Environment.OSVersion.Version.Build >= windows11Version ?
            "Windows 11" : "Windows 10";

        private static string logoWindows = ParseTextFile(url);

        private static int x, y;
        #endregion

        #region Сlass Member Functions
        public HardwareInfo(int left, int top)
        {
            x = left;
            y = top;
        }
        #endregion

        #region Functions
        // this is old method, but if u need u can update to HttpsClient solution
        private static string ParseTextFile(string url)
        {
            string? textFile = null;

            using (WebClient client = new())
            {
                try
                {
                    textFile = client.DownloadString(url);
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Error: {ex.Status}");
                }
            }

            return textFile;
        }

        private static void WriteLine(string text = "", object? obj = null)
        {
            int index = logoWindows.IndexOf('\n');
            int length = index >= NullIndex ? index : logoWindows.Length;

            Console.SetCursorPosition(x + length + SpaceValue, y + indexValue++);

            Console.WriteLine(text, obj);
        }

        public static void GetCPUInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                WriteLine();
                WriteLine("---------------------CPU-------------------");
                WriteLine("CPU: {0}", obj["Name"]);
                WriteLine("CPU Speed: {0} MHz", obj["CurrentClockSpeed"]);
                WriteLine("Cores: {0}", obj["NumberOfCores"]);
                WriteLine("Threads: {0}", obj["NumberOfLogicalProcessors"]);
                WriteLine("Manufacture: {0}", obj["Manufacturer"]);
                WriteLine();
            }
        }

        public static void GetHDDInfo()
        {
            List<string> discName = ["A:\\", "B:\\", "C:\\", "D:\\", "E:\\", "F:\\"];

            WriteLine("-------------------HDD/SSD------------------");

            foreach (string disc in discName)
            {
                DriveInfo drive = new DriveInfo(disc);
                if (!drive.IsReady)
                {
                    continue;
                }

                WriteLine($"HDD/SSD: {drive.Name}");

                double totalSizeInGb = drive.TotalSize / Math.Pow(1024, 3);
                double freeSpaceInGb = drive.AvailableFreeSpace / Math.Pow(1024, 3);
                double usedSpaceInGb = totalSizeInGb - freeSpaceInGb;

                WriteLine($"Total size: {Math.Round(totalSizeInGb, 2)} Gb");
                WriteLine($"Free space: {Math.Round(freeSpaceInGb, 2)} Gb");
                WriteLine($"Used space: {Math.Round(usedSpaceInGb, 2)} Gb");

                WriteLine();
            }
        }

        public static void GetRAMInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");

            WriteLine("--------------------Memory------------------");

            List<string> enumeration = ["First RAM", "Second RAM", "Third RAM", "Fourth RAM"];

            int index = 0;

            foreach (ManagementObject obj in searcher.Get())
            {
                WriteLine($"{enumeration[index]} Capacity: {Convert.ToInt64(obj["Capacity"]) / Math.Pow(1024, 3)} Gb");
                WriteLine($"Speed: {obj["Speed"]} MHz");
                index++;
            }

            WriteLine();
        }

        public static void GetOSInfo()
        {
            WriteLine("---------------------OS--------------------");
            WriteLine($"OS name: {version} | {Environment.OSVersion}");
            WriteLine($"User name: {Environment.UserName}");
            WriteLine($"PC name: {Environment.MachineName}");
        }

        public void RenderLogo()
        {
            Console.SetCursorPosition(x, y + 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(logoWindows);
            Console.ResetColor();
        }

        public void RenderInfo()
        {
            WriteLine();
            GetOSInfo();
            GetCPUInfo();
            GetRAMInfo();
            GetHDDInfo();
            WriteLine();

            Console.SetCursorPosition(NullIndex, indexValue);
        }
        #endregion
    }
}
