using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Minesweeper.Properties;

namespace Minesweeper.GUI
{
    public partial class StatisticsInformationForm : Form
    {
        public StatisticsInformationForm()
        {
            InitializeComponent();
        }

        private void StatisticsInformationForm_Load(object sender, EventArgs e)
        {
            PlayTimeslabel.Text =Settings.Default.totalGamesCount.ToString();
            WonTimeslabel.Text = Settings.Default.totalGamesWon.ToString();
            LostTimeslabel.Text = (Settings.Default.totalGamesCount - Settings.Default.totalGamesWon).ToString();
        }
    }
}
