using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Week4
{
    public partial class Form1 : Form
    {
        int startflag = 0;
        int flag_sensor;
        string RxString;
        string temp = "30";

        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM27";
            serialPort1.BaudRate = 115200;

            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                // startSerial.Enabled = false;
                // serialStop.Enabled = true;
                textbox.ReadOnly = false;
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            //  startSerial.Enabled = true;
            //  serialStop.Enabled = false;
            textbox.ReadOnly = true;
        }

        private void label_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();

            serialPort1.PortName = "COM27";
            serialPort1.BaudRate = 115200;
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Current_data_Click(object sender, EventArgs e)
        {
            textbox.AppendText(RxString);
        }


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(Current_data_Click));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!string.Equals(textbox.Text, ""))
            {
                if (serialPort1.IsOpen) serialPort1.Close();
                try
                {
                    if (RxString[0] == 'B')
                    {
                        flag_sensor = 10;

                    }

                    const string WRITEKEY = "QV0YVADUH2VCSU25";
                    string strUpdateBase = "http://api.thingspeak.com/update";


                    string strUpdateURI = strUpdateBase + "?api_key=" + WRITEKEY;
                    string strField1 = textbox.Text;

                    HttpWebRequest ThingsSpeakReq;
                    HttpWebResponse ThingsSpeakResp;

                    if (flag_sensor == 10)
                    {

                        strUpdateURI += "&field1=" + strField1;
                        flag_sensor = 10;
                    }



                    ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(strUpdateURI);
                    ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();
                    ThingsSpeakResp.Close();

                    if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                    {
                        Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                        throw exData;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                textbox.Text = "";


                serialPort1.Open();
            }

        }
    }
}
