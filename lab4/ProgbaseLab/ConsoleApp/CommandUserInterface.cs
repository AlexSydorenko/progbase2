using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace lab4
{
    class CommandUserInterface
    {
        public static void ProcessCommand(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Minimum 4 command line arguments!");
                Environment.Exit(1);
            }

            IImageEditor imgEditor = null;
            if (args[0] == "pixel")
            {
                imgEditor = new PixelImageEditor();
            }
            else if (args[0] == "fast")
            {
                imgEditor = new FastImageEditor();
            }
            else
            {
                Console.Error.WriteLine($"Invalid mode: {args[0]}! `pixel` or `fast`!");
                Environment.Exit(1);
            }

            if (!File.Exists(args[1]))
            {
                Console.Error.WriteLine($"Non-existing input file: {args[1]}!");
                Environment.Exit(1);
            }

            string command = "";
            for (int i = 1; i < args.Length; i++)
            {
                if (i == args.Length - 1)
                {
                    command += args[i];
                    break;
                }
                command += args[i] + " ";
            }

            if (command.Contains("crop"))
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                ProcessCrop(command, imgEditor, stopWatch);
            }
            else if (command.Contains("RotateLeft90"))
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                ProcessRotateLeft90(command, imgEditor, stopWatch);
            }
            else if (command.Contains("ExtractGreen"))
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                ProcessExtractGreen(command, imgEditor, stopWatch);
            }
            else if (command.Contains("Sepia"))
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                ProcessSepia(command, imgEditor, stopWatch);
            }
            else if (command.Contains("ChangeSaturation"))
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                ProcessChangeSaturation(command, imgEditor, stopWatch);
            }
        }


        private static void ProcessCrop(string command, IImageEditor editor, Stopwatch stopWatch)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands.Length != 7)
            {
                Console.Error.WriteLine($"Incorrect number of command line args! Shoul be `7`, but have `{subcommands.Length}`!");
                Environment.Exit(1);
            }
            if (subcommands[2] != "crop")
            {
                Console.Error.WriteLine($"Incorrect command: `{subcommands[0]}`! Should be `crop`!");
                Environment.Exit(1);
            }

            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;

            if (!int.TryParse(subcommands[3], out width) || !int.TryParse(subcommands[4], out height) ||
                !int.TryParse(subcommands[5], out left) || !int.TryParse(subcommands[6], out top))
            {
                Console.Error.WriteLine("Mistakes in crop params!");
                Environment.Exit(1);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap croppedImage = editor.Crop(new Bitmap(subcommands[0]), width, height, left, top);
            
            sw.Stop();
            Console.WriteLine($"Time Elapsed (func): {sw.Elapsed}");


            stopWatch.Stop();
            Console.WriteLine($"Time Elapsed (command): {stopWatch.Elapsed}");

            croppedImage.Save(subcommands[1]);
            Console.WriteLine($"Cropped image was successfully saved into file `{subcommands[1]}`!");
            Environment.Exit(0);
        }

        private static void ProcessRotateLeft90(string command, IImageEditor editor, Stopwatch stopWatch)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands.Length != 3)
            {
                Console.Error.WriteLine($"Incorrect number of command line args! Shoul be `7`, but have `{subcommands.Length}`!");
                Environment.Exit(1);
            }
            if (subcommands[2] != "RotateLeft90")
            {
                Console.Error.WriteLine($"Invalid command: `{subcommands[2]}`! Should be `RotateLeft90`!");
                Environment.Exit(1);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap rotatedImage = editor.RotateLeft90(new Bitmap(subcommands[0]));
            
            sw.Stop();
            Console.WriteLine($"Time Elapsed (func): {sw.Elapsed}");

            stopWatch.Stop();
            Console.WriteLine($"Time Elapsed (command): {stopWatch.Elapsed}");

            rotatedImage.Save(subcommands[1]);
            Console.WriteLine($"Rotated image was successfully saved into file `{subcommands[1]}`!");
            Environment.Exit(0);
        }

        private static void ProcessExtractGreen(string command, IImageEditor editor, Stopwatch stopWatch)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands.Length != 3)
            {
                Console.Error.WriteLine($"Incorrect number of command line args! Shoul be `3`, but have `{subcommands.Length}`!");
                Environment.Exit(1);
            }
            if (subcommands[2] != "ExtractGreen")
            {
                Console.Error.WriteLine($"Invalid command: `{subcommands[2]}`! Should be `ExtractGreen`!");
                Environment.Exit(1);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap greenImage = editor.ExtractGreen(new Bitmap(subcommands[0]));
            
            sw.Stop();
            Console.WriteLine($"Time Elapsed (func): {sw.Elapsed}");

            stopWatch.Stop();
            Console.WriteLine($"Time Elapsed (command): {stopWatch.Elapsed}");

            greenImage.Save(subcommands[1]);
            Console.WriteLine($"Extract green image was successfully saved into file `{subcommands[1]}`!");
            Environment.Exit(0);
        }

        private static void ProcessSepia(string command, IImageEditor editor, Stopwatch stopWatch)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands.Length != 3)
            {
                Console.Error.WriteLine($"Incorrect number of command line args! Shoul be `3`, but have `{subcommands.Length}`!");
                Environment.Exit(1);
            }
            if (subcommands[2] != "Sepia")
            {
                Console.Error.WriteLine($"Invalid command: `{subcommands[2]}`! Should be `Sepia`!");
                Environment.Exit(1);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap sepiaImage = editor.Sepia(new Bitmap(subcommands[0]));
            
            sw.Stop();
            Console.WriteLine($"Time Elapsed (func): {sw.Elapsed}");

            stopWatch.Stop();
            Console.WriteLine($"Time Elapsed (command): {stopWatch.Elapsed}");

            sepiaImage.Save(subcommands[1]);
            Console.WriteLine($"Sepia image was successfully saved into file `{subcommands[1]}`!");
            Environment.Exit(0);
        }

        private static void ProcessChangeSaturation(string command, IImageEditor editor, Stopwatch stopWatch)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands.Length != 4)
            {
                Console.Error.WriteLine($"Incorrect number of command line args! Shoul be `4`, but have `{subcommands.Length}`!");
                Environment.Exit(1);
            }
            if (subcommands[2] != "ChangeSaturation")
            {
                Console.Error.WriteLine($"Invalid command: `{subcommands[2]}`! Should be `ChangeSaturation`!");
                Environment.Exit(1);
            }
            int saturation = 0;
            if (!int.TryParse(subcommands[3], out saturation))
            {
                Console.Error.WriteLine($"Saturation level is integer from 0 to 200!");
                Environment.Exit(1);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap saturationImage = editor.ChangeSaturation(new Bitmap(subcommands[0]), saturation);
            
            sw.Stop();
            Console.WriteLine($"Time Elapsed (func): {sw.Elapsed}");

            stopWatch.Stop();
            Console.WriteLine($"Time Elapsed (command): {stopWatch.Elapsed}");

            saturationImage.Save(subcommands[1]);
            Console.WriteLine($"Image with changed saturation was successfully saved into file `{subcommands[1]}`!");
            Environment.Exit(0);
        }
    }
}
