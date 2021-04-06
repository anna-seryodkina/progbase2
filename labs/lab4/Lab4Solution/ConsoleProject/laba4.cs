using System;
using System.Drawing;
using System.Diagnostics;
using ProgbaseLab.ImageEditor.Pixel;
using ProgbaseLab.ImageEditor.Common;
using ProgbaseLab.ImageEditor.Fast;

static class Program
{
    static void Main(string[] args)
    {
        ArgsProcessor.Run(args);
    }
}


static class ArgsProcessor
{
    private static void ValidateModule(string module)
    {
        string[] supportedModules = new string[] {"pixel", "fast"};
        for(int i = 0; i < supportedModules.Length; i++)
        {
            if(supportedModules[i] == module)
            {
                return;
            }
        }
        throw new ArgumentException($"Not supported module: {module}");
    }

    private static void ValidateArgsLength(int length)
    {
        if(length < 4)
        {
            throw new ArgumentException($"Not enough command line arguments. Expected more than 3, got {length}");
        }
    }

    private static void ValidateInputFile(string file)
    {
        if(!System.IO.File.Exists(file))
        {
            throw new ArgumentException($"File does not exist: {file}");
        }
    }

    private static void ValidateCommand(string command)
    {
        string[] supportedCommands = new string[] {"crop", "rotate-right-90", "extract-red", "grayscale", "change-hue"};
        for(int i = 0; i < supportedCommands.Length; i++)
        {
            if(supportedCommands[i] == command)
            {
                return;
            }
        }
        throw new ArgumentException($"Not supported command: {command}");
    }

    private static Rectangle ParseRectangle(string rectFormat)
    {
        // crop {width}x{height}+{left}+{top}
        // 1 split: {width} and {height}+{left}+{top}
        // 2 split: {height} and {left} and {top}
        int width;
        int height;
        int top;
        int left;
        //
        string[] firstSplit = rectFormat.Split("x");
        string[] secondSplit = firstSplit[1].Split("+");
        //
        if(!int.TryParse(firstSplit[0], out width))
        {
            throw new ArgumentException("Invalid arguments for crop.");
        }
        if(!int.TryParse(secondSplit[0], out height))
        {
            throw new ArgumentException("Invalid arguments for crop.");
        }
        if(!int.TryParse(secondSplit[1], out left))
        {
            throw new ArgumentException("Invalid arguments for crop.");
        }
        if(!int.TryParse(secondSplit[2], out top))
        {
            throw new ArgumentException("Invalid arguments for crop.");
        }
        //
        Rectangle rect = new Rectangle(left, top, width, height);
        return rect;
    }

    struct ProgramArgs
    {
        public string module;
        public string inputFile;
        public string outputFile;
        public string command;
        public string[] otherArgs;
    }

    private static ProgramArgs ParseArguments(string[] args)
    {
        ValidateArgsLength(args.Length);

        string module = args[0];
        ValidateModule(module);
        
        string inputFile = args[1];
        ValidateInputFile(inputFile);

        string outputFile = args[2];
        string command = args[3];
        ValidateCommand(command);

        ProgramArgs programArgs = new ProgramArgs();
        programArgs.module = module;
        programArgs.inputFile = inputFile;
        programArgs.outputFile = outputFile;
        programArgs.command = command;
        programArgs.otherArgs = new string[args.Length-4];
        for(int i = 0; i < programArgs.otherArgs.Length; i++)
        {
            programArgs.otherArgs[i] = args[i + 4];
        }
        return programArgs;
    }

    public static void Run(string[] args)
    {
        ProgramArgs programArgs = ParseArguments(args);
        Bitmap inputBitmap = new Bitmap(programArgs.inputFile);

        IImageEditor imageEditor;
        if(programArgs.module == "pixel")
        {
            imageEditor = new Pixel();
        }
        else
        {
            imageEditor = new Fast();
        }

        switch(programArgs.command)
        {
            case "crop":
            {
                ProcessCrop(programArgs, inputBitmap, imageEditor);
                break;
            }
            case "rotate-right-90":
            {
                ProcessRotate(programArgs, inputBitmap, imageEditor);
                break;
            }
            case "extract-red":
            {
                ProcessExtract(programArgs, inputBitmap, imageEditor);
                break;
            }
            case "grayscale":
            {
                ProcessGrayscale(programArgs, inputBitmap, imageEditor);
                break;
            }
            case "change-hue":
            {
                ProcessHue(programArgs, inputBitmap, imageEditor);
                break;
            }
        }
    }

    private static void ProcessHue(ProgramArgs programArgs, Bitmap inputBitmap, IImageEditor imageEditor)
    {
        if(programArgs.otherArgs.Length < 1)
        {
            throw new ArgumentException("change-hue command should have hue value");
        }
        int hue;
        if(!int.TryParse(programArgs.otherArgs[0], out hue))
        {
            throw new ArgumentException($"invalid hue: {hue}");
        }
        Stopwatch commandWatch = new Stopwatch();
        commandWatch.Start();
        Bitmap outputBitmap = imageEditor.ChangeHue(inputBitmap, hue);
        commandWatch.Stop();

        Console.WriteLine($"ChangeHue finished in {commandWatch.ElapsedMilliseconds} ms");

        outputBitmap.Save(programArgs.outputFile);
    }

    private static void ProcessGrayscale(ProgramArgs programArgs, Bitmap inputBitmap, IImageEditor imageEditor)
    {
        Stopwatch commandWatch = new Stopwatch();
        commandWatch.Start();
        Bitmap outputBitmap = imageEditor.Grayscale(inputBitmap);
        commandWatch.Stop();

        Console.WriteLine($"Grayscale finished in {commandWatch.ElapsedMilliseconds} ms");

        outputBitmap.Save(programArgs.outputFile);
    }

    private static void ProcessExtract(ProgramArgs programArgs, Bitmap inputBitmap, IImageEditor imageEditor)
    {
        Stopwatch commandWatch = new Stopwatch();
        commandWatch.Start();
        Bitmap outputBitmap = imageEditor.ExtractRed(inputBitmap);
        commandWatch.Stop();

        Console.WriteLine($"ExtractRed finished in {commandWatch.ElapsedMilliseconds} ms");

        outputBitmap.Save(programArgs.outputFile);
    }

    private static void ProcessRotate(ProgramArgs programArgs, Bitmap inputBitmap, IImageEditor imageEditor)
    {
        Stopwatch commandWatch = new Stopwatch();
        commandWatch.Start();
        Bitmap outputBitmap = imageEditor.RotateRight90(inputBitmap);
        commandWatch.Stop();

        Console.WriteLine($"RotateRight90 finished in {commandWatch.ElapsedMilliseconds} ms");

        outputBitmap.Save(programArgs.outputFile);
    }

    private static void ProcessCrop(ProgramArgs programArgs, Bitmap inputBitmap, IImageEditor imageEditor)
    {
        if(programArgs.otherArgs.Length < 1)
        {
            throw new ArgumentException("Crop command should have dimentions arguments");
        }
        string cropArgs = programArgs.otherArgs[0];
        Rectangle cropRect = ParseRectangle(cropArgs);

        Stopwatch commandWatch = new Stopwatch();
        commandWatch.Start();
        Bitmap outputBitmap = imageEditor.Crop(inputBitmap, cropRect);
        commandWatch.Stop();

        Console.WriteLine($"Crop finished in {commandWatch.ElapsedMilliseconds} ms");

        outputBitmap.Save(programArgs.outputFile);
    }
}