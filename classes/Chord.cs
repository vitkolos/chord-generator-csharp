class Chord {
    Parser parser;
    int _root;
    public int root => _root;
    int _bass = -1;
    public int bass => _bass;
    public int[] notes;

    public Chord(string input, Parser parser) {
        this.parser = parser;
        string trimmedInput = input.Trim();
        string[] inputParts = trimmedInput.Split('/');
        int accidental;
        _root = parser.ParseNoteWithAccidental(inputParts[0], out accidental);
        string variant = inputParts[0].Substring(accidental == 0 ? 1 : 2);
        int[] parsedChord = parser.ParseChordSuffix(variant);
        notes = new int[parsedChord.Length];

        for (int i = 0; i < notes.Length; i++) {
            notes[i] = Music.Modulo(parsedChord[i] + root);
        }

        if (inputParts.Length > 1 && inputParts[1] != "") {
            _bass = parser.ParseNoteWithAccidental(inputParts[1], out _bass);

            if (Array.IndexOf(notes, bass) == -1) {
                var newNotes = new int[notes.Length + 1];
                newNotes[0] = bass;
                notes.CopyTo(newNotes, 1);
                notes = newNotes;
            }
        }
    }
}
