static class Tests {
    public static void Run() {
        var parser = new Parser("testing-config.txt");

        ShouldEqual(parser.ParseNote('C'), 0, "C note");

        int[] major = { 0, 4, 7 };
        ShouldEqual(parser.ParseChordSuffix(""), major, "major chord");

        var chord = new Chord("D7", parser);
        int[] d7 = { 2, 6, 9, 0 };
        ShouldEqual(chord.root, 2, "D7 root");
        ShouldEqual(chord.notes, d7, "D7 chord");

        var chordA = new Chord("Emaj", parser);
        var chordB = new Chord("Emaj7", parser);
        ShouldEqual(chordA.root, chordB.root, "maj = maj7 (root)");
        ShouldEqual(chordA.notes, chordB.notes, "maj = maj7 (notes)");

        Instrument? nInstrument = parser.ParseInstrument("guitar");
        ShouldNotBeNull(nInstrument, "instrument");
        Instrument instrument = nInstrument!;
        ShouldEqual(instrument.frets, Music.DefaultNumberOfFrets, "default frets");
        ShouldEqual(instrument.strings[0], 4, "first string");
        ShouldEqual(instrument.realStrings[0], 28, "first real string");

        Instrument uke = parser.ParseInstrument("ukulele")!;
        ShouldEqual(uke.frets, 19, "uke frets");
        ShouldEqual(uke.strings[1], 0, "second string");

        TestChord(parser, "guitar", "D7", "0 -1,-1,0,2,1,2");
        TestChord(parser, "ukulele", "C", "0 0,0,0,3");
        TestChord(parser, "ukulele", "G", "0 0,2,3,2");
        TestChord(parser, "guitar", "C/E", "0 0,3,2,0,1,0");
        TestChord(parser, "guitar", "Dm", "0 -1,-1,0,2,3,1");
    }

    static void ShouldEqual(int a, int b, string info) {
        Console.Write(info);

        if (a != b) {
            throw new ArgumentException(a.ToString() + " does not equal " + b.ToString());
        }

        Console.WriteLine(" OK");
    }

    static void ShouldEqual(string a, string b, string info) {
        Console.Write(info);

        if (a != b) {
            throw new ArgumentException(a + " does not equal " + b);
        }

        Console.WriteLine(" OK");
    }

    static void ShouldEqual(int[] a, int[] b, string info) {
        Console.WriteLine(info);

        if (a.Length != b.Length) {
            throw new ArgumentException("one array is longer");
        }

        for (int i = 0; i < a.Length; i++) {
            ShouldEqual(a[i], b[i], "- item " + i.ToString());
        }

        Console.WriteLine(" OK");
    }

    static void ShouldNotBeNull(object? a, string info) {
        Console.Write(info);

        if (a is null) {
            throw new ArgumentException("is null");
        }

        Console.WriteLine(" is not null OK");
    }

    static void TestChord(Parser parser, string instrumentName, string chordName, string diagram) {
        var chord = new Chord(chordName, parser);
        Instrument instrument = parser.ParseInstrument(instrumentName)!;
        var positions = new Positions(instrument, chord);
        positions.list.Sort(Position.PositionComparer);
        positions.list.Reverse();
        ShouldEqual(positions.list[0].GetText(), diagram, chordName + " diagram");
    }
}
