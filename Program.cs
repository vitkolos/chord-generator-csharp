using System;

class Program {
    static void Main(string[] args) {
        var parser = new Parser("chords.txt");
        var instrument = new Instrument(parser);
        string? input;

        while (true) {
            Console.Write("Zadejte akord: ");
            input = Console.ReadLine();

            if (input == "" || input == null) {
                return;
            }

            var chord = new Chord(input, parser);
            var positions = new Positions(instrument, chord);

            if (positions.list != null) {
                positions.list.Sort(Position.PositionComparer);
                var i = positions.list.Count + 1;

                foreach (var position in positions.list) {
                    if (--i <= 20) {
                        Console.WriteLine();
                        Console.Write(i + ". ");
                        position.PrintDiagram();
                        Console.WriteLine();
                    }
                }

                Console.WriteLine($"nalezeno {positions.list.Count} variant akordu {input}");
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
