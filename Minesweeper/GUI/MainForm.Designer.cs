namespace Minesweeper.GUI
{
    partial class MainForm
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
            this.newB = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.minesCountL = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.seedTB = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearB = new System.Windows.Forms.Button();
            this.copyB = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.networkStatusL = new System.Windows.Forms.ToolStripStatusLabel();
            this.mineFieldTLP = new System.Windows.Forms.TableLayoutPanel();
            this.BlockGUIbtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // newB
            // 
            this.newB.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newB.Location = new System.Drawing.Point(12, 33);
            this.newB.Name = "newB";
            this.newB.Size = new System.Drawing.Size(53, 36);
            this.newB.TabIndex = 1;
            this.newB.Text = "New";
            this.newB.UseVisualStyleBackColor = true;
            this.newB.Click += new System.EventHandler(this.newB_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.Red;
            this.statusLabel.Location = new System.Drawing.Point(3, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(144, 32);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Game Lost!";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statusLabel.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.Controls.Add(this.statusLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.minesCountL, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 581);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(499, 32);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(153, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(79, 18);
            this.panel1.TabIndex = 3;
            // 
            // minesCountL
            // 
            this.minesCountL.AutoSize = true;
            this.minesCountL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.minesCountL.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minesCountL.Location = new System.Drawing.Point(433, 0);
            this.minesCountL.Margin = new System.Windows.Forms.Padding(0);
            this.minesCountL.Name = "minesCountL";
            this.minesCountL.Size = new System.Drawing.Size(33, 32);
            this.minesCountL.TabIndex = 4;
            this.minesCountL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Minesweeper.Properties.Resources.mine;
            this.pictureBox1.Location = new System.Drawing.Point(469, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Seed:";
            // 
            // seedTB
            // 
            this.seedTB.Location = new System.Drawing.Point(109, 42);
            this.seedTB.Name = "seedTB";
            this.seedTB.Size = new System.Drawing.Size(88, 20);
            this.seedTB.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.networkToolStripMenuItem,
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(520, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // networkToolStripMenuItem
            // 
            this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.configurationsToolStripMenuItem});
            this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
            this.networkToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.networkToolStripMenuItem.Text = "Network";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // configurationsToolStripMenuItem
            // 
            this.configurationsToolStripMenuItem.Name = "configurationsToolStripMenuItem";
            this.configurationsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.configurationsToolStripMenuItem.Text = "Configurations";
            this.configurationsToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statisticsToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // clearB
            // 
            this.clearB.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearB.Location = new System.Drawing.Point(202, 42);
            this.clearB.Name = "clearB";
            this.clearB.Size = new System.Drawing.Size(43, 20);
            this.clearB.TabIndex = 3;
            this.clearB.Text = "clear";
            this.clearB.UseVisualStyleBackColor = true;
            this.clearB.Click += new System.EventHandler(this.clearB_Click);
            // 
            // copyB
            // 
            this.copyB.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyB.Location = new System.Drawing.Point(247, 42);
            this.copyB.Name = "copyB";
            this.copyB.Size = new System.Drawing.Size(43, 20);
            this.copyB.TabIndex = 4;
            this.copyB.Text = "copy";
            this.copyB.UseVisualStyleBackColor = true;
            this.copyB.Click += new System.EventHandler(this.copyB_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.networkStatusL});
            this.statusStrip1.Location = new System.Drawing.Point(0, 616);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(520, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // networkStatusL
            // 
            this.networkStatusL.Name = "networkStatusL";
            this.networkStatusL.Size = new System.Drawing.Size(0, 17);
            // 
            // mineFieldTLP
            // 
            this.mineFieldTLP.AutoSize = true;
            this.mineFieldTLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mineFieldTLP.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.mineFieldTLP.ColumnCount = 16;
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mineFieldTLP.Location = new System.Drawing.Point(12, 75);
            this.mineFieldTLP.Name = "mineFieldTLP";
            this.mineFieldTLP.RowCount = 16;
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mineFieldTLP.Size = new System.Drawing.Size(17, 17);
            this.mineFieldTLP.TabIndex = 0;
            // 
            // BlockGUIbtn
            // 
            this.BlockGUIbtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlockGUIbtn.Location = new System.Drawing.Point(427, 37);
            this.BlockGUIbtn.Name = "BlockGUIbtn";
            this.BlockGUIbtn.Size = new System.Drawing.Size(81, 32);
            this.BlockGUIbtn.TabIndex = 6;
            this.BlockGUIbtn.Text = "BlockGUI";
            this.BlockGUIbtn.UseVisualStyleBackColor = true;
            this.BlockGUIbtn.Click += new System.EventHandler(this.BlockGUIbtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 638);
            this.Controls.Add(this.BlockGUIbtn);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.copyB);
            this.Controls.Add(this.clearB);
            this.Controls.Add(this.seedTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.newB);
            this.Controls.Add(this.mineFieldTLP);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Minesweeper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button newB;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox seedTB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label minesCountL;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
        private System.Windows.Forms.Button clearB;
        private System.Windows.Forms.Button copyB;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel mineFieldTLP;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel networkStatusL;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.Button BlockGUIbtn;


    }
}

