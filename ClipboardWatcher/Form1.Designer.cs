namespace ClipboardWatcher
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      StopMonitor();

      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lbxWords = new System.Windows.Forms.ListBox();
      this.btnStart = new System.Windows.Forms.Button();
      this.btnStop = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lbxWords
      // 
      this.lbxWords.FormattingEnabled = true;
      this.lbxWords.HorizontalScrollbar = true;
      this.lbxWords.Location = new System.Drawing.Point(12, 12);
      this.lbxWords.Name = "lbxWords";
      this.lbxWords.ScrollAlwaysVisible = true;
      this.lbxWords.Size = new System.Drawing.Size(452, 277);
      this.lbxWords.TabIndex = 0;
      // 
      // btnStart
      // 
      this.btnStart.Location = new System.Drawing.Point(12, 295);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(75, 23);
      this.btnStart.TabIndex = 1;
      this.btnStart.Text = "Start";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
      // 
      // btnStop
      // 
      this.btnStop.Enabled = false;
      this.btnStop.Location = new System.Drawing.Point(389, 295);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size(75, 23);
      this.btnStop.TabIndex = 2;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(12, 361);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Save to CP";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(476, 447);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.btnStop);
      this.Controls.Add(this.btnStart);
      this.Controls.Add(this.lbxWords);
      this.Name = "Form1";
      this.Text = "ClipboardWatcher";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox lbxWords;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnSave;
  }
}

