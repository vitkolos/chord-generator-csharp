class Chord {
    public int root;
    public int[] notes;

    public Chord(string input, Parser parser) {
        root = parser.ParseNote(input[0]);
        int accidental = input.Length > 1 ? parser.ParseAccidental(input[1]) : 0;

        if (root != 10) { // Bb equals B (in German notation)
            root = Music.Modulo(root + accidental);
        }

        string variant = input.Substring(accidental == 0 ? 1 : 2);
        int[] parsedChord = parser.ParseChord(variant);
        notes = new int[Array.IndexOf(parsedChord, -1)];

        for (int i = 0; i < notes.Length; i++) {
            notes[i] = Music.Modulo(parsedChord[i] + root);
        }

        Console.WriteLine(root.ToString() + "x" + string.Join(',', notes));
    }
}