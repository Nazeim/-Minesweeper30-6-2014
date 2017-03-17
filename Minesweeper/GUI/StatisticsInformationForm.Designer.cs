namespace Minesweeper.GUI
{
    partial class StatisticsInformationForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.WonTimeslabel = new System.Windows.Forms.Label();
            this.PlayTimeslabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.LostTimeslabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.LostTimeslabel);
            this.panel1.Controls.Add(this.WonTimeslabel);
            this.panel1.Controls.Add(this.PlayTimeslabel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(11, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 95);
            this.panel1.TabIndex = 6;
            // 
            // WonTimeslabel
            // 
            this.WonTimeslabel.AutoSize = true;
            this.WonTimeslabel.Location = new System.Drawing.Point(102, 41);
            this.WonTimeslabel.Name = "WonTimeslabel";
            this.WonTimeslabel.Size = new System.Drawing.Size(58, 13);
            this.WonTimeslabel.TabIndex = 4;
            this.WonTimeslabel.Text = "WonTimes";
            // 
            // PlayTimeslabel
            // 
            this.PlayTimeslabel.AutoSize = true;
            this.PlayTimeslabel.Location = new System.Drawing.Point(105, 11);
            this.PlayTimeslabel.Name = "PlayTimeslabel";
            this.PlayTimeslabel.Size = new System.Drawing.Size(55, 13);
            this.PlayTimeslabel.TabIndex = 3;
            this.PlayTimeslabel.Text = "PlayTimes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "LostTimes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "WonTimes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PlayTimes";
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(188, 114);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // resetButton
            // 
            this.resetButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.resetButton.Location = new System.Drawing.Point(107, 114);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 4;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // LostTimeslabel
            // 
            this.LostTimeslabel.AutoSize = true;
            this.LostTimeslabel.Location = new System.Drawing.Point(102, 71);
            this.LostTimeslabel.Name = "LostTimeslabel";
            this.LostTimeslabel.Size = new System.Drawing.Size(55, 13);
            this.LostTimeslabel.TabIndex = 5;
            this.LostTimeslabel.Text = "LostTimes";
            // 
            // StatisticsInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 148);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.resetButton);
            this.Name = "StatisticsInformationForm";
            this.Text = "StatisticsInformationForm";
            this.Load += new System.EventHandler(this.StatisticsInformationForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label WonTimeslabel;
        private System.Windows.Forms.Label PlayTimeslabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LostTimeslabel;
    }
}