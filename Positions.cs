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

        bool mutedCond = (s == 0) || (stringPos[s - 1] == Position.mutedString) || (remainingNotes == 0); // this should have no influence on final number of diagrams
        bool nonMutedCond = (s == 0) || (stringPos[s - 1] != Position.mutedString) || (Program.SumTo(stringPos, s) == -s);

        if (mutedCond) {
            stringPos[s] = Position.mutedString;
            notes[s] = Position.mutedString;
            FillNextString(barre, stringPos, notes, s + 1, remainingNotes, remainingFingers);
        }

        if (nonMutedCond) {
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
}

class Position {
    public const int emptyString = 0;
    public const int mutedString = -1;
    int barre = 0;
    int score = 0;
    int minPos = 0;
    int maxPos = 0;
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

    public string GetText() {
        return barre.ToString() + " " + string.Join(',', stringPos);
    }

    public string GetDiagram() {
        var sb = new System.Text.StringBuilder();

        int firstFret = 1;
        int lastFret = 0;
        int finger = 1;
        int totalFingers = (barre != 0) ? 1 : 0;
        int fingerShift = 0;

        if (maxPos <= 5) {
            lastFret = Math.Max(4, maxPos);
        } else {
            firstFret = minPos;
            lastFret = Math.Max(minPos + 3, maxPos);
        }

        sb.Append(" ");

        for (int i = 0; i < stringPos.Length; i++) {
            if (stringPos[i] != barre && stringPos[i] != mutedString) {
                totalFingers++;
            }

            sb.Append(" ");

            switch (stringPos[i]) {
                case -1:
                    sb.Append("x");
                    break;
                case 0:
                    sb.Append("o");
                    break;
                default:
                    sb.Append(" ");
                    break;
            }
        }

        sb.AppendLine();
        sb.Append(firstFret == 1 ? " " : firstFret);

        for (int i = firstFret; i <= lastFret; i++) {
            if (i != firstFret) {
                sb.Append("  ");
            } else if (firstFret < 10) {
                sb.Append(" ");
            }

            if (barre == i) {
                sb.Append(new String('e', stringPos.Length * 2 - 1));
                finger++;
            } else {
                int lastFinger = finger;

                for (int j = 0; j < stringPos.Length; j++) {
                    if (j != 0) {
                        sb.Append(" ");
                    }

                    if (stringPos[j] == i) {
                        // sb.Append("0");
                        sb.Append(fingerShift + finger);
                        finger++;
                    } else {
                        sb.Append("|");
                    }
                }

                if (i > barre && lastFinger == finger && totalFingers + fingerShift < Music.AvailableFingers - 1) {
                    fingerShift++;
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    int getScore() {
        maxPos = -1;
        minPos = instrument.frets;
        int minTone = (barre != 0) ? barre : instrument.frets;
        int lowestTone = Music.HighestNote;
        int numberOfMuted = 0;
        int hasBarre = (barre != 0) ? 1 : 0;
        int barreNotes = 0;
        int numberOfFingers = hasBarre;
        int firstString = instrument.strings.Length;
        int lastString = -1;

        for (int i = 0; i < stringPos.Length; i++) {
            int pos = stringPos[i];

            if (pos == mutedString) {
                numberOfMuted++;
            } else if (pos < minTone) {
                minTone = pos;
            }

            if (pos != emptyString && pos != mutedString) {
                if (pos != barre) {
                    numberOfFingers++;
                    lastString = i;

                    if (firstString > i) {
                        firstString = i;
                    }
                } else {
                    barreNotes++;
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
        int mutedOnBothSides = (stringPos[0] == mutedString && stringPos[stringPos.Length - 1] == mutedString) ? 1 : 0;
        int tooLittleBarreNotes = (barreNotes < 2) ? 1 : 0;

        int rootWeight = (instrument.strings.Length > 4) ? 3 : 1;
        int mutedWeight = (instrument.strings.Length > 4) ? 1 : 3;

        int s = 1000;

        s -= hasBarre * 10;
        s -= barre * 5;
        s -= hasBarre * tooLittleBarreNotes * 100;
        s -= noRoot * rootWeight * 25;

        s -= minTone * 15;
        s -= (maxPos - minTone) * 20;
        s -= (lastString - firstString) * (maxPos - minPos) * 10;
        s -= numberOfFingers * 10;

        s -= hasMuted * mutedWeight * 20;
        s -= numberOfMuted * mutedWeight * 30;
        s -= (hasTooManyMuted + mutedOnBothSides) * 100;

        return s;
    }

    public static int PositionComparer(Position a, Position b) {
        if (a.score < b.score) return -1;
        else if (a.score > b.score) return 1;
        else return 0;
    }
}
