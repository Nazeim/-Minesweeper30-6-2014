using System;   
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Agent
{
    public partial class NetworkConfigurationsForm : Form
    {
        public int PortNumber
        {
            set
            {
                if(value >= numericUpDown1.Minimum && value<= numericUpDown1.Maximum)
                    numericUpDown1.Value = value;
            }

            get
            {
                return (int)numericUpDown1.Value;
            }
        }

        public string IPAddress
        {
            set
            {
                textBox1.Text = value;
            }

            get
            {
                return textBox1.Text;
            }
        }

        public NetworkConfigurationsForm()
        {
            InitializeComponent();
        }

        public NetworkConfigurationsForm(int port, string ipAddress)
            :this()
        {
            PortNumber = port;
            IPAddress = ipAddress;
        }


    }
}
