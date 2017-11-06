namespace MultiFindWindowsForms {
  partial class Form1 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.FindButton = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.KeywordTextBox = new System.Windows.Forms.TextBox();
      this.listView1 = new System.Windows.Forms.ListView();
      this.PathList = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // FindButton
      // 
      this.FindButton.Location = new System.Drawing.Point(209, 12);
      this.FindButton.Name = "FindButton";
      this.FindButton.Size = new System.Drawing.Size(75, 23);
      this.FindButton.TabIndex = 0;
      this.FindButton.Text = "Find";
      this.FindButton.UseVisualStyleBackColor = true;
      this.FindButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(290, 13);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "Stop";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // KeywordTextBox
      // 
      this.KeywordTextBox.Location = new System.Drawing.Point(13, 13);
      this.KeywordTextBox.Name = "KeywordTextBox";
      this.KeywordTextBox.Size = new System.Drawing.Size(190, 20);
      this.KeywordTextBox.TabIndex = 2;
      // 
      // listView1
      // 
      this.listView1.Location = new System.Drawing.Point(209, 42);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(529, 480);
      this.listView1.TabIndex = 3;
      this.listView1.UseCompatibleStateImageBehavior = false;
      // 
      // PathList
      // 
      this.PathList.Cursor = System.Windows.Forms.Cursors.Default;
      this.PathList.Location = new System.Drawing.Point(13, 42);
      this.PathList.Name = "PathList";
      this.PathList.Size = new System.Drawing.Size(190, 480);
      this.PathList.TabIndex = 4;
      this.PathList.UseCompatibleStateImageBehavior = false;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(750, 534);
      this.Controls.Add(this.PathList);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.KeywordTextBox);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.FindButton);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button FindButton;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TextBox KeywordTextBox;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ListView PathList;
  }
}

