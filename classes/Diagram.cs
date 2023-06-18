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
                for (int j = 0; j < stringPos.Length; j++) {
                    if (stringPos[j] == i) {
                        frets[i - firstFret, j] = finger;
                    }
                }

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
                case Position.mutedString:
                    sb.Append("x");
                    break;
                case Position.emptyString:
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

    public void Draw(Graphics g) {
        Font FretFont = new Font("Arial", 10);
        Font FingerFont = new Font("Arial", 8);
        Font StringFont = new Font("Consolas", 12);
        StringFormat stringFormat = new StringFormat();

        int numberOfStrings = stringPos.Length;
        int numberOfFrets = frets.GetLength(0);

        int fingerRadius = 7;
        int barreHeight = 8;
        int stringLabelsPadding = -1;
        
        int firstFretY = 20;
        int leftPadding = firstFret == 1 ? 0 : 20;
        int availableVerticalSpace = Program.DiagramHeight - firstFretY - 1;
        int availableHorizontalSpace = Program.DiagramWidth - leftPadding;

        int fretSpace = Math.Min(availableVerticalSpace / numberOfFrets, 20); // random constant
        int stringSpace = Math.Min(availableHorizontalSpace / numberOfStrings, 20); // random constant
        int horizontalPadding = (availableHorizontalSpace - (stringSpace * (numberOfStrings - 1))) / 2;

        // draw strings
        for (int i = 0; i < numberOfStrings; i++) {
            int x = leftPadding + horizontalPadding + i * stringSpace;
            g.DrawLine(Pens.Gray, x, firstFretY, x, Program.DiagramHeight);
        }

        // draw frets
        for (int i = 0; i <= numberOfFrets; i++) {
            int y = firstFretY + i * fretSpace;
            g.DrawLine(Pens.Gray, leftPadding + horizontalPadding, y, Program.DiagramWidth - horizontalPadding, y);
        }

        // draw second line (for fret 0) or fret number
        if (firstFret == 1) {
            int y = firstFretY + 1;
            g.DrawLine(Pens.Gray, leftPadding + horizontalPadding, y, Program.DiagramWidth - horizontalPadding, y);
        } else {
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Far;
            g.DrawString(firstFret.ToString(), FretFont, Brushes.Black, new Point(leftPadding, firstFretY + fretSpace / 2), stringFormat);
        }

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // draw barre
        if (barre != 0) {
            int y = (firstFretY + (barre - firstFret) * fretSpace) + (fretSpace - barreHeight) / 2;
            g.FillRectangle(Brushes.Black, leftPadding + horizontalPadding, y, (numberOfStrings - 1) * stringSpace, barreHeight);
        }

        stringFormat.LineAlignment = StringAlignment.Center;
        stringFormat.Alignment = StringAlignment.Center;

        // draw fingers
        for (int i = 0; i < numberOfFrets; i++) {
            for (int j = 0; j < numberOfStrings; j++) {
                if (frets[i, j] != 0) {
                    int fingerX = leftPadding + horizontalPadding + j * stringSpace;
                    int fingerY = firstFretY + i * fretSpace + fretSpace / 2;
                    g.FillEllipse(Brushes.Black, fingerX - fingerRadius, fingerY - fingerRadius, 2 * fingerRadius, 2 * fingerRadius);
                    g.DrawString(frets[i, j].ToString(), FingerFont, Brushes.White, new Point(fingerX, fingerY), stringFormat);
                }
            }
        }

        stringFormat.LineAlignment = StringAlignment.Far;
        stringFormat.Alignment = StringAlignment.Center;

        // draw labels for muted and empty strings
        for (int i = 0; i < numberOfStrings; i++) {
            if (stringPos[i] == Position.emptyString || stringPos[i] == Position.mutedString) {
                string symbol = stringPos[i] == Position.emptyString ? "o" : "x";
                int x = leftPadding + horizontalPadding + i * stringSpace;
                g.DrawString(symbol, StringFont, Brushes.Black, new Point(x, firstFretY - stringLabelsPadding), stringFormat);
            }
        }
    }
}
