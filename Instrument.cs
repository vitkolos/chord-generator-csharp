class Instrument {
    string _name;
    int _frets;
    int[] _strings;
    int[] _realStrings;

    public string name => _name;
    public int frets => _frets;
    public int[] strings => _strings;
    public int[] realStrings => _realStrings;

    public Instrument(Parser parser, string name, int frets, char[] stringNames, int[] octaves) {
        _name = name;
        _frets = frets;
        _strings = new int[stringNames.Length];
        _realStrings = new int[stringNames.Length];

        for (int i = 0; i < stringNames.Length; i++) {
            _strings[i] = parser.ParseNote(stringNames[i]);
            _realStrings[i] = _strings[i] + octaves[i] * Music.NumberOfNotes;
        }
    }
}
