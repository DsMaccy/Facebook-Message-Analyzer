namespace Facebook_Message_Analyzer.Presentation
{
    partial class ConversationSelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversationSelectionForm));
            this.previous = new System.Windows.Forms.Button();
            this.next = new System.Windows.Forms.Button();
            this.conversations = new System.Windows.Forms.DataGridView();
            this.refresh = new System.Windows.Forms.Button();
            this.menu = new System.Windows.Forms.ToolStrip();
            this.analyze = new System.Windows.Forms.ToolStripButton();
            this.moduleSelect = new System.Windows.Forms.ToolStripButton();
            this.moduleOptions = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.conversations)).BeginInit();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // previous
            // 
            this.previous.Location = new System.Drawing.Point(12, 332);
            this.previous.Name = "previous";
            this.previous.Size = new System.Drawing.Size(75, 23);
            this.previous.TabIndex = 0;
            this.previous.Text = "previous";
            this.previous.UseVisualStyleBackColor = true;
            this.previous.Click += new System.EventHandler(this.previous_Click);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(461, 332);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(75, 23);
            this.next.TabIndex = 1;
            this.next.Text = "next";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.next_Click);
            // 
            // conversations
            // 
            this.conversations.AllowUserToAddRows = false;
            this.conversations.AllowUserToDeleteRows = false;
            this.conversations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.conversations.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.conversations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.conversations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.conversations.Location = new System.Drawing.Point(12, 28);
            this.conversations.MultiSelect = false;
            this.conversations.Name = "conversations";
            this.conversations.ReadOnly = true;
            this.conversations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.conversations.Size = new System.Drawing.Size(523, 288);
            this.conversations.TabIndex = 2;
            this.conversations.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.analyze_click);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(247, 332);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(75, 23);
            this.refresh.TabIndex = 3;
            this.refresh.Text = "refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyze,
            this.moduleSelect,
            this.moduleOptions});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(548, 25);
            this.menu.TabIndex = 4;
            this.menu.Text = "toolStrip1";
            // 
            // analyze
            // 
            this.analyze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.analyze.Image = ((System.Drawing.Image)(resources.GetObject("analyze.Image")));
            this.analyze.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.analyze.Name = "analyze";
            this.analyze.Size = new System.Drawing.Size(52, 22);
            this.analyze.Text = "Analyze";
            this.analyze.Click += new System.EventHandler(this.analyze_click);
            // 
            // moduleSelect
            // 
            this.moduleSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.moduleSelect.Image = ((System.Drawing.Image)(resources.GetObject("moduleSelect.Image")));
            this.moduleSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moduleSelect.Name = "moduleSelect";
            this.moduleSelect.Size = new System.Drawing.Size(137, 22);
            this.moduleSelect.Text = "Select Analysis Modules";
            this.moduleSelect.Click += new System.EventHandler(this.moduleSelect_click);
            // 
            // moduleOptions
            // 
            this.moduleOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.moduleOptions.Image = ((System.Drawing.Image)(resources.GetObject("moduleOptions.Image")));
            this.moduleOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moduleOptions.Name = "moduleOptions";
            this.moduleOptions.Size = new System.Drawing.Size(97, 22);
            this.moduleOptions.Text = "Module Options";
            this.moduleOptions.Click += new System.EventHandler(this.moduleOptions_Click);
            // 
            // ConversationSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 367);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.conversations);
            this.Controls.Add(this.next);
            this.Controls.Add(this.previous);
            this.Name = "ConversationSelectionForm";
            this.Text = "ConversationSelectionForm";
            ((System.ComponentModel.ISupportInitialize)(this.conversations)).EndInit();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button previous;
        private System.Windows.Forms.Button next;
        private System.Windows.Forms.DataGridView conversations;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.ToolStrip menu;
        private System.Windows.Forms.ToolStripButton analyze;
        private System.Windows.Forms.ToolStripButton moduleSelect;
        private System.Windows.Forms.ToolStripButton moduleOptions;
    }
}