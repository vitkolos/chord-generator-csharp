class Parser {
    Dictionary<string, int[]> chordSuffixes;
    public Dictionary<string, Instrument> instruments;
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
        char[] stringNames;
        int[] octaves;

        if (defaultInstrumentName == "") {
            defaultInstrumentName = lineParts[0];
        }

        if (lineParts.Length > 2) {
            frets = int.Parse(lineParts[2]);
        }

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
            chordSuffixes.Add(variant, notes);
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
            throw new Exception(Strings.BadChord);
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
            return chordSuffixes[input];
        } catch (KeyNotFoundException) {
            throw new Exception(Strings.BadChordType);
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
}
