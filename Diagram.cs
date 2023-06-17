class Diagram {
    int _barre = 0;
    public int barre => _barre;
    int _firstFret = 1;
    public int firstFret => _firstFret;
    public int[] stringPos;
    public int[,] frets;

    public Diagram(int[] stringPos, int barre, int minPos, int maxPos) {
        this.stringPos = stringPos;
        this._barre = barre;
        int lastFret = 0;
        int finger = 1;
        int totalFingers = (barre != 0) ? 1 : 0;
        int fingerShift = 0;

        if (maxPos <= 5) {
            lastFret = Math.Max(4, maxPos);
        } else {
            _firstFret = minPos;
            lastFret = Math.Max(minPos + 3, maxPos);
        }

        frets = new int[lastFret - firstFret + 1, stringPos.Length];

        for (int i = 0; i < stringPos.Length; i++) {
            if (stringPos[i] != barre && stringPos[i] != Position.mutedString) {
                totalFingers++;
            }
        }

        for (int i = firstFret; i <= lastFret; i++) {
            if (barre == i) {
                finger++;
            } else {
                int lastFinger = finger;

                for (int j = 0; j < stringPos.Length; j++) {
                    if (stringPos[j] == i) {
                        frets[i - firstFret, j] = fingerShift + finger;
                        finger++;
                    }
                }

                if (i > barre && lastFinger == finger && totalFingers + fingerShift < Music.AvailableFingers - 1) {
                    fingerShift++;
                }
            }
        }
    }

    public string GetString() {
        var sb = new System.Text.StringBuilder();
        sb.Append(" ");

        for (int i = 0; i < stringPos.Length; i++) {
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

        for (int i = 0; i < frets.GetLength(0); i++) {
            if (i != 0) {
                sb.Append("  ");
            } else if (firstFret < 10) {
                sb.Append(" ");
            }

            if (barre == i + firstFret) {
                sb.Append(new String('e', stringPos.Length * 2 - 1));
            } else {
                for (int j = 0; j < frets.GetLength(1); j++) {
                    if (j != 0) {
                        sb.Append(" ");
                    }

                    if (frets[i, j] != 0) {
                        // sb.Append("0");
                        sb.Append(frets[i, j]);
                    } else {
                        sb.Append("|");
                    }
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
