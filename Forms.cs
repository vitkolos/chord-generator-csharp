class Forms {
    [STAThread]
    static void Main(string[] args) {
        if (Program.RunInWindow) {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        } else {
            ConsoleApp.Run(args);
        }
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();
}

public partial class Form1 : Form {
    Instrument instrument;
    Parser parser;
    ChordPlayer player;
    private Label chordLabel;
    private LinkLabel configLabel;
    private TextBox chordInput;
    private FlowLayoutPanel outputArea;
    private Button okButton;
    private FlowLayoutPanel instrumentRadioGroup;
    int[]? sharedVerticalSpace;
    int sharedLeftPadding;
    int diagramHorizontalPadding = 10;
    int diagramVerticalPadding = 8;

    public Form1() {
        chordLabel = new Label();
        configLabel = new LinkLabel();
        chordInput = new TextBox();
        outputArea = new FlowLayoutPanel();
        okButton = new Button();
        instrumentRadioGroup = new FlowLayoutPanel();
        parser = new Parser(Program.ConfigPath);
        player = new ChordPlayer();
        instrument = parser.GetDefaultInstrument();
        InitializeComponent();
    }

    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent() {
        SuspendLayout();

        int windowHeight = 500;

        int verticalPadding = 8;
        int itemHeight = 23;
        int outputAreaVerticalPadding = 15;

        int[] verticalSpace = { 10, itemHeight, verticalPadding, itemHeight, verticalPadding, itemHeight, outputAreaVerticalPadding };
        // top padding, label, padding, input, padding, radio, padding, (output area)

        int horizontalPadding = 10;
        int leftPadding = 20;
        int inputBoxWidth = 250;
        int buttonWidth = 100;

        sharedVerticalSpace = verticalSpace;
        sharedLeftPadding = leftPadding;

        chordLabel.Text = Strings.ChordLabel;
        chordLabel.Font = new Font("Segoe UI", 10F);
        chordLabel.Location = new Point(leftPadding, Program.SumTo(verticalSpace, 1));
        chordLabel.Size = new Size(inputBoxWidth, verticalSpace[1]);
        chordLabel.TextAlign = ContentAlignment.BottomLeft;
        chordLabel.Click += chordLabel_Click;

        configLabel.Text = Strings.ConfigLabel;
        configLabel.Font = new Font("Segoe UI", 8F);
        configLabel.Location = new Point(leftPadding + inputBoxWidth + horizontalPadding, Program.SumTo(verticalSpace, 1));
        configLabel.Size = new Size(buttonWidth, verticalSpace[1]);
        configLabel.TextAlign = ContentAlignment.BottomCenter;
        configLabel.TabStop = true;
        configLabel.TabIndex = 3;
        configLabel.LinkColor = Color.DimGray;
        configLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        configLabel.Cursor = Cursors.Hand;
        configLabel.Click += configLabel_Click;
        configLabel.LinkClicked += configLabel_Click;

        chordInput.Location = new Point(leftPadding, Program.SumTo(verticalSpace, 3));
        chordInput.Size = new Size(inputBoxWidth, verticalSpace[3]);
        chordInput.TabIndex = 0;
        chordInput.KeyDown += chordInput_KeyDown;

        okButton.Location = new Point(leftPadding + inputBoxWidth + horizontalPadding, Program.SumTo(verticalSpace, 3));
        okButton.Size = new Size(buttonWidth, verticalSpace[3]);
        okButton.TabIndex = 1;
        okButton.Text = Strings.OkButton;
        okButton.Cursor = Cursors.Hand;
        okButton.Click += okButton_Click;

        instrumentRadioGroup.Location = new Point(leftPadding, Program.SumTo(verticalSpace, 5));
        instrumentRadioGroup.Size = new Size(0, verticalSpace[5] + verticalSpace[6]); // including the padding ensures there is enough space for a horizontal scrollbar
        instrumentRadioGroup.TabIndex = 2;
        instrumentRadioGroup.WrapContents = false;
        instrumentRadioGroup.AutoScroll = true;
        List<string> instrumentNames = parser.GetInstrumentsNames();

        foreach (string instrumentName in instrumentNames) {
            var rb = new RadioButton();
            rb.Text = instrumentName;
            rb.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            rb.Cursor = Cursors.Hand;
            rb.AutoSize = true;
            instrumentRadioGroup.Controls.Add(rb);

            if (instrument.name == instrumentName) {
                rb.Checked = true;
            }
        }

        outputArea.AutoScroll = true;
        outputArea.Location = new Point(0, Program.SumTo(verticalSpace, 7));
        outputArea.BackColor = Color.White;
        outputArea.Padding = new Padding(leftPadding - diagramHorizontalPadding, outputAreaVerticalPadding - diagramVerticalPadding,
            leftPadding - diagramHorizontalPadding, leftPadding);

        // form configuration
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(leftPadding + inputBoxWidth + horizontalPadding + buttonWidth + leftPadding, windowHeight);
        Controls.Add(chordLabel);
        Controls.Add(configLabel);
        Controls.Add(chordInput);
        Controls.Add(instrumentRadioGroup);
        Controls.Add(okButton);
        Controls.Add(outputArea);
        Text = Strings.Title;
        Load += Form1_Load;
        Resize += Form1_Resize;

        ResumeLayout(false);
    }

    private void Form1_Load(object? sender, EventArgs e) {
        RefreshResponsiveLayout();
    }

    private void Form1_Resize(object? sender, EventArgs e) {
        RefreshResponsiveLayout();
    }

    void RefreshResponsiveLayout() {
        if (sharedVerticalSpace != null) {
            outputArea.Width = Convert.ToInt32(this.ClientSize.Width);
            outputArea.Height = Convert.ToInt32(this.ClientSize.Height - Program.SumTo(sharedVerticalSpace, 7));
            instrumentRadioGroup.Width = Convert.ToInt32(this.ClientSize.Width - sharedLeftPadding * 2);
        }
    }

    private void radioButton_CheckedChanged(object? sender, EventArgs e) {
        RadioButton? rb = sender as RadioButton;

        if (rb != null && rb.Checked) {
            instrument = parser.ParseInstrument(rb.Text)!;
            ShowDiagrams();
        }
    }

    private void configLabel_Click(object? sender, EventArgs e) {
        var startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.UseShellExecute = true; // prevents an error
        startInfo.FileName = Program.ConfigPath;
        System.Diagnostics.Process.Start(startInfo);
        Label m = WriteMessage(Strings.ConfigMessage);
        m.Cursor = Cursors.Hand;
        m.Click += configMessage_Click;
    }

    private void configMessage_Click(object? sender, EventArgs e) {
        Application.Restart();
        Environment.Exit(0);
    }

    private void chordLabel_Click(object? sender, EventArgs e) {
        chordInput.Focus();
    }

    private void okButton_Click(object? sender, EventArgs e) {
        ShowDiagrams();
    }

    private void diagram_Click(object? sender, EventArgs e) {
        Position position = ((sender as Control)!.Tag as Position)!;
        position.Play(player, instrument);
    }

    private void chordInput_KeyDown(object? sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Enter) {
            ShowDiagrams();
            e.SuppressKeyPress = true; // removes the sound
        }
    }

    void ShowDiagrams() {
        string input = chordInput.Text;

        if (input != "") {
            try {
                var chord = new Chord(input, parser);
                var positions = new Positions(instrument, chord);

                positions.list.Sort(Position.PositionComparer);
                positions.list.Reverse();
                var i = 0;
                outputArea.Controls.Clear();

                foreach (var position in positions.list) {
                    if (++i <= Program.NumberOfDiagrams) {
                        if (Program.GraphicMode) {
                            PictureBox imageDiagram = new PictureBox();
                            imageDiagram.Tag = position;
                            imageDiagram.Size = new Size(Program.DiagramWidth, Program.DiagramHeight);
                            imageDiagram.Cursor = Cursors.Hand;
                            imageDiagram.Margin = new Padding(diagramHorizontalPadding, diagramVerticalPadding, diagramHorizontalPadding, diagramVerticalPadding);
                            imageDiagram.Paint += new PaintEventHandler(this.imageDiagram_Paint);
                            imageDiagram.Click += diagram_Click;
                            outputArea.Controls.Add(imageDiagram);
                        } else {
                            var diagram = new Label();
                            diagram.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
                            diagram.Text = i.ToString() + ". \n" + position.GetDiagram();
                            diagram.AutoSize = true;
                            diagram.Margin = new Padding(diagramHorizontalPadding, diagramVerticalPadding, 0, diagramVerticalPadding);
                            diagram.Tag = position;
                            diagram.Cursor = Cursors.Hand;
                            diagram.Click += diagram_Click;
                            outputArea.Controls.Add(diagram);
                        }
                    }
                }
            } catch (Exception e) {
                Label m = WriteMessage(e.Message);
                m.ForeColor = Color.DarkRed;
            }
        }
    }

    Label WriteMessage(string messageText) {
        var message = new Label();
        message.Text = messageText;
        message.AutoSize = true;
        message.Margin = new Padding(diagramHorizontalPadding, diagramVerticalPadding, diagramHorizontalPadding, diagramVerticalPadding);
        outputArea.Controls.Clear();
        outputArea.Controls.Add(message);
        return message;
    }

    private void imageDiagram_Paint(object? sender, PaintEventArgs e) {
        Graphics g = e.Graphics;
        Position position = ((sender as PictureBox)!.Tag as Position)!;
        position.DrawDiagram(g);
    }
}
