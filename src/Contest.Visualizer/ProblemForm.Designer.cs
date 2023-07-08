namespace Contest.Visualizer
{
    partial class ProblemForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            problemVisualizer1 = new ProblemVisualizer();
            splitContainer2 = new SplitContainer();
            tbMessages = new TextBox();
            statusStrip1 = new StatusStrip();
            lblMusicianCount = new ToolStripStatusLabel();
            lblIterations = new ToolStripStatusLabel();
            lblAttendeesCount = new Label();
            lblInstrumentsCount = new Label();
            lblMusiciansCount = new Label();
            btnSubmit = new Button();
            button1 = new Button();
            label2 = new Label();
            tbGridSize = new TextBox();
            label1 = new Label();
            comboBox1 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(problemVisualizer1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(966, 800);
            splitContainer1.SplitterDistance = 599;
            splitContainer1.TabIndex = 0;
            // 
            // problemVisualizer1
            // 
            problemVisualizer1.Dock = DockStyle.Fill;
            problemVisualizer1.Location = new Point(0, 0);
            problemVisualizer1.Name = "problemVisualizer1";
            problemVisualizer1.Problem = null;
            problemVisualizer1.Size = new Size(966, 599);
            problemVisualizer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tbMessages);
            splitContainer2.Panel1.Controls.Add(statusStrip1);
            splitContainer2.Panel1.Controls.Add(lblAttendeesCount);
            splitContainer2.Panel1.Controls.Add(lblInstrumentsCount);
            splitContainer2.Panel1.Controls.Add(lblMusiciansCount);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(btnSubmit);
            splitContainer2.Panel2.Controls.Add(button1);
            splitContainer2.Panel2.Controls.Add(label2);
            splitContainer2.Panel2.Controls.Add(tbGridSize);
            splitContainer2.Panel2.Controls.Add(label1);
            splitContainer2.Panel2.Controls.Add(comboBox1);
            splitContainer2.Size = new Size(966, 197);
            splitContainer2.SplitterDistance = 556;
            splitContainer2.TabIndex = 0;
            // 
            // tbMessages
            // 
            tbMessages.Dock = DockStyle.Bottom;
            tbMessages.Location = new Point(0, 79);
            tbMessages.MaxLength = 327670;
            tbMessages.Multiline = true;
            tbMessages.Name = "tbMessages";
            tbMessages.ScrollBars = ScrollBars.Vertical;
            tbMessages.Size = new Size(556, 96);
            tbMessages.TabIndex = 4;
            tbMessages.Text = "Hello World";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblMusicianCount, lblIterations });
            statusStrip1.Location = new Point(0, 175);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(556, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblMusicianCount
            // 
            lblMusicianCount.Name = "lblMusicianCount";
            lblMusicianCount.Size = new Size(118, 17);
            lblMusicianCount.Text = "toolStripStatusLabel1";
            // 
            // lblIterations
            // 
            lblIterations.Name = "lblIterations";
            lblIterations.Size = new Size(118, 17);
            lblIterations.Text = "toolStripStatusLabel1";
            // 
            // lblAttendeesCount
            // 
            lblAttendeesCount.AutoSize = true;
            lblAttendeesCount.Location = new Point(21, 61);
            lblAttendeesCount.Name = "lblAttendeesCount";
            lblAttendeesCount.Size = new Size(38, 15);
            lblAttendeesCount.TabIndex = 2;
            lblAttendeesCount.Text = "label3";
            // 
            // lblInstrumentsCount
            // 
            lblInstrumentsCount.AutoSize = true;
            lblInstrumentsCount.Location = new Point(21, 37);
            lblInstrumentsCount.Name = "lblInstrumentsCount";
            lblInstrumentsCount.Size = new Size(38, 15);
            lblInstrumentsCount.TabIndex = 1;
            lblInstrumentsCount.Text = "label3";
            // 
            // lblMusiciansCount
            // 
            lblMusiciansCount.AutoSize = true;
            lblMusiciansCount.Location = new Point(21, 15);
            lblMusiciansCount.Name = "lblMusiciansCount";
            lblMusiciansCount.Size = new Size(38, 15);
            lblMusiciansCount.TabIndex = 0;
            lblMusiciansCount.Text = "label3";
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(130, 121);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(75, 23);
            btnSubmit.TabIndex = 5;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // button1
            // 
            button1.Location = new Point(34, 121);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Solve";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 32);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 3;
            label2.Text = "Grid Size";
            // 
            // tbGridSize
            // 
            tbGridSize.Location = new Point(71, 29);
            tbGridSize.Name = "tbGridSize";
            tbGridSize.Size = new Size(53, 23);
            tbGridSize.TabIndex = 2;
            tbGridSize.Text = "64";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 5);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 1;
            label1.Text = "Problem";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(71, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(53, 23);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // ProblemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(966, 800);
            Controls.Add(splitContainer1);
            Name = "ProblemForm";
            Text = "Form1";
            Shown += ProblemForm_Shown;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ProblemVisualizer problemVisualizer1;
        private SplitContainer splitContainer2;
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private TextBox tbGridSize;
        private Button button1;
        private Label lblAttendeesCount;
        private Label lblInstrumentsCount;
        private Label lblMusiciansCount;
        private Button btnSubmit;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblMusicianCount;
        private ToolStripStatusLabel lblIterations;
        private TextBox tbMessages;
    }
}