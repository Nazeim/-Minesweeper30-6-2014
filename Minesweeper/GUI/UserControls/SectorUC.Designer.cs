namespace Minesweeper.GUI.UserControls
{
    partial class SectorUC
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
            this.label = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.TableLayoutPanel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(3, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(20, 26);
            this.label.TabIndex = 1;
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label.Visible = false;
            // 
            // button
            // 
            this.button.BackColor = System.Drawing.Color.SteelBlue;
            this.button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button.Location = new System.Drawing.Point(0, 0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(26, 26);
            this.button.TabIndex = 0;
            this.button.TabStop = false;
            this.button.UseVisualStyleBackColor = false;
            this.button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_MouseClick);
            this.button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_MouseDown);
            this.button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_MouseUp);
            // 
            // panel
            // 
            this.panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel.ColumnCount = 1;
            this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel.Controls.Add(this.label, 0, 0);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.RowCount = 1;
            this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panel.Size = new System.Drawing.Size(26, 26);
            this.panel.TabIndex = 2;
            // 
            // SectorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button);
            this.Controls.Add(this.panel);
            this.Name = "SectorUC";
            this.Size = new System.Drawing.Size(26, 26);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TableLayoutPanel panel;
    }
}
