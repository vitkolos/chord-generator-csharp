class Positions {
    public List<Position> list;
    Instrument instrument;
    Chord chord;

    public Positions(Instrument instrument, Chord chord) {
        list = new List<Position>();
        this.instrument = instrument;
        this.chord = chord;
        // stringPos … finger positions on strings
        int[] stringPos = new int[instrument.strings.Length];
        int[] notes = new int[instrument.strings.Length];
        int remainingNotes = chord.notes.Length;

        for (int barre = 0; barre <= instrument.frets; barre++) {
            int b = (barre == 0) ? 0 : 1;
            FillNextString(barre, stringPos, notes, 0, remainingNotes, Music.AvailableFingers - b);
        }
    }

    void FillNextString(int barre, int[] stringPos, int[] notes, int s, int remainingNotes, int remainingFingers) {
        int remainingStrings = instrument.strings.Length - s;

        if (remainingStrings == 0) {
            if (remainingNotes == 0 && remainingFingers >= 0 && (barre == 0 || Array.IndexOf(stringPos, barre) != -1)) {
                list.Add(new Position(instrument, chord, stringPos, barre));
            }

            return;
        }

        // bool mutedCond = (remainingStrings > remainingNotes && (s == 0 || stringPos[s - 1] == Position.mutedString)) || (remainingNotes == 0);
        bool mutedCond = (remainingStrings > remainingNotes && (s == 0 || stringPos[s - 1] == Position.mutedString));
        // bool mutedCond = true;

        if (mutedCond) {
            stringPos[s] = Position.mutedString;
            notes[s] = Position.mutedString;
            FillNextString(barre, stringPos, notes, s + 1, remainingNotes, remainingFingers);
        }

        for (int i = 0; i < chord.notes.Length; i++) {
            stringPos[s] = Music.Modulo(chord.notes[i] - instrument.strings[s] - barre) + barre;

            if (stringPos[s] > instrument.frets) {
                continue;
            }

            notes[s] = chord.notes[i];
            int r = (Array.IndexOf(notes, chord.notes[i]) == s) ? 1 : 0;
            int f = (stringPos[s] != barre) ? 1 : 0;
            FillNextString(barre, stringPos, notes, s + 1, remainingNotes - r, remainingFingers - f);

            // check other octaves
            while (stringPos[s] + Music.NumberOfNotes <= instrument.frets) {
                stringPos[s] = stringPos[s] + Music.NumberOfNotes;
                FillNextString(barre, stringPos, notes, s + 1, remainingNotes - r, remainingFingers - 1);
            }
        }
    }
}

class Position {
    public const int emptyString = 0;
    public const int mutedString = -1;
    int barre = 0;
    int score = 0;
    Instrument instrument;
    Chord chord;
    int[] stringPos;

    public Position(Instrument instrument, Chord chord, int[] stringPos, int barre) {
        this.barre = barre;
        this.instrument = instrument;
        this.chord = chord;
        this.stringPos = new int[instrument.strings.Length];
        stringPos.CopyTo(this.stringPos, 0);
        this.score = this.getScore();
    }

    public void PrintDiagram() {
        Console.WriteLine(barre.ToString() + " " + string.Join(',', stringPos) + " " + score);
    }

    int getScore() {
        int maxPos = -1;
        int minPos = instrument.frets;
        int numberOfMuted = 0;
        int lowestTone = Music.HighestNote;
        int hasBarre = (barre != 0) ? 1 : 0;
        int numberOfFingers = hasBarre;
        int firstString = instrument.strings.Length;
        int lastString = -1;

        for (int i = 0; i < stringPos.Length; i++) {
            int pos = stringPos[i];

            if (pos == mutedString) {
                numberOfMuted++;
            }

            if (pos != emptyString && pos != mutedString) {
                if (pos != barre) {
                    numberOfFingers++;
                    lastString = i;

                    if (firstString > i) {
                        firstString = i;
                    }
                }

                if (pos < minPos) {
                    minPos = pos;
                }
            }


            if (pos > maxPos) {
                maxPos = pos;
            }

            if (pos != mutedString && instrument.realStrings[i] + pos < lowestTone) {
                lowestTone = instrument.realStrings[i] + pos;
            }
        }

        if (maxPos < minPos) {
            maxPos = 0;
            minPos = 0;
        }

        if (lastString < firstString) {
            lastString = 0;
            firstString = 0;
        }

        int noRoot = (Music.Modulo(lowestTone) != chord.root) ? 1 : 0;
        int hasMuted = (numberOfMuted > 0) ? 1 : 0;
        int hasTooManyMuted = (numberOfMuted >= instrument.strings.Length / 2) ? 1 : 0;
        hasMuted *= (instrument.strings.Length > 4) ? 1 : 2;

        int s = 1000;

        s -= hasBarre * 10;
        s -= (maxPos - minPos) * 10;
        s -= maxPos * 20;
        s -= hasMuted * 15;
        s -= numberOfMuted * 15;
        s -= hasTooManyMuted * 100;
        s -= noRoot * 31;
        s -= numberOfFingers * 4;
        s -= (lastString - firstString) * 5;

        return s;
    }

    public static int PositionComparer(Position a, Position b) {
        if (a.score < b.score) return -1;
        else if (a.score > b.score) return 1;
        else return 0;
    }
}
