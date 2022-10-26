using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace SortOutFiles
{
    /// <summary>
    /// CMD run example: SortOutFiles.exe c:\Temp\Picture c:\Temp\PictureOut
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            TestInputArguments(args);

            string sourcePath = args[0];
            string destPath = args[1];

            //DEBUG
            //sourcePath = @"c:\Temp\Picture";
            //destPath = @"c:\Temp\PictureOut";

            Console.WriteLine($"\nProvided source path: {sourcePath}");
            Console.WriteLine($"Provided destination path: {destPath}");

            try
            {
                SortOutPhotoFiles sortFiles = new SortOutPhotoFiles(sourcePath, destPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nApplication run failed with message: {e.Message}\n");
                Console.WriteLine($"Source: {e.Source}\n");
                Console.WriteLine($"Inner exception: {e.InnerException}\n");
                Console.WriteLine($"Stack trace:\n\n{e.StackTrace}\n");
                Environment.Exit(-1);
            }

            Console.WriteLine("\nApplication run finish successfully\n");
        }

        /// <summary>
        /// Test input arguments, if an error is found, the application will be terminated
        /// </summary>
        /// <param name="args"></param>
        private static void TestInputArguments(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Missing arguments");
                PrintPleaseProvidePaths();
                Environment.Exit(-1);
            }
            else if (args.Length != 2)
            {
                Console.WriteLine($"Total arguments: {args.Length}, requested are two arguments");
                PrintPleaseProvidePaths();
                Environment.Exit(-1);
            }
            else if (String.IsNullOrEmpty(args[0]) || String.IsNullOrEmpty(args[1]))
            {
                if (String.IsNullOrEmpty(args[0]))
                {
                    Console.WriteLine("Source path is not provided");
                    PrintPleaseProvidePaths();
                }
                else if (String.IsNullOrEmpty(args[1]))
                {
                    Console.WriteLine("Destination path is not provided");
                    PrintPleaseProvidePaths();
                }
                Environment.Exit(-1);
            }
            else if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Source path is not valid");
                PrintPleaseProvidePaths();
                Environment.Exit(-1);
            }
        }

        private static void PrintPleaseProvidePaths()
        {
            Console.WriteLine("Please provide valid source and destination path.");
            Console.WriteLine("EXAMPLE:");
            Console.WriteLine(@"SortOutFiles.exe c:\Temp\Picture c:\Temp\PictureOut");
        }
    }
}