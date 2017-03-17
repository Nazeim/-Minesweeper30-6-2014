using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Minesweeper.GUI.UserControls;
using Minesweeper.Model;
using Minesweeper.Properties;

namespace Minesweeper.GUI
{
    //TODO 16x16 dimensions: 530; 670
    public partial class MainForm : Form
    {
        //TODO make more difficulty options
        //TODO highlight the exploded mine

        public enum NetworkStatus
        {
            Stopped,
            Listening,
            Playing
        }

        private const int WIDTH = 16;
        private const int HEIGHT = 16;
        private const int MINES_COUNT = 40;
        private Model.Minesweeper minesweeper = new Model.Minesweeper();
        private int networkPort = 3614;
        

        private string NetworkStatusLabel
        {
            set
            {
                networkStatusL.Text = value;
            }

            get
            {
                return networkStatusL.Text;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            minesweeper.RevealSectorRequested += new EventHandler<Model.Minesweeper.RevealSectorsEventArgs>(minesweeper_RevealSectorRequested);
            minesweeper.MarkSectorRequested += new EventHandler<Model.Minesweeper.MarkSectorEventArgs>(minesweeper_MarkSectorRequested);
            minesweeper.NewGameRequested += new EventHandler<Model.Minesweeper.NewGameEventArgs>(minesweeper_NewGameRequested);
            minesweeper.AgentConnected += new EventHandler<EventArgs>(minesweeper_AgentConnected);
            minesweeper.AgentDisconnected += new EventHandler<EventArgs>(minesweeper_AgentDisconnected);

            UpdateNetworkStatus(NetworkStatus.Stopped);
            NewGame();
        }
        private SectorUC FindSecctorByIndex(int xIndex, int yIndex)
        {
            int linearIndex = yIndex + WIDTH * xIndex;

            return (SectorUC)mineFieldTLP.Controls[linearIndex];
        }

        #region status

        private void Lose()
        {
            statusLabel.Text = "Game Lost!";
            statusLabel.ForeColor = Color.Red;
            statusLabel.Show();
        }

        private void Win()
        {
            statusLabel.Text = "Game Won!";
            statusLabel.ForeColor = Color.Green;
            statusLabel.Show();
        }

        private void UpdateState()
        {
            switch (minesweeper.State)
            {
                case Model.Minesweeper.GameState.LOST:
                    Lose();
                    break;
                case Model.Minesweeper.GameState.WON:
                    Win();
                    break;
            }

            minesCountL.Text = (minesweeper.MinesCount - minesweeper.MarkedSetorsCount).ToString();
        }

        private void UpdateNetworkStatus(NetworkStatus newStatus)
        {
            switch (newStatus)
            {
                case NetworkStatus.Listening:
                    NetworkStatusLabel = string.Format("Listening to port ({0})", networkPort);
                    break;
                case NetworkStatus.Playing:
                    NetworkStatusLabel = "Agent connected";
                    break;
                case NetworkStatus.Stopped:
                    NetworkStatusLabel = "Network stopped";
                    break;
            }
        }
        #endregion

        #region game generation
        private void clearB_Click(object sender, EventArgs e)
        {
            seedTB.Text = "";
        }

        private void copyB_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(seedTB.Text);
        }

        private void newB_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            if (seedTB.Text == "")
            {
                minesweeper.NewGame(HEIGHT, WIDTH, MINES_COUNT);
            }
            else
            {
                int seed = Convert.ToInt32(seedTB.Text);
                minesweeper.NewGame(HEIGHT, WIDTH, MINES_COUNT, seed);
            }

            InitializeFieldForNewGame();
        }

