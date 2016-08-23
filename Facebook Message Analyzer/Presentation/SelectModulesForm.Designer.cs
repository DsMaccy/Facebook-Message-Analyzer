namespace Facebook_Message_Analyzer.Presentation
{
    partial class SelectModulesForm
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
            this.components = new System.ComponentModel.Container();
            this.title = new System.Windows.Forms.Label();
            this.checkedList = new System.Windows.Forms.CheckedListBox();
            this.moduleDescription = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.title.Location = new System.Drawing.Point(61, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(180, 20);
            this.title.TabIndex = 0;
            this.title.Text = "Select Analysis Modules";
            // 
            // checkedList
            // 
            this.checkedList.FormattingEnabled = true;
            this.checkedList.Location = new System.Drawing.Point(12, 35);
            this.checkedList.Name = "checkedList";
            this.checkedList.Size = new System.Drawing.Size(260, 214);
            this.checkedList.TabIndex = 1;
            this.checkedList.MouseHover += new System.EventHandler(this.checkedList_MouseHover);
            // 
            // SelectModulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.checkedList);
            this.Controls.Add(this.title);
            this.Name = "SelectModulesForm";
            this.Text = "SelectModulesForm";
            this.Resize += new System.EventHandler(this.SelectModulesForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        private System.Windows.Forms.CheckedListBox checkedList;
        private System.Windows.Forms.ToolTip moduleDescription;
    }
}