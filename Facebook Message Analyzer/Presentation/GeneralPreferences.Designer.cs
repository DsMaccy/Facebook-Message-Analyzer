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
            this.moduleLocationLabel = new System.Windows.Forms.Label();
            this.modulePath = new System.Windows.Forms.TextBox();
            this.browse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // moduleLocationLabel
            // 
            this.moduleLocationLabel.AutoSize = true;
            this.moduleLocationLabel.Location = new System.Drawing.Point(4, 4);
            this.moduleLocationLabel.Name = "moduleLocationLabel";
            this.moduleLocationLabel.Size = new System.Drawing.Size(97, 13);
            this.moduleLocationLabel.TabIndex = 0;
            this.moduleLocationLabel.Text = "Module Locations: ";
            // 
            // modulePath
            // 
            this.modulePath.Location = new System.Drawing.Point(107, 0);
            this.modulePath.Name = "modulePath";
            this.modulePath.Size = new System.Drawing.Size(179, 20);
            this.modulePath.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(211, 26);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 24);
            this.browse.TabIndex = 2;
            this.browse.Text = "browse";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // GeneralPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browse);
            this.Controls.Add(this.modulePath);
            this.Controls.Add(this.moduleLocationLabel);
            this.Name = "GeneralPreferences";
            this.Size = new System.Drawing.Size(289, 205);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label moduleLocationLabel;
        private System.Windows.Forms.TextBox modulePath;
        private System.Windows.Forms.Button browse;
    }
}
