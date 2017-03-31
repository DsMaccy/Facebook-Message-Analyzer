namespace ProfanityCounter
{
    partial class ProfanityOptions
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
            this.censorCheckBox = new System.Windows.Forms.CheckBox();
            this.wordManager = new System.Windows.Forms.GroupBox();
            this.addButton = new System.Windows.Forms.Button();
            this.profanityListBox = new System.Windows.Forms.ListBox();
            this.profanityAdderTextBox = new System.Windows.Forms.TextBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.wordManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // censorCheckBox
            // 
            this.censorCheckBox.AutoSize = true;
            this.censorCheckBox.Location = new System.Drawing.Point(3, 3);
            this.censorCheckBox.Name = "censorCheckBox";
            this.censorCheckBox.Size = new System.Drawing.Size(128, 17);
            this.censorCheckBox.TabIndex = 0;
            this.censorCheckBox.Text = "Innocence Protection";
            this.censorCheckBox.UseVisualStyleBackColor = true;
            // 
            // wordManager
            // 
            this.wordManager.Controls.Add(this.removeButton);
            this.wordManager.Controls.Add(this.profanityAdderTextBox);
            this.wordManager.Controls.Add(this.profanityListBox);
            this.wordManager.Controls.Add(this.addButton);
            this.wordManager.Location = new System.Drawing.Point(4, 27);
            this.wordManager.Name = "wordManager";
            this.wordManager.Size = new System.Drawing.Size(160, 179);
            this.wordManager.TabIndex = 1;
            this.wordManager.TabStop = false;
            this.wordManager.Text = "Word Manager";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(111, 20);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(39, 23);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // profanityListBox
            // 
            this.profanityListBox.FormattingEnabled = true;
            this.profanityListBox.Location = new System.Drawing.Point(7, 46);
            this.profanityListBox.Name = "profanityListBox";
            this.profanityListBox.Size = new System.Drawing.Size(144, 95);
            this.profanityListBox.TabIndex = 1;
            // 
            // profanityAdderTextBox
            // 
            this.profanityAdderTextBox.Location = new System.Drawing.Point(7, 20);
            this.profanityAdderTextBox.Name = "profanityAdderTextBox";
            this.profanityAdderTextBox.Size = new System.Drawing.Size(98, 20);
            this.profanityAdderTextBox.TabIndex = 2;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(41, 147);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 3;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // ProfanityOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wordManager);
            this.Controls.Add(this.censorCheckBox);
            this.Name = "ProfanityOptions";
            this.Size = new System.Drawing.Size(172, 213);
            this.wordManager.ResumeLayout(false);
            this.wordManager.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox censorCheckBox;
        private System.Windows.Forms.GroupBox wordManager;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.TextBox profanityAdderTextBox;
        private System.Windows.Forms.ListBox profanityListBox;
        private System.Windows.Forms.Button addButton;
    }
}
