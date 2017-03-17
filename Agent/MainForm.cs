using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Agent
{
    public partial class MainForm : Form
    {
        private AbstractAgent agent;
        private int portNumber = 3614;
        private string ipAddress = "127.0.0.1";
        private const int WIDTH = 16;
        private const int HEIGHT = 16;
        private const int MINES_COUNT = 40;
        private const int REPETITIONS_COUNT = 100;
        private bool isBatch = false;
        private bool useOldAgent = false;


        public MainForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        #region Helper
        private void InitializeAgent()
        {
            if (!useOldAgent)
                agent = new NewAgent(ipAddress, portNumber);
            else
                agent = new OldAgent(ipAddress, portNumber);

            agent.AgentStatus += new EventHandler<AbstractAgent.AgentStatusEventArgs>(agent_AgentStatus);
            agent.DisconnectedFromServer += new EventHandler<EventArgs>(agent_DisconnectedFromServer);
        }
        private void AddLog(string message)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    TimeSpan span = DateTime.Now.TimeOfDay;
                    TimeSpan span1 = new TimeSpan(span.Hours, span.Minutes, span.Seconds);
                    textBox1.Text += string.Format("{0:c} : {1}.", span1, message) + "\n";
                    textBox1.Focus();
                    textBox1.Select(textBox1.Text.Length - 1, 0);
                }));
            }
            catch
            { }

        }
        private void SetNetworkStateStatus(bool isStarted)
        {
            startGameToolStripMenuItem.Enabled = !isStarted;
            stopGameToolStripMenuItem.Enabled = isStarted;
            AddLog(isStarted ? "Network Started\n**********************" : "Network Stopped\n***********************");
        }
        #endregion

        #region Agent Events
        void agent_DisconnectedFromServer(object sender, EventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    stopGameToolStripMenuItem.PerformClick();

                    if (isBatch)
                        progressBar1.Value++;
                }
                ));
            }
            catch
            { }
        }

        void agent_AgentStatus(object sender, AbstractAgent.AgentStatusEventArgs e)
        {
            if (!isBatch)
            {
                AddLog(e.Description);
            }
        }
        #endregion

        #region Form Events
        private void clearB_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void configurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkConfigurationsForm form = new NetworkConfigurationsForm(portNumber, ipAddress);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                portNumber = form.PortNumber;
                ipAddress = form.IPAddress;
            }
        }

        private void startGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isBatch = false;

            if (agent == null || agent.Networkstatus == AbstractAgent.NetStatus.NotConnected)
                InitializeAgent();

            SetNetworkStateStatus(true);
            agent.StartPlaying(HEIGHT, WIDTH, MINES_COUNT, true);

        }

        private void stopGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (agent != null)
                agent.ForceStop();

            SetNetworkStateStatus(false);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopGameToolStripMenuItem.PerformClick();
        }

        private void btnNextStep_Click(object sender, EventArgs e)
        {
            isBatch = false;

            if (agent == null || agent.Networkstatus == AbstractAgent.NetStatus.NotConnected)
                InitializeAgent();

            agent.StartPlaying(HEIGHT, WIDTH, MINES_COUNT, false);
            //SetNetorkStateStatus(true);
        }
        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = REPETITIONS_COUNT;
            progressBar1.Value = 0;
            isBatch = true;

            if (agent == null || agent.Networkstatus == AbstractAgent.NetStatus.NotConnected)
                InitializeAgent();

            agent.PlayBatch(HEIGHT, WIDTH, MINES_COUNT, REPETITIONS_COUNT);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            useOldAgent = comboBox1.SelectedIndex == 0;
            agent = null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }


    }
}
