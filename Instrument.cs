class Instrument {
    public string name = "ukulele";
    public int[] strings;
    public int[] realStrings;
    public int frets = 19;


    public Instrument(Parser parser) {
        string input = "GCEA";
        // string input = "E2 A2 D3 G3 H3 E4";
        char[] stringNames;
        int[] octaves;

        if (input.Contains(' ')) {
            string[] s = input.Split(' ');
            stringNames = new char[s.Length];
            octaves = new int[s.Length];

            for (int i = 0; i < s.Length; i++) {
                stringNames[i] = s[i][0];

                if (s[i].Length == 2) {
                    octaves[i] = s[i][1] - '0';
                }
            }
        } else {
            octaves = new int[input.Length];
            stringNames = input.ToCharArray();
        }

        strings = new int[stringNames.Length];
        realStrings = new int[stringNames.Length];

        for (int i = 0; i < stringNames.Length; i++) {
            strings[i] = parser.ParseNote(stringNames[i]);
            realStrings[i] = strings[i] + octaves[i] * Music.NumberOfNotes;
        }

        Console.WriteLine(string.Join(',', strings));
        Console.WriteLine(string.Join(',', realStrings));
    }
}
