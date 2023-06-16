public class Tests {
    public static void Run() {
        var parser = new Parser("config.txt");
        ShouldEqual(parser.ParseNote('C'), 0, "C note");
        int[] major = { 0, 4, 7 };
        ShouldEqual(parser.ParseChord(""), major, "major chord");
        var chord = new Chord("D7", parser);
        int[] d7 = { 2, 6, 9, 0 };
        ShouldEqual(chord.root, 2, "D7 root");
        ShouldEqual(chord.notes, d7, "D7 chord");
    }

    static void ShouldEqual(int a, int b, string info) {
        Console.Write(info);

        if (a != b) {
            throw new Exception(a.ToString() + " does not equal " + b.ToString());
        }

        Console.WriteLine(" OK");
    }

    static void ShouldEqual(int[] a, int[] b, string info) {
        Console.WriteLine(info);

        if (a.Length != b.Length) {
            throw new Exception("one array is longer");
        }

        for (int i = 0; i < a.Length; i++) {
            ShouldEqual(a[i], b[i], "- item " + i.ToString());
        }

        Console.WriteLine(" OK");
    }
}
