class Instrument {
    public string name;
    public int[] strings;
    public int[] realStrings;
    public int frets = 19;


    public Instrument(Parser parser, string name, int frets, char[] stringNames, int[] octaves) {
        this.name = name;
        this.frets = frets;
        strings = new int[stringNames.Length];
        realStrings = new int[stringNames.Length];

        for (int i = 0; i < stringNames.Length; i++) {
            strings[i] = parser.ParseNote(stringNames[i]);
            realStrings[i] = strings[i] + octaves[i] * Music.NumberOfNotes;
        }
    }
}
