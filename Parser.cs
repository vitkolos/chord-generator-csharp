using System;

class Parser {
    Dictionary<string, int[]> chords;

    public Parser(string chordsFile) {
        chords = new Dictionary<string, int[]>();

        using (var reader = new System.IO.StreamReader(chordsFile)) {
            string? line;

            while ((line = reader.ReadLine()) != null) {
                string[] lineParts = line.Split(',');

                if (lineParts.Length == 2) {
                    string[] chordVariants = lineParts[0].Split(' ');
                    string[] chordNotes = lineParts[1].Split(' ');
                    int[] notes = new int[Music.ChordSize];

                    for (int i = 0; i < notes.Length; i++) {
                        notes[i] = -1;
                    }

                    for (int i = 0; i < chordNotes.Length; i++) {
                        switch (chordNotes[i]) {
                            case "t":
                                notes[i] = 10;
                                break;
                            case "e":
                                notes[i] = 11;
                                break;
                            default:
                                notes[i] = int.Parse(chordNotes[i]);
                                break;
                        }
                    }

                    foreach (var variant in chordVariants) {
                        chords.Add(variant, notes);
                    }
                }
            }
        }
    }

    public int ParseNote(char note) {
        var notes = new Dictionary<char, int>();
        notes.Add('C', 0);
        notes.Add('D', 2);
        notes.Add('E', 4);
        notes.Add('F', 5);
        notes.Add('G', 7);
        notes.Add('A', 9);
        notes.Add('H', 11);
        notes.Add('B', 10); // 10 or 11

        try {
            return notes[note];
        } catch (KeyNotFoundException) {
            throw new Exception("Akord byl chybně zadán.");
        }
    }

    public int ParseAccidental(char acc) {
        switch (acc) {
            case '#':
                return 1;
            case 'b':
                return -1;
            default:
                return 0;
        }
    }

    public int[] ParseChord(string input) {
        if (input == "") {
            input = "major";
        }

        try {
            return chords[input];
        } catch (KeyNotFoundException) {
            throw new Exception("Typ akordu byl chybně zadán.");
        }
    }
}
