namespace Fluffers_CE_Decompiler
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Forum_Theme = new __________3vil_Theme();
            this.Agree_Checkbox = new WC_Linux.WC_Linux_CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Decompile_Button = new WC_Linux.WC_Linux_Button_2();
            this.wC_Linux_Separator1 = new WC_Linux.WC_Linux_Separator();
            this.FileLocation_Label = new WC_Linux.WC_Linux_Label();
            this.wC_Linux_ControlBox1 = new WC_Linux.WC_Linux_ControlBox();
            this.Forum_Theme.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // Forum_Theme
            // 
            this.Forum_Theme.AccentOffset = 0;
            this.Forum_Theme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Forum_Theme.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Forum_Theme.Colors = new Bloom[0];
            this.Forum_Theme.Controls.Add(this.Agree_Checkbox);
            this.Forum_Theme.Controls.Add(this.richTextBox1);
            this.Forum_Theme.Controls.Add(this.Decompile_Button);
            this.Forum_Theme.Controls.Add(this.wC_Linux_Separator1);
            this.Forum_Theme.Controls.Add(this.FileLocation_Label);
            this.Forum_Theme.Controls.Add(this.wC_Linux_ControlBox1);
            this.Forum_Theme.Customization = "";
            this.Forum_Theme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Forum_Theme.Font = new System.Drawing.Font("Verdana", 8F);
            this.Forum_Theme.Image = null;
            this.Forum_Theme.Location = new System.Drawing.Point(0, 0);
            this.Forum_Theme.Movable = true;
            this.Forum_Theme.Name = "Forum_Theme";
            this.Forum_Theme.NoRounding = false;
            this.Forum_Theme.Sizable = true;
            this.Forum_Theme.Size = new System.Drawing.Size(513, 261);
            this.Forum_Theme.SmartBounds = true;
            this.Forum_Theme.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Forum_Theme.TabIndex = 0;
            this.Forum_Theme.Text = "Fluffer\'s CE Decompiler";
            this.Forum_Theme.TransparencyKey = System.Drawing.Color.Empty;
            this.Forum_Theme.Transparent = false;
            // 
            // Agree_Checkbox
            // 
            this.Agree_Checkbox.BackColor = System.Drawing.Color.Transparent;
            this.Agree_Checkbox.Checked = false;
            this.Agree_Checkbox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Agree_Checkbox.ForeColor = System.Drawing.Color.Transparent;
            this.Agree_Checkbox.Location = new System.Drawing.Point(416, 140);
            this.Agree_Checkbox.Name = "Agree_Checkbox";
            this.Agree_Checkbox.Size = new System.Drawing.Size(87, 15);
            this.Agree_Checkbox.TabIndex = 11;
            this.Agree_Checkbox.Text = "I AGREE";
            this.Agree_Checkbox.CheckedChanged += new WC_Linux.WC_Linux_CheckBox.CheckedChangedEventHandler(this.Agree_Checkbox_CheckedChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ForeColor = System.Drawing.Color.Gray;
            this.richTextBox1.Location = new System.Drawing.Point(12, 39);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(489, 91);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // Decompile_Button
            // 
            this.Decompile_Button.BackColor = System.Drawing.Color.Transparent;
            this.Decompile_Button.Enabled = false;
            this.Decompile_Button.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.Decompile_Button.Image = null;
            this.Decompile_Button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Decompile_Button.Location = new System.Drawing.Point(10, 220);
            this.Decompile_Button.Name = "Decompile_Button";
            this.Decompile_Button.Size = new System.Drawing.Size(493, 28);
            this.Decompile_Button.TabIndex = 9;
            this.Decompile_Button.Text = "Select File and Decompile";
            this.Decompile_Button.TextAlignment = System.Drawing.StringAlignment.Center;
            this.Decompile_Button.Click += new System.EventHandler(this.Decompile_Button_Click);
            // 
            // wC_Linux_Separator1
            // 
            this.wC_Linux_Separator1.Location = new System.Drawing.Point(10, 158);
            this.wC_Linux_Separator1.Name = "wC_Linux_Separator1";
            this.wC_Linux_Separator1.Size = new System.Drawing.Size(493, 10);
            this.wC_Linux_Separator1.TabIndex = 4;
            this.wC_Linux_Separator1.Text = "wC_Linux_Separator1";
            // 
            // FileLocation_Label
            // 
            this.FileLocation_Label.AutoSize = true;
            this.FileLocation_Label.BackColor = System.Drawing.Color.Transparent;
            this.FileLocation_Label.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.FileLocation_Label.ForeColor = System.Drawing.Color.Gray;
            this.FileLocation_Label.Location = new System.Drawing.Point(12, 180);
            this.FileLocation_Label.Name = "FileLocation_Label";
            this.FileLocation_Label.Size = new System.Drawing.Size(96, 20);
            this.FileLocation_Label.TabIndex = 3;
            this.FileLocation_Label.Text = "File Location:";
            // 
            // wC_Linux_ControlBox1
            // 
            this.wC_Linux_ControlBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.wC_Linux_ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.wC_Linux_ControlBox1.EnableMaximize = false;
            this.wC_Linux_ControlBox1.Font = new System.Drawing.Font("Marlett", 7F);
            this.wC_Linux_ControlBox1.Location = new System.Drawing.Point(466, 3);
            this.wC_Linux_ControlBox1.Name = "wC_Linux_ControlBox1";
            this.wC_Linux_ControlBox1.Size = new System.Drawing.Size(44, 22);
            this.wC_Linux_ControlBox1.TabIndex = 0;
            this.wC_Linux_ControlBox1.Text = "wC_Linux_ControlBox1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 261);
            this.Controls.Add(this.Forum_Theme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Forum_Theme.ResumeLayout(false);
            this.Forum_Theme.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private __________3vil_Theme Forum_Theme;
        private WC_Linux.WC_Linux_Separator wC_Linux_Separator1;
        private WC_Linux.WC_Linux_Label FileLocation_Label;
        private WC_Linux.WC_Linux_ControlBox wC_Linux_ControlBox1;
        private WC_Linux.WC_Linux_Button_2 Decompile_Button;
        private WC_Linux.WC_Linux_CheckBox Agree_Checkbox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

