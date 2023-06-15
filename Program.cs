using System;

class Program {
    static void Main(string[] args) {
        var parser = new Parser("chords.txt");
        var instrument = new Instrument(parser);
        Console.Write("Zadejte akord: ");
        string? input = Console.ReadLine();

        if (input != null) {
            var chord = new Chord(input, parser);
            var positions = new Positions(instrument, chord);

            if (positions.list != null) {
                positions.list.Sort(Position.PositionComparer);

                foreach (var position in positions.list) {
                    position.PrintDiagram();
                }
            }
        }
    }
}

class Music {
    public const int NumberOfNotes = 12;
    public const int AvailableFingers = 4;
    public const int ChordSize = 7;
    public const int HighestNote = 200;

    public static int Modulo(int note) {
        int barreOverflowPrevention = 5; // we suppose any instrument has less than 60 frets
        return (note + Music.NumberOfNotes * barreOverflowPrevention) % Music.NumberOfNotes;
    }
}