        private void InitializeFieldForNewGame()
        {
            statusLabel.Hide();
            mineFieldTLP.Hide();
            minesCountL.Text = minesweeper.MinesCount.ToString();
            seedTB.Text = minesweeper.Seed.ToString();
            mineFieldTLP.Controls.Clear();
            SectorUC current;

            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    current = GenerateSector(i, j);
                    mineFieldTLP.Controls.Add(current, j, i);
                }
            }

            mineFieldTLP.Show();
        }
        private SectorUC GenerateSector(int xIndex, int yIndex)
        {
            SectorUC current = new SectorUC();
            current.XIndex = xIndex;
            current.YIndex = yIndex;
            current.Dock = DockStyle.Fill;
            current.Width = 30;
            current.Height = 30;
            current.Margin = new System.Windows.Forms.Padding(0);
            current.SectorClicked += new EventHandler<SectorUC.SectorClickEventArgs>(sectorUC_SectorClicked);
            current.FlagImage = Properties.Resources._1399253824_Flag_red;
            current.MineImage = Properties.Resources.mine;
            current.TabStop = false;
            //current.UnrevealedSectorColor = Color.Black;

            return current;
        }
        #endregion

        #region gameplay
        private void sectorUC_SectorClicked(object sender, GUI.UserControls.SectorUC.SectorClickEventArgs e)
        {
            if (e.ClickButton == SectorUC.SectorClickButton.LEFT_CLICK)
            {
                RevealSector(((SectorUC)sender).XIndex, ((SectorUC)sender).YIndex);
            }
            else if (e.ClickButton == SectorUC.SectorClickButton.RIGHT_CLICK)
            {
                int x = ((SectorUC)sender).XIndex;
                int y = ((SectorUC)sender).YIndex;
                ToggleMark(x, y);
            }
        }

        private void ShowFlag(int x, int y)
        {
            SectorUC sector = FindSecctorByIndex(x, y);
            sector.Mark();
        }
        private void ToggleMark(int x, int y)
        {
            minesweeper.ToggleMark(x, y);
            UpdateState();
        }

        private void RevealSector(int x, int y)
        {
            List<Sector> toReveal = minesweeper.RevealSector(x, y);
            RevealSectors(toReveal);
        }

        private void RevealSectors(List<Sector> toReveal)
        {
            UpdateState();
            SectorUC current;

            foreach (Sector s in toReveal)
            {
                current = FindSecctorByIndex(s.X, s.Y);
                current.HasMine = s.HasMine;
                current.Number = s.Value;
                current.Reveal();
            }
        }

        #endregion

        #region network
        void minesweeper_NewGameRequested(object sender, Model.Minesweeper.NewGameEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (e.HAS_SEED)
                        seedTB.Text = e.SEED.ToString();
                    else
                        seedTB.Text = "";

                    //TODO should read game properties from the event args
                    InitializeFieldForNewGame();
                }));
            }
            catch
            { }
        }

        void minesweeper_MarkSectorRequested(object sender, Model.Minesweeper.MarkSectorEventArgs e)
        {
            
                try
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        ShowFlag(e.Position.XIndex, e.Position.YIndex);
                        UpdateState();
                    }));
                }
                catch
                { }
           
        }

        void minesweeper_RevealSectorRequested(object sender, Model.Minesweeper.RevealSectorsEventArgs e)
        {
              try
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        RevealSectors(e.ToRevealSectors);
                    }));
                }
                catch
                { }
        }

        void minesweeper_AgentDisconnected(object sender, EventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    UpdateNetworkStatus(NetworkStatus.Listening);
                }));
            }
            catch
            { }
        }

        void minesweeper_AgentConnected(object sender, EventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    UpdateNetworkStatus(NetworkStatus.Playing);
                }));
            }
            catch
            { }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minesweeper.StartNetwork(networkPort);
            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            UpdateNetworkStatus(NetworkStatus.Listening);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minesweeper.StopNetwork();
            startToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            UpdateNetworkStatus(NetworkStatus.Stopped);
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkConfigurationsForm form = new NetworkConfigurationsForm(networkPort);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                networkPort = form.PortNumber;
            }
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            minesweeper.StopNetwork();
        }


        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatisticsInformationForm form = new StatisticsInformationForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.Retry)
            {
                Settings.Default.totalGamesCount = 0;
                Settings.Default.totalGamesWon = 0;
                Settings.Default.Save();
            }
        }

        private void BlockGUIbtn_Click(object sender, EventArgs e)
        {
            minesweeper.BlockGUI = !minesweeper.BlockGUI;
            if (minesweeper.BlockGUI)
                BlockGUIbtn.Text = "UnBlockGUI";
            else
                BlockGUIbtn.Text = "BlockGUI";
        }

       
    }
}
