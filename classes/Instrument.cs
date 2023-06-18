class Instrument {
    string _name;
    public string name => _name;
    int _frets;
    public int frets => _frets;
    public int[] strings;
    public int[] realStrings;

    public Instrument(Parser parser, string name, int frets, string[] stringNames, int[] octaves) {
        _name = name;
        _frets = frets;
        strings = new int[stringNames.Length];
        realStrings = new int[stringNames.Length];

        for (int i = 0; i < stringNames.Length; i++) {
            strings[i] = parser.ParseNoteWithAccidental(stringNames[i]);
            realStrings[i] = strings[i] + octaves[i] * Music.NumberOfNotes;
        }
    }
}
