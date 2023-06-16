using System;

class Program {
    static void Main(string[] args) {
        var parser = new Parser("config.txt");
        Instrument instrument = parser.ParseInstrument()!;
        string? input;

        while (true) {
            Console.WriteLine("Podporovane nastroje: {0}", string.Join(", ", parser.instruments.Keys));
            Console.WriteLine($"Zvoleny nastroj: {instrument.name}");
            Console.Write("Zadejte akord nebo nastroj: ");
            input = Console.ReadLine();

            if (input == "" || input == null) {
                return;
            }

            Instrument? nextInstrument = parser.ParseInstrument(input);

            if (nextInstrument != null) {
                instrument = nextInstrument;
                continue;
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

                // Console.WriteLine($"nalezeno {positions.list.Count} variant akordu {input}");
                Console.WriteLine("akord " + input);
            }
        }
    }
}

class Music {
    public const int NumberOfNotes = 12;
    public const int AvailableFingers = 4;
    public const int DefaultNumberOfFrets = 20;
    public const int HighestNote = 200;

    public static int Modulo(int note) {
        int barreOverflowPrevention = 10; // we suppose every instrument has less than 120 frets
        return (note + Music.NumberOfNotes * barreOverflowPrevention) % Music.NumberOfNotes;
    }
}
