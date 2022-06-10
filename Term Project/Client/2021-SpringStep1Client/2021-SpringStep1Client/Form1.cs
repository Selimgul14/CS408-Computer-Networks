using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2021_SpringStep1Client
{
    public partial class Form1 : Form
    {
        bool terminating = false; //this is the will of the client to disconnect from the server. 
        bool connected = false; //Initially the client is not connected.
        bool disconnectPressed = false;
        string clientUsername = "";
        Socket clientSocket; 
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);// do it in order to not need any delegate to access the GUI object. 
            InitializeComponent();
        }

   
        private void Button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // IPv4, stream to exchange messages byte array forms.
            string IP = textBoxIP.Text;
            int Port;

            if(Int32.TryParse(textBoxPort.Text, out Port)) // check if integer or not if not sure use it in that way with TryParse
            {
                string username = textBoxUsername.Text;
                //Possible Problems.
                if(username == "")
                { clientLogs.AppendText("Please enter you username.\n");}
                else if(IP == "")
                { clientLogs.AppendText("Please enter an IP address.\n"); }

                //If no problem related to textboxes in order to connect.
                else
                {
                    try 
                    {
                        clientSocket.Connect(IP, Port);
                        try
                        {
                            clientSocket.Send(Encoding.Default.GetBytes(username));
                            try
                            {
                                Byte[] responsebuffer = new Byte[64];
                                clientSocket.Receive(responsebuffer);
                                string response = Encoding.Default.GetString(responsebuffer);
                                //response = response.Substring(0, response.IndexOf("\0"));
                                response = response.Trim('\0');

                                if (response == "NOT_FOUND")
                                {
                                    clientLogs.AppendText("Please enter a valid username.\n");// not a db user, error. 
                                }
                                else if (response == "Already_Connected")
                                {
                                    clientLogs.AppendText("This user is already connected.\n");
                                }
                                else if (response == "SUCCESS")
                                {
                                    
                                    Button_connect.Enabled = false; // since we are connected we should not able to click to connect again. 
                                    button_SendPost.Enabled = true;
                                    button_Disconnect.Enabled = true;
                                    button_AllPosts.Enabled = true;
                                    button_DeletePost.Enabled = true;
                                    button_remove.Enabled = true;
                                    textBox_friend.Enabled = true;
                                    button_AddFriend.Enabled = true;
                                    button_FriendPost.Enabled = true;
                                    textBox_PostID.Enabled = true;
                                    button_DeletePost.Enabled = true;
                                    button_ShowFriends.Enabled = true;
                                    textBoxPost.Enabled = true; // should be able to send posts.
                                    disconnectPressed = false;
                                    connected = true;
                                    clientUsername = username;
                                    clientLogs.AppendText("Hello " + username + "! You are connected to the server.\n");

                                    Thread receiveThread = new Thread(Receive);
                                    receiveThread.Start();
                                }
                            }
                            catch
                            {
                                clientLogs.AppendText("There was a problem receiving response.\n");
                            }
                        }
                        catch
                        {
                            clientLogs.AppendText("Problem occured while username is sent.\n");
                        }
                    }
                    catch
                    {
                        clientLogs.AppendText("Could not connect to the server.\n");
                    }
                    
                }
            }
            else
            {
                clientLogs.AppendText("Check the port\n");
            }
        }

        private void button_AddFriend_Click(object sender, EventArgs e)
        {
            string message = "ADD_FRINDS" + textBox_friend.Text;
            if (message != "" && message.Length <= 64)//correct format of the message. 
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                try
                {
                    clientSocket.Send(buffer);

                }
                catch
                {
                    clientLogs.AppendText("There was a problem sending the post to the server.\n");
                }
            }
        }

        private void button_SendPost_Click(object sender, EventArgs e)
        {
            string message = "SEND_POSTS" + textBoxPost.Text;
            textBoxPost.Text = "";
            if (message != "" && message.Length <= 64)//correct format of the message. 
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                try
                {
                    clientSocket.Send(buffer);

                    clientLogs.AppendText("You have successfully sent a post!\n");
                    clientLogs.AppendText(clientUsername + ": " + message.Substring(10) + " \n");
                }
                catch
                {
                    clientLogs.AppendText("There was a problem sending the post to the server.\n");
                }
            }

        }

        private void button_AllPosts_Click(object sender, EventArgs e)
        {
            string message = "SHOW_POSTS";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            try
            {
                clientLogs.AppendText("\nShowing all posts from clients: \n");
                clientSocket.Send(buffer);
            }
            catch
            {
                clientLogs.AppendText("There was a problem in the request of reaching posts page to server.\n");
            }

        }
        private void Receive()
        {
            while (connected)
            { 
                try
                {   Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);

                    Byte[] response = Encoding.Default.GetBytes("receivedinfo");
                    clientSocket.Send(response);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    //incomingMessage = incomingMessage.Trim('\0');

                   string label = incomingMessage.Substring(0, 10);

                    if (label == "SHOW_POSTS")
                    {
                       clientLogs.AppendText(incomingMessage.Substring(10) + "\n");
                    }
                    else if (label == "ADDEDSUCCE")
                    {
                        clientLogs.AppendText("You have added " + textBox_friend.Text + " as a friend!\n");

                    }
                    else if (label == "ALREADYADD")
                    {
                        clientLogs.AppendText(textBox_friend.Text + " is already added as your friend\n");
                    }
                    else if (label == "REMOVEFAIL")
                    {
                        clientLogs.AppendText("No such friend found to remove from your friends list\n");
                    }
                    else if (label == "REMOVESCCS")
                    {
                        string friend = incomingMessage.Substring(10);
                        clientLogs.AppendText("You have removed "+ friend + " from your friend list\n");
                    }
                    else if (label == "REMOVEPOST")
                    {
                        string postID = incomingMessage.Substring(10);
                        clientLogs.AppendText("You have deleted post with PostID: " + postID + "\n");
                    }
                    else if (label == "RMVPSTFAIL")
                    {
                        string postID = incomingMessage.Substring(10);
                        clientLogs.AppendText("Post with PostID: " + postID + " doesn't belong to you!\n");
                    }
                    else if (label == "PSTNOEXIST")
                    {
                        string postID = incomingMessage.Substring(10);
                        clientLogs.AppendText("There is no post with PostID: " + postID + "\n");
                    }
                    else if (label == "SNDFRIENDS")
                    {
                        string friendStr = incomingMessage.Substring(10);
                        string[] friendList = friendStr.Split(':');
                        foreach (string friend in friendList)
                        {
                            clientLogs.AppendText(friend + "\n");
                        }
                    }
                    else if (label == "NOTEXISTSS")
                    {
                        clientLogs.AppendText("No such user exists in the database\n");
                    }
                    else if (label == "ADDYOURSEL")
                    {
                        clientLogs.AppendText("You cannot add yourself as a friend!\n");
                    }

                }
                catch
                {
                    if (!terminating && !disconnectPressed)//not terminating the action and not disconnected.
                    {
                        clientLogs.AppendText("The server has disconnected.\n");//Probably the server has stopped, client cannot connect then to that server. 
                        Button_connect.Enabled = true;
                        button_remove.Enabled = false;
                        textBox_friend.Enabled = false;
                        button_AddFriend.Enabled = false;
                        button_FriendPost.Enabled = false;
                        textBox_PostID.Enabled = false;
                        button_DeletePost.Enabled = false;
                        textBoxPost.Enabled = false;
                        button_SendPost.Enabled = false;
                        button_Disconnect.Enabled = false;
                        button_AllPosts.Enabled = false;
                        button_ShowFriends.Enabled = false;


                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
            //throw new NotImplementedException();
        }

        private void button_Disconnect_Click(object sender, EventArgs e)
        {
            string message = "DISCONNECT";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            try
            {
                clientSocket.Send(buffer);

                disconnectPressed = true;

                Button_connect.Enabled = true;
                button_remove.Enabled = false;
                textBox_friend.Enabled = false;
                button_AddFriend.Enabled = false;
                button_FriendPost.Enabled = false;
                textBox_PostID.Enabled = false;
                button_DeletePost.Enabled = false;
                textBoxPost.Enabled = false;
                button_SendPost.Enabled = false;
                button_Disconnect.Enabled = false;
                button_AllPosts.Enabled = false;
                button_ShowFriends.Enabled = false;
                clientSocket.Close();
                connected = false;

                clientLogs.AppendText("Successfuly disconnected.\n");
            }
            catch
            {
                clientLogs.AppendText("There was a problem sending disconnect request to server.\n");
            }
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            string message = "RMV_FRINDS" + textBox_friend.Text;
            if (message != "" && message.Length <= 64)//correct format of the message. 
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                try
                {
                    clientSocket.Send(buffer);

                    clientLogs.AppendText("You have successfully removed " + textBox_friend.Text + " as a friend!\n");

                }
                catch
                {
                    clientLogs.AppendText("There was a problem sending the post to the server.\n");
                }
            }
        }

        private void button_DeletePost_Click(object sender, EventArgs e)
        {
            string message = "DELETE_PST" + textBox_PostID.Text;
            if (message != "" && message.Length <= 64)//correct format of the message. 
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                try
                {
                    clientSocket.Send(buffer);
                }
                catch
                {
                    clientLogs.AppendText("There was a problem sending the post to the server.\n");
                }
            }
        }

        private void button_FriendPost_Click(object sender, EventArgs e)
        {
            string message = "SHOW_FRNDS";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            try
            {
                clientLogs.AppendText("\nShowing all posts from friends: \n");
                clientSocket.Send(buffer);
            }
            catch
            {
                clientLogs.AppendText("There was a problem in the request of reaching posts page to server.\n");
            }

        }

        private void button_ShowFriends_Click(object sender, EventArgs e)
        {
            string message = "FRIEND_LST";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            try
            {
                clientLogs.AppendText("\nShowing your friends: \n");
                clientSocket.Send(buffer);
            }
            catch
            {
                clientLogs.AppendText("There was a problem in the request of reaching friends list to server.\n");
            }

        }
    }
}
