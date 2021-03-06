﻿namespace Facebook_Message_Analyzer.Presentation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModulePreferencesForm));
            this.modules = new System.Windows.Forms.ListBox();
            this.ok = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // modules
            // 
            this.modules.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.modules.FormattingEnabled = true;
            this.modules.Location = new System.Drawing.Point(12, 12);
            this.modules.Name = "modules";
            this.modules.Size = new System.Drawing.Size(99, 238);
            this.modules.TabIndex = 0;
            this.modules.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.modules_DrawItem);
            this.modules.SelectedIndexChanged += new System.EventHandler(this.modules_SelectedIndexChanged);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(228, 226);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(390, 226);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 2;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(309, 226);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ModulePreferencesForm
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(477, 261);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.modules);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModulePreferencesForm";
            this.Text = "Module Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModulePreferencesForm_FormClosing);
            this.Resize += new System.EventHandler(this.ModulePreferencesForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox modules;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
    }
}