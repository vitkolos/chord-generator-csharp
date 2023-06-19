/*
    Generátor hmatů akordů
    Vít Kološ, 1. ročník, IPP
    letní semestr 2022/2023
    NPRG031 Programování 2
*/

static class Program {
    public const bool RunInWindow = true;
    public const bool GraphicMode = true;
    public const string ConfigPath = "config.txt";
    public const int NumberOfDiagrams = 10;
    public const int DiagramWidth = 100;
    public const int DiagramHeight = 120;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    [STAThread]
    static void Main(string[] args) {
        bool inWindow, runTests, graphicMode;
        Program.ProcessArgs(args, out inWindow, out runTests, out graphicMode);

        if (runTests) {
            Tests.Run();
        } else if (inWindow) {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.Run(new WindowsForm(graphicMode));
        } else {
            ConsoleApp.Run(args);
        }
    }

    public static void ProcessArgs(string[] args, out bool inWindow, out bool runTests, out bool graphicMode) {
        inWindow = Program.RunInWindow;
        graphicMode = Program.GraphicMode;
        runTests = false;

        if (Array.IndexOf(args, "tests") != -1) {
            runTests = true;
        } else if (Array.IndexOf(args, "console") != -1) {
            inWindow = false;
        } else if (Array.IndexOf(args, "window") != -1) {
            inWindow = true;
        }

        if (Array.IndexOf(args, "text") != -1) {
            graphicMode = false;
        }
    }

    public static int SumTo(int[] array, int to) {
        int sum = 0;

        for (int i = 0; i < to; i++) {
            sum += array[i];
        }

        return sum;
    }
}

static class Music {
    public const int NumberOfNotes = 12;
    public const int AvailableFingers = 4;
    public const int DefaultNumberOfFrets = 20;
    public const int HighestNote = 200;
    public const int DefaultOctave = 4;

    public static int Modulo(int note) {
        int barreOverflowPrevention = 10; // we suppose every instrument has less than 120 frets
        return (note + Music.NumberOfNotes * barreOverflowPrevention) % Music.NumberOfNotes;
    }
}
