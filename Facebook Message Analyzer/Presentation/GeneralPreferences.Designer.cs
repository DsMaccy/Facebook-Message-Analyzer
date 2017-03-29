namespace Facebook_Message_Analyzer.Presentation
{
    partial class GeneralPreferences
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
            this.cacheMessages = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cacheMessages
            // 
            this.cacheMessages.AutoSize = true;
            this.cacheMessages.Checked = true;
            this.cacheMessages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cacheMessages.Location = new System.Drawing.Point(3, 3);
            this.cacheMessages.Name = "cacheMessages";
            this.cacheMessages.Size = new System.Drawing.Size(264, 17);
            this.cacheMessages.TabIndex = 3;
            this.cacheMessages.Text = "Cache Message Data (saves FB messages on PC)";
            this.cacheMessages.UseVisualStyleBackColor = true;
            // 
            // GeneralPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cacheMessages);
            this.Name = "GeneralPreferences";
            this.Size = new System.Drawing.Size(289, 205);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cacheMessages;
    }
}
