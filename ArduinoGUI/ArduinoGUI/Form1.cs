using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ArduinoGUI
{
    public partial class Form1 : Form
    {
        ArduinoAPI API;

        public Form1()
        {
            InitializeComponent();

            API = new ArduinoAPI();
        }

        private void btn_conectar_Click(object sender, EventArgs e)
        {
            if (!API.isOpen())
            {
                int numPort = (int)num_puerto.Value;
                API.setPort(numPort);
                API.openPort();               
            }
            else
                API.closePort();

        }

        private void update_Tick(object sender, EventArgs e)
        {
            updateConectionStatus();

        }

        private void updateConectionStatus()
        {
            if(API.isOpen())
            {
                label_estado.Text = "Conectado";
                btn_conectar.Text = "Desconectar";
                num_puerto.Enabled = false;
            }
            else
            {
                label_estado.Text = "No conectado";
                btn_conectar.Text = "Conectar";
                num_puerto.Enabled = true;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
