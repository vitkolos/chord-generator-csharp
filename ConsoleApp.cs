class ConsoleApp {
    public static void Run(string[] args) {
        var parser = new Parser(Program.ConfigPath);
        Instrument instrument = parser.GetDefaultInstrument();
        string? input;

        while (true) {
            Console.WriteLine("Podporovane nastroje: {0}", string.Join(", ", parser.GetInstrumentsNames()));
            Console.WriteLine($"Zvoleny nastroj: {instrument.name}");
            Console.Write("Zadejte akord nebo nastroj: ");
            input = Console.ReadLine();

            if (input == "" || input == null) {
                return;
            } else if (input == "run tests") {
                Tests.Run();
                continue;
            }

            Instrument? nextInstrument = parser.ParseInstrument(input);

            if (nextInstrument != null) {
                instrument = nextInstrument;
                continue;
            }

            var chord = new Chord(input, parser);
            var positions = new Positions(instrument, chord);

            positions.list.Sort(Position.PositionComparer);
            var i = positions.list.Count + 1;

            foreach (var position in positions.list) {
                if (--i <= Program.NumberOfDiagrams) {
                    Console.WriteLine();
                    Console.WriteLine(i + ". " + position.GetText() + " " + position.Score);
                    Console.Write(position.GetDiagram());
                    Console.WriteLine();
                }
            }

            // Console.WriteLine($"nalezeno {positions.list.Count} variant akordu {input}");
            Console.WriteLine("akord " + input);
        }
    }
}
