class Program {
    public static bool RunInWindow = true;
    public static bool GraphicMode = true;
    public const string ConfigPath = "config.txt";
    public const int NumberOfDiagrams = 10;
    public const int DiagramWidth = 100;
    public const int DiagramHeight = 120;

    public static int SumTo(int[] array, int to) {
        int sum = 0;

        for (int i = 0; i < to; i++) {
            sum += array[i];
        }

        return sum;
    }

    public static void ProcessArgs(string[] args, out bool inWindow, out bool runTests) {
        inWindow = Program.RunInWindow;
        runTests = false;

        if (Array.IndexOf(args, "tests") != -1) {
            runTests = true;
        } else if (Array.IndexOf(args, "console") != -1) {
            inWindow = false;
        } else if (Array.IndexOf(args, "window") != -1) {
            inWindow = true;
        }

        if (Array.IndexOf(args, "text") != -1) {
            Program.GraphicMode = false;
        }
    }
}

class Music {
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
