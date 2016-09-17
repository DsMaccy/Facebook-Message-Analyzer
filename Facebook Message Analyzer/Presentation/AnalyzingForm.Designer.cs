namespace Facebook_Message_Analyzer.Presentation
{
    partial class AnalyzingForm
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
            this.message = new System.Windows.Forms.Label();
            this.abort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // message
            // 
            this.message.AutoSize = true;
            this.message.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.message.Location = new System.Drawing.Point(138, 9);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(177, 20);
            this.message.TabIndex = 0;
            this.message.Text = "Analyzing... Please Wait";
            // 
            // abort
            // 
            this.abort.Location = new System.Drawing.Point(195, 122);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(75, 23);
            this.abort.TabIndex = 1;
            this.abort.Text = "Cancel";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.abort_Click);
            // 
            // AnalyzingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 157);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.message);
            this.Name = "AnalyzingForm";
            this.Text = "AnalyzeForm";
            this.Resize += new System.EventHandler(this.AnalyzingForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label message;
        private System.Windows.Forms.Button abort;
    }
}