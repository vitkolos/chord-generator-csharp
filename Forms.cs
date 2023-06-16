using System.Windows.Forms;

class Forms {
    [STAThread]
    static void Main(string[] args) {
        if (Program.RunInWindow) {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        } else {
            Program.ConsoleMain(args);
        }
    }
}

public partial class Form1 : Form {
    Instrument instrument;
    Parser parser;
    private Label chordLabel;
    private Label configLabel;
    private TextBox chordInput;
    private FlowLayoutPanel outputArea;
    private Button okButton;
    private FlowLayoutPanel instrumentRadioGroup;
    int[]? sharedVerticalSpace;

    public Form1() {
        chordLabel = new Label();
        configLabel = new Label();
        chordInput = new TextBox();
        outputArea = new FlowLayoutPanel();
        okButton = new Button();
        instrumentRadioGroup = new FlowLayoutPanel();
        parser = new Parser(Program.ConfigPath);
        instrument = parser.ParseInstrument()!;
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

        int h = 23; // item height
        int vp = 5; // item vertical padding
        int hp = 5;
        int leftPadding = 10;
        int boxWidth = 250;
        int buttonWidth = 100;

        int[] verticalSpace = { vp, h, vp, h, vp, h, vp * 2 };
        sharedVerticalSpace = verticalSpace;

        chordLabel.Text = Strings.ChordLabel;
        chordLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        chordLabel.Location = new Point(leftPadding, Program.SumTo(verticalSpace, 1));
        chordLabel.Size = new Size(boxWidth, verticalSpace[1]);
        chordLabel.TextAlign = ContentAlignment.BottomLeft;
        chordLabel.Click += chordLabel_Click;

        configLabel.Text = Strings.ConfigLabel;
        configLabel.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
        configLabel.Location = new Point(leftPadding + boxWidth + hp, Program.SumTo(verticalSpace, 1));
        configLabel.Size = new Size(buttonWidth, verticalSpace[1]);
        configLabel.TextAlign = ContentAlignment.BottomCenter;
        configLabel.ForeColor = Color.Gray;
        configLabel.Cursor = Cursors.Hand;
        configLabel.Click += configLabel_Click;

        ToolTip configToolTip = new ToolTip();
        configToolTip.AutoPopDelay = 5000;
        configToolTip.InitialDelay = 500;
        configToolTip.ReshowDelay = 500;
        configToolTip.ShowAlways = true;
        configToolTip.SetToolTip(configLabel, Strings.ConfigLabelTooltip);

        chordInput.Location = new Point(leftPadding, Program.SumTo(verticalSpace, 3));
        chordInput.Size = new Size(boxWidth, verticalSpace[3]);
        chordInput.TabIndex = 0;
        chordInput.KeyDown += chordInput_KeyDown;

        okButton.Location = new Point(leftPadding + boxWidth + hp, Program.SumTo(verticalSpace, 3));
        okButton.Size = new Size(buttonWidth, verticalSpace[5]);
        okButton.TabIndex = 1;
        okButton.Text = Strings.OkButton;
        okButton.Cursor = Cursors.Hand;
        okButton.Click += okButton_Click;

        instrumentRadioGroup.Location = new Point(leftPadding, Program.SumTo(verticalSpace, 5));
        instrumentRadioGroup.TabIndex = 2;
        instrumentRadioGroup.Size = new Size(0, 0);
        instrumentRadioGroup.AutoSize = true;
        List<string> instrumentNames = parser.instruments.Keys.ToList();

        foreach (string instrumentName in instrumentNames) {
            var rb = new RadioButton();
            rb.Text = instrumentName;
            rb.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
            rb.Cursor = Cursors.Hand;
            rb.AutoSize = true;
            instrumentRadioGroup.Controls.Add(rb);

            if (parser.defaultInstrument == instrumentName) {
                rb.Checked = true;
            }
        }

        outputArea.AutoScroll = true;
        outputArea.Location = new Point(0, Program.SumTo(verticalSpace, 7));
        outputArea.Size = new Size(354, 153);
        outputArea.BackColor = Color.White;
        outputArea.Padding = new Padding(10);

        // Form1
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(leftPadding + boxWidth + hp + buttonWidth + leftPadding, 550);
        Controls.Add(chordLabel);
        Controls.Add(configLabel);
        Controls.Add(chordInput);
        Controls.Add(instrumentRadioGroup);
        Controls.Add(okButton);
        Controls.Add(outputArea);
        Margin = new Padding(3, 2, 3, 2);
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
        }
    }

    private void radioButton_CheckedChanged(object? sender, EventArgs e) {
        RadioButton? rb = sender as RadioButton;

        if (rb != null && rb.Checked) {
            instrument = parser.ParseInstrument(rb.Text)!;
            showDiagrams();
        }
    }

    private void configLabel_Click(object? sender, EventArgs e) {
        var startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.UseShellExecute = true; // prevents an error
        startInfo.FileName = Program.ConfigPath;
        System.Diagnostics.Process.Start(startInfo);
    }

    private void chordLabel_Click(object? sender, EventArgs e) {
        chordInput.Focus();
    }

    private void okButton_Click(object? sender, EventArgs e) {
        showDiagrams();
    }

    private void chordInput_KeyDown(object? sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Enter) {
            showDiagrams();
            e.SuppressKeyPress = true; // removes the sound
        }
    }

    void showDiagrams() {
        string input = chordInput.Text;

        if (input != "") {
            try {
                outputArea.Controls.Clear();
                var chord = new Chord(input, parser);
                var positions = new Positions(instrument, chord);

                positions.list.Sort(Position.PositionComparer);
                positions.list.Reverse();
                var i = 0;

                foreach (var position in positions.list) {
                    if (++i <= 10) {
                        var diagram = new Label();
                        diagram.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
                        diagram.Text = i.ToString() + ". " + position.GetDiagram();
                        diagram.AutoSize = true;
                        diagram.Padding = new Padding(0, 5, 0, 5);
                        outputArea.Controls.Add(diagram);
                    }
                }
            } catch (Exception e) {
                var message = new Label();
                message.Text = e.Message;
                message.AutoSize = true;
                outputArea.Controls.Add(message);
            }
        }
    }
}
