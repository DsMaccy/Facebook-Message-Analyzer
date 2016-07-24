namespace Facebook_Message_Analyzer.Presentation
{
    partial class ModulePreferencesForm
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
            this.modules = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // modules
            // 
            this.modules.FormattingEnabled = true;
            this.modules.Location = new System.Drawing.Point(12, 12);
            this.modules.Name = "modules";
            this.modules.Size = new System.Drawing.Size(99, 238);
            this.modules.TabIndex = 0;
            // 
            // ModulePreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 261);
            this.Controls.Add(this.modules);
            this.Name = "ModulePreferencesForm";
            this.Text = "ModulePreferences";
            this.Resize += new System.EventHandler(this.ModulePreferencesForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox modules;
    }
}