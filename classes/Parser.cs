class Parser {
    Dictionary<string, int[]> chordSuffixes;
    Dictionary<string, Instrument> instruments;
    string defaultInstrumentName;

    public Parser(string configFile) {
        chordSuffixes = new Dictionary<string, int[]>();
        instruments = new Dictionary<string, Instrument>();
        bool inInstrumentSection = true;
        defaultInstrumentName = "";

        using (var reader = new System.IO.StreamReader(configFile)) {
            string? line;

            while ((line = reader.ReadLine()) != null) {
                if (line == "") {
                    inInstrumentSection = false;
                    continue;
                }

                string[] lineParts = line.Split(',');

                if (lineParts.Length > 1) {
                    if (inInstrumentSection) {
                        ParseInstrumentFromLine(lineParts);
                    } else {
                        ParseChordSuffixesFromLine(lineParts);
                    }
                }
            }
        }
    }

    void ParseInstrumentFromLine(string[] lineParts) {
        string input = lineParts[1];
        int frets = Music.DefaultNumberOfFrets;
        string[] stringNames;
        int[] octaves;

        if (defaultInstrumentName == "") {
            defaultInstrumentName = lineParts[0];
        }

        if (lineParts.Length > 2 && lineParts[2] != "") {
            frets = int.Parse(lineParts[2]);
        }

        if (input.Contains(' ')) {
            string[] s = input.Split(' ');
            stringNames = new string[s.Length];
            octaves = new int[s.Length];

            for (int i = 0; i < octaves.Length; i++) {
                octaves[i] = Music.DefaultOctave;
            }

            for (int i = 0; i < s.Length; i++) {
                int firstNumericIndex = s[i].IndexOfAny("1234567890".ToCharArray());

                if (firstNumericIndex == -1) {
                    stringNames[i] = s[i];
                } else {
                    stringNames[i] = s[i].Substring(0, firstNumericIndex);
                    octaves[i] = int.Parse(s[i].Substring(firstNumericIndex));
                }
            }
        } else {
            octaves = new int[input.Length];

            for (int i = 0; i < octaves.Length; i++) {
                octaves[i] = Music.DefaultOctave;
            }

            stringNames = input.ToCharArray().Select(c => c.ToString()).ToArray();
        }

        var instrument = new Instrument(this, lineParts[0], frets, stringNames, octaves);
        instruments.Add(instrument.name, instrument);
    }

    void ParseChordSuffixesFromLine(string[] lineParts) {
        string[] chordVariants = lineParts[0].Split(' ');
        string[] chordNotes;

        if (lineParts[1].Contains(' ')) {
            chordNotes = lineParts[1].Split(' ');
        } else {
            chordNotes = lineParts[1].ToCharArray().Select(c => c.ToString()).ToArray();
        }

        int[] notes = new int[chordNotes.Length];

        for (int i = 0; i < chordNotes.Length; i++) {
            switch (chordNotes[i]) {
                case "t":
                case "A":
                    notes[i] = 10;
                    break;
                case "e":
                case "B":
                    notes[i] = 11;
                    break;
                default:
                    notes[i] = int.Parse(chordNotes[i]);
                    break;
            }
        }

        foreach (var variant in chordVariants) {
            chordSuffixes.Add(variant, notes);
        }
    }

    public int ParseNoteWithAccidental(string input, out int accidental) {
        int note = ParseNote(input[0]);
        accidental = input.Length > 1 ? ParseAccidental(input[1]) : 0;

        if (note != 10) { // Bb equals B (in German notation)
            note = Music.Modulo(note + accidental);
        }

        return note;
    }

    public int ParseNoteWithAccidental(string input) {
        int accidental;
        return ParseNoteWithAccidental(input, out accidental);
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
            throw new FormatException(Strings.BadChord);
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

    public int[] ParseChordSuffix(string input) {
        if (input == "") {
            input = "major";
        }

        try {
            return chordSuffixes[input];
        } catch (KeyNotFoundException) {
            throw new FormatException(Strings.BadChordType);
        }
    }

    public Instrument? ParseInstrument(string input) {
        try {
            return instruments[input];
        } catch (KeyNotFoundException) {
            return null;
        }
    }

    public Instrument GetDefaultInstrument() {
        return instruments[defaultInstrumentName];
    }

    public List<string> GetInstrumentsNames() {
        return instruments.Keys.ToList();
    }
}
