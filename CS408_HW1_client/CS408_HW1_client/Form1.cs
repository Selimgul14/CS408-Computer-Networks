using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CS408_HW1_client
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_IP.Text;
            int portNum;
            if (textBox_IP.Text == "")
            {
                logs.AppendText("Please enter an IP address!\n");
                return;
            }
            if (Int32.TryParse(textBox_Port.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    button_connect.Enabled = false;
                    button_disconnect.Enabled = true;
                    button_create.Enabled = true;
                    connected = true;
                    logs.AppendText("You are connected!\n");
                    Thread receiveThread = new Thread(Receive);
                    receiveThread.Start();

                }
                catch
                {
                    logs.AppendText("Couldn't connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port number!\n");
            }
        }

        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Replace("\0", string.Empty);
                    if (incomingMessage == "exists")
                    {
                        logs.AppendText("There is already an account with this username!\n");
                    }
                    if (incomingMessage == "success")
                    {
                        logs.AppendText("You have created an account!\n");
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected!\n");
                        button_connect.Enabled = true;
                    }

                    clientSocket.Close();
                    connected = false; 
                }
            }
            button_connect.Enabled = true;
            button_create.Enabled = false;
            button_disconnect.Enabled = false;
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private bool checkFields()
        {
            if (textBox_Name.Text == "")
            {
                logs.AppendText("Please enter a name!\n");
                return false;
            }
            if (textBox_Surname.Text == "")
            {
                logs.AppendText("Please enter a surname!\n");
                return false;
            }
            if (textBox_Username.Text == "")
            {
                logs.AppendText("Please enter a username!\n");
                return false;
            }
            if (textBox_Password.Text == "")
            {
                logs.AppendText("Please enter a password!\n");
                return false;
            }
            return true;

        }
        private void button_create_Click(object sender, EventArgs e)
        {
            if (checkFields())
            {
                string name = textBox_Name.Text;
                string surname = textBox_Surname.Text;
                string username = textBox_Username.Text;
                string password = textBox_Password.Text;

                string message = name + ":" + surname + ":" + username + ":" + password;

                Byte[] buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer);
            }

        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            connected = false;
            terminating = true;
            clientSocket.Close();
            logs.AppendText("Successfully disconnected!");
            button_connect.Enabled = true;
            button_disconnect.Enabled = false;
            button_create.Enabled = false;
        }
    }
}