class Chord {
    int _root;
    int _bass;
    int[] _notes;

    public int root => _root;
    public int bass => _bass;
    public int[] notes => _notes;

    public Chord(string input, Parser parser) {
        _root = parser.ParseNote(input[0]);
        int accidental = input.Length > 1 ? parser.ParseAccidental(input[1]) : 0;

        if (_root != 10) { // Bb equals B (in German notation)
            _root = Music.Modulo(_root + accidental);
        }

        string variant = input.Substring(accidental == 0 ? 1 : 2);
        int[] parsedChord = parser.ParseChord(variant);
        _notes = new int[parsedChord.Length];

        for (int i = 0; i < notes.Length; i++) {
            _notes[i] = Music.Modulo(parsedChord[i] + _root);
        }
    }
}
