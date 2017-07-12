namespace CustomInstaller
{
    partial class DownloadOptionsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DataLocBrowseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DataLocationTextBox = new System.Windows.Forms.TextBox();
            this.ProgLocBrowseButton = new System.Windows.Forms.Button();
            this.SaveLocation = new System.Windows.Forms.Label();
            this.ProgramLocationTextBox = new System.Windows.Forms.TextBox();
            this.StartMenuCheckBox = new System.Windows.Forms.CheckBox();
            this.DesktopShortcutCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DataLocBrowseButton
            // 
            this.DataLocBrowseButton.Location = new System.Drawing.Point(201, 101);
            this.DataLocBrowseButton.Name = "DataLocBrowseButton";
            this.DataLocBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.DataLocBrowseButton.TabIndex = 16;
            this.DataLocBrowseButton.Text = "Browse";
            this.DataLocBrowseButton.UseVisualStyleBackColor = true;
            this.DataLocBrowseButton.Click += new System.EventHandler(this.DataLocBrowseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Data Location";
            // 
            // DataLocationTextBox
            // 
            this.DataLocationTextBox.Location = new System.Drawing.Point(3, 103);
            this.DataLocationTextBox.Name = "DataLocationTextBox";
            this.DataLocationTextBox.Size = new System.Drawing.Size(192, 20);
            this.DataLocationTextBox.TabIndex = 14;
            // 
            // ProgLocBrowseButton
            // 
            this.ProgLocBrowseButton.Location = new System.Drawing.Point(201, 59);
            this.ProgLocBrowseButton.Name = "ProgLocBrowseButton";
            this.ProgLocBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.ProgLocBrowseButton.TabIndex = 13;
            this.ProgLocBrowseButton.Text = "Browse";
            this.ProgLocBrowseButton.UseVisualStyleBackColor = true;
            this.ProgLocBrowseButton.Click += new System.EventHandler(this.ProgLocBrowseButton_Click);
            // 
            // SaveLocation
            // 
            this.SaveLocation.AutoSize = true;
            this.SaveLocation.Location = new System.Drawing.Point(7, 45);
            this.SaveLocation.Name = "SaveLocation";
            this.SaveLocation.Size = new System.Drawing.Size(90, 13);
            this.SaveLocation.TabIndex = 12;
            this.SaveLocation.Text = "Program Location";
            // 
            // ProgramLocationTextBox
            // 
            this.ProgramLocationTextBox.Location = new System.Drawing.Point(3, 61);
            this.ProgramLocationTextBox.Name = "ProgramLocationTextBox";
            this.ProgramLocationTextBox.Size = new System.Drawing.Size(192, 20);
            this.ProgramLocationTextBox.TabIndex = 11;
            // 
            // StartMenuCheckBox
            // 
            this.StartMenuCheckBox.AutoSize = true;
            this.StartMenuCheckBox.Location = new System.Drawing.Point(3, 25);
            this.StartMenuCheckBox.Name = "StartMenuCheckBox";
            this.StartMenuCheckBox.Size = new System.Drawing.Size(155, 17);
            this.StartMenuCheckBox.TabIndex = 10;
            this.StartMenuCheckBox.Text = "Create Start Menu Shortcut";
            this.StartMenuCheckBox.UseVisualStyleBackColor = true;
            // 
            // DesktopShortcutCheckBox
            // 
            this.DesktopShortcutCheckBox.AutoSize = true;
            this.DesktopShortcutCheckBox.Location = new System.Drawing.Point(3, 1);
            this.DesktopShortcutCheckBox.Name = "DesktopShortcutCheckBox";
            this.DesktopShortcutCheckBox.Size = new System.Drawing.Size(143, 17);
            this.DesktopShortcutCheckBox.TabIndex = 9;
            this.DesktopShortcutCheckBox.Text = "Create Desktop Shortcut";
            this.DesktopShortcutCheckBox.UseVisualStyleBackColor = true;
            // 
            // DownloadOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataLocBrowseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataLocationTextBox);
            this.Controls.Add(this.ProgLocBrowseButton);
            this.Controls.Add(this.SaveLocation);
            this.Controls.Add(this.ProgramLocationTextBox);
            this.Controls.Add(this.StartMenuCheckBox);
            this.Controls.Add(this.DesktopShortcutCheckBox);
            this.Name = "DownloadOptionsControl";
            this.Size = new System.Drawing.Size(304, 172);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DataLocBrowseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DataLocationTextBox;
        private System.Windows.Forms.Button ProgLocBrowseButton;
        private System.Windows.Forms.Label SaveLocation;
        private System.Windows.Forms.TextBox ProgramLocationTextBox;
        private System.Windows.Forms.CheckBox StartMenuCheckBox;
        private System.Windows.Forms.CheckBox DesktopShortcutCheckBox;
    }
}
