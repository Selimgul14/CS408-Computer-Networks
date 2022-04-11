using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CS408_HW1_server
{
    public partial class Form1 : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();

        bool terminating = false;
        bool listening = false;
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            InitializeComponent();
        }

        private void button_listen_Click(object sender, EventArgs e)
        {

            int serverPort;

            if (Int32.TryParse(textBox_Port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                button_listen.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                server_logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                server_logs.AppendText("Check port number!\n");
            }
        }

        private void Accept()
        {
            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept();
                    clientSockets.Add(newClient);
                    server_logs.AppendText("A client is connected!\n");
                 
                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        server_logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }
        
        private void Receive(Socket thisClient)
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    string response = "";
                    Byte[] buffer = new Byte[64];
                    thisClient.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Trim();
                    //server_logs.AppendText("Client: " + incomingMessage + "\n");
                    checkAndAddDatabase(incomingMessage, ref response);
                    Byte[] buff = Encoding.Default.GetBytes(response);
                    thisClient.Send(buff);
                }
                catch
                {
                    if (!terminating)
                    {
                        server_logs.AppendText("A client has disconnected!\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }
        
        private void checkAndAddDatabase(string message, ref string response)
        {
            string[] words = message.Split(':');
            string username = words[2];
            var lines = new List<string>();
            using (StreamReader reader = new StreamReader(@"C:/Users/selim/source/repos/CS408_HW1_server/CS408_HW1_server/database.txt"))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
            }
                using (StreamWriter file = new StreamWriter("C:/Users/selim/source/repos/CS408_HW1_server/CS408_HW1_server/database.txt", append: true))
            {
                if (new FileInfo("C:/Users/selim/source/repos/CS408_HW1_server/CS408_HW1_server/database.txt").Length == 0)
                {
                    file.WriteLine(message);
                    response = "success";
                    server_logs.AppendText(username + " has created an account!.\n");
                }
                else
                {
                    bool found = false;
                    foreach (string line in lines)
                    {
                        string[] toParse = line.Split(':');
                        string user = toParse[2];
                        if (user == username)
                        {
                            server_logs.AppendText("An acount with the username " + username +  " alredy exists!\n");
                            response = "exists";
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        file.WriteLine(message);
                        response = "success";
                        server_logs.AppendText(username + " has created an account!\n");
                        
                    }
                }

            }
            


        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }
    }
}