using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2021_SpringStep1Server
{
    public partial class Form1 : Form
    {
        int lineCount = File.ReadLines(@"../../user-db.txt").Count();

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        List<string> clientusernames = new List<string>();

        int postCount = CountPost();

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
            int port;
            if (Int32.TryParse(textBox_port.Text, out port))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port); //listen in any interface, initialize end point here. 
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3); 

                listening = true;
                button_listen.Enabled = false;

                //When client disconnect no problem in the server so no need to check here with try. 
                Thread acceptThread = new Thread(Accept); // Thread to accept new clients from now on. 
                acceptThread.Start();

                serverLogs.AppendText("Started listening on port: " + port + "\n");

            }
            else
            {
                serverLogs.AppendText("Please check port number \n");
            }

        }

        private void Accept() //Accepting new clients to the server. 
        {

            while (listening)
            {
                try
                {
                    Socket newClient = serverSocket.Accept(); // accept corresponding sockets for clients.

                    // Need to check username whether from database or not.
                    Thread usernameCheckThread = new Thread(() => usernameCheck(newClient));
                    usernameCheckThread.Start();
                }
                catch
                {
                    if (terminating) // If we close the server. No crash, correctly closed and not listening from now on. 
                    {
                        listening = false;
                    }
                    else //Problem occured here. 
                    {
                        serverLogs.AppendText("The socket stopped working.\n");
                    }

                }
            }

        }

        private void usernameCheck(Socket thisClient)
        {
            string message = "NOT_FOUND"; // will be used for usernames from outside the database. 
            try
            {
                Byte[] username_buffer = new Byte[64];
                thisClient.Receive(username_buffer);

                string username = Encoding.Default.GetString(username_buffer); // Convert byte array to string.
                username = username.Substring(0, username.IndexOf("\0"));
                //username = username.Trim('\0');

                if (clientusernames.Contains(username)) // if in database but already connected.
                {
                    serverLogs.AppendText(username + " has tried to connect from another client!\n");
                    message = "Already_Connected";
                }
                else
                {
                    var lines = File.ReadLines(@"../../user-db.txt"); // check the txt line by line.
                    foreach (var line in lines)
                    {
                        if (line == username) // if the db contains the username, can connect !
                        {
                            clientSockets.Add(thisClient);
                            clientusernames.Add(username);
                            message = "SUCCESS";
                            serverLogs.AppendText(username + " has connected.\n");
                            
                            //After the client is connected, Received information from the client's actions.
                            Thread receiveThread = new Thread(() => Receive(thisClient, username)); //Receive posts.
                            receiveThread.Start();
                        }
                    }

                }
                if(message=="NOT_FOUND")
                {
                    serverLogs.AppendText(username + " tried to connect to the server but cannot!\n");
                }
                try
                {
                    thisClient.Send(Encoding.Default.GetBytes(message)); //send the corresponding message to the client.
                }
                catch
                {
                    serverLogs.AppendText("There was a problem when sending the username response to the client.\n");
                }
            }

            catch
            {
                serverLogs.AppendText("Problem receiving username.\n");
            }


        }

        private void Receive(Socket thisClient, string username)//Actions from clients.
        {
            bool connected = true; //To receive information, should be connected by default.

          
            while (connected && !terminating) //still connected and not closing.
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    thisClient.Receive(buffer);//Gets information related to thisclient.

                    string incomingMessage = Encoding.Default.GetString(buffer);//convert byte array to string.
                    //incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    incomingMessage = incomingMessage.Trim('\0');

                    //string label = incomingMessage.Substring(0, 10);

                    if (incomingMessage.Substring(0, 10) == "DISCONNECT")
                    {
                        thisClient.Close();
                        clientSockets.Remove(thisClient);
                        clientusernames.Remove(username);//remove it from the connected list.
                        connected = false;
                        serverLogs.AppendText(username + " has disconnected\n");
                    }
                    else if (incomingMessage.Substring(0, 10) == "SHOW_POSTS")
                    {
                        allposts(thisClient, username); //This function will print all posts when requested. 
                    }
                    else if(incomingMessage.Substring(0, 10) == "SHOW_FRNDS")
                    {
                        friendPosts(thisClient, username);
                    }
                    else if (incomingMessage.Substring(0, 10) == "SEND_POSTS")
                    {
                        string post = incomingMessage.Substring(10);
                        postCount += 1;
                        postToLog(username, postCount, post);
                    }
                    else if (incomingMessage.Substring(0, 10) == "ADD_FRINDS")
                    {
                        string friend = incomingMessage.Substring(10);
                        addfriends(thisClient, username, friend);
                    }
                    else if (incomingMessage.Substring(0, 10) == "RMV_FRINDS")
                    {
                        string friend = incomingMessage.Substring(10);
                        removeFriends(thisClient, username, friend);
                    }
                    else if (incomingMessage.Substring(0,10) == "FRIEND_LST")
                    {
                        var friendLine = File.ReadAllLines("../../friends.txt");
                        string friendString = "SNDFRIENDS";
                        foreach (string friendPair in friendLine)
                        {
                            if (friendPair.Contains(username))
                            {
                                string[] friendList = friendPair.Split(':');
                                if (friendList[0] != username)
                                {
                                    friendString += ":" + friendList[0];
                                }
                                else if (friendList[1] != username)
                                {
                                    friendString += ":" + friendList[1];
                                }
                            }
                        }
                        Byte[] frndmsg = Encoding.Default.GetBytes(friendString);
                        thisClient.Send(frndmsg);
                    }
                    else if (incomingMessage.Substring(0, 10) == "DELETE_PST")
                    {
                        string postID = incomingMessage.Substring(10);
                        var lines = File.ReadAllLines("../../posts.log");
                        bool found = false;
                        foreach (string line in lines)
                        {
                            if (line.Contains("/" + postID))
                            {
                                found = true;
                                if (line.Contains(username))
                                {
                                    serverLogs.AppendText(username + " deleted the post with ID: " + postID + "\n");
                                    Byte[] msg = Encoding.Default.GetBytes("REMOVEPOST" + postID);
                                    thisClient.Send(msg);
                                    deletePosts(thisClient, username, postID);
                                }
                                else
                                {
                                    serverLogs.AppendText(username + " tried to delete the post with ID: " + postID + " but the post doesn't belong to them!\n");
                                    Byte[] msg = Encoding.Default.GetBytes("RMVPSTFAIL");
                                    thisClient.Send(msg);
                                }
                            }

                        }
                        if (found == false)
                        {
                            serverLogs.AppendText(username + " tried to delete the post with ID: " + postID + " but no such post exists\n");
                            Byte[] msg = Encoding.Default.GetBytes("PSTNOEXIST");
                            thisClient.Send(msg);
                        }
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        serverLogs.AppendText(username + " has disconnected.\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    clientusernames.Remove(username);
                    connected = false;
                }
            }

        }

        private void postToLog(string username, object postID, string post)
        {
            DateTime currentDateTime = DateTime.Now;
            string DT = currentDateTime.ToString("s"); // 2021-11-20T16:54:52
            using (StreamWriter file = new StreamWriter("../../posts.log", append: true))//append all posts to a file.
            {
                file.WriteLine(DT + " /" + username + "/" + postID.ToString() + "/" + post + "/");
            }
            serverLogs.AppendText(username + " has sent a post:\n" + post + "\n");
        }

        private void removeFriends(Socket thisClient, string username, string friend)
        {
            var lines = File.ReadAllLines("../../friends.txt");
            bool found = false;
            if (lines.Length != 0) { 
                foreach (string pair in lines)
                {
                    if (pair == username + ":" + friend)
                    {
                        found = true;

                        DeleteLines(pair);
                        if (found == true)
                        {
                            break;
                        }
                    }
                    else if (pair == friend + ":" + username)
                    {
                        found = true;
                        DeleteLines(pair);
                        if (found == true)
                        {
                            break;
                        }
                    }
                }
            }
            if (found == false)
            {
                serverLogs.AppendText("No such friend found to remove\n");
                Byte[] buffer = Encoding.Default.GetBytes("REMOVEFAIL");
                thisClient.Send(buffer);
            }
            else if (found == true)
            {
                serverLogs.AppendText(username + " removed " + friend + " from their friends list\n");
                Byte[] buffer = Encoding.Default.GetBytes("REMOVESCCS" + friend);
                thisClient.Send(buffer);
            }
        }

        public void DeleteLines(string strLineToDelete)
        {
            string strFilePath = "../../friends.txt";
            string strSearchText = strLineToDelete;
            string strOldText;
            string n = "";
            StreamReader sr = File.OpenText(strFilePath);
            while ((strOldText = sr.ReadLine()) != null)
            {
                if (!strOldText.Contains(strSearchText))
                {
                    n += strOldText + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText(strFilePath, n);
        }

     

        private void addfriends(Socket thisClient, string username, string friend)
        {
            if (username == friend)
            {
                serverLogs.AppendText(username + " tried to add themselves as a friend!\n");
                Byte[] buffer = Encoding.Default.GetBytes("ADDYOURSEL");
                thisClient.Send(buffer);
                return;
            }
            var lines = File.ReadAllLines("../../friends.txt");
            var users = File.ReadAllLines("../../user-db.txt");
            bool found = false;
            bool exists = false;
            foreach(string user in users)
            {
                if (friend == user)
                {
                    exists = true;
                    break;
                }
            }
            if (lines.Length != 0)
            {
                foreach (string pair in lines)
                {
                    if (pair == username + ":" + friend)
                    {
                        found = true;
                        if (found == true)
                        {
                            break;
                        }
                    }
                    else if (pair == friend + ":" + username)
                    {
                        found = true;
                        if (found == true)
                        {
                            break;
                        }
                    }
                }
            }
            if (exists)
            {
                if (found == false)
                {
                    using (StreamWriter file = new StreamWriter("../../friends.txt", append: true))
                    {
                        file.WriteLine(username + ":" + friend);
                        serverLogs.AppendText(username + " added " + friend + " as a friend\n");
                        Byte[] buffer = Encoding.Default.GetBytes("ADDEDSUCCE");
                        thisClient.Send(buffer);
                    }
                }
                else
                {
                    serverLogs.AppendText(username + " tried to add " + friend + " as a friend but already added\n");
                    Byte[] buffer = Encoding.Default.GetBytes("ALREADYADD");
                    thisClient.Send(buffer);
                }

            }
            else
            {
                serverLogs.AppendText(username + " tried to add " + friend + " as a friend but no such user exists in the database!\n");
                Byte[] buffer = Encoding.Default.GetBytes("NOTEXISTSS");
                thisClient.Send(buffer);
            }

        }

        private string getFriends(string username)
        {
            var lines = File.ReadAllLines("../../friends.txt");
            string friendList = "";
            foreach (string pair in lines)
            {
                if (pair.Contains(username))
                {
                    friendList += pair;
                }
            }
            return friendList;
        }

        private void deletePosts(Socket thisClient, string username, string postID)
        {
            string search_text = "/" + postID + "/";
            string old;
            string n = "";
            StreamReader sr = File.OpenText("../../posts.log");
            while ((old = sr.ReadLine()) != null)
            {
                if (!old.Contains(search_text))
                {
                    n += old + Environment.NewLine;
                }
            }
            sr.Close();
            File.WriteAllText("../../posts.log", n);
        }

        private void friendPosts(Socket thisClient, string username)
        {
            string allposts = File.ReadAllText(@"../../posts.log");
            string pattern = @"\d\d\d\d[-]\d\d[-]\d\d[T]\d\d[:]\d\d[:]\d\d";

            Regex regex = new Regex(pattern);
            string[] splitted = regex.Split(allposts);
            MatchCollection matches = Regex.Matches(allposts, pattern);
            string friends = getFriends(username);
            for (int i = 1; i < splitted.Length; i++)
            {
                int beforeid = splitted[i].IndexOf("/", 2);
                int afterid = splitted[i].IndexOf("/", beforeid + 1);
                string Name = splitted[i].Substring(2, beforeid - 2);
                string pID = splitted[i].Substring(beforeid + 1, afterid - beforeid - 1);
                string post = splitted[i].Substring(afterid + 1, splitted[i].Length - 4 - afterid);
                if (username == Name || friends.Contains(Name))
                {
                    try
                    {
                        Byte[] buffer1 = Encoding.Default.GetBytes("SHOW_POSTSUsername: " + Name);
                        try
                        {
                            thisClient.Send(buffer1);
                            Byte[] response = new Byte[64];
                            thisClient.Receive(response);
                            string received = Encoding.Default.GetString(response);
                            Byte[] buffer2 = Encoding.Default.GetBytes("SHOW_POSTSPostID: " + pID);
                            try
                            {
                                thisClient.Send(buffer2);
                                Byte[] response2 = new Byte[64];
                                thisClient.Receive(response);
                                string received2 = Encoding.Default.GetString(response);
                                Byte[] buffer3 = Encoding.Default.GetBytes("SHOW_POSTSPost: " + post);
                                try
                                {
                                    thisClient.Send(buffer3);
                                    Byte[] response3 = new Byte[64];
                                    thisClient.Receive(response);
                                    string received3 = Encoding.Default.GetString(response);
                                    Byte[] buffer4 = Encoding.Default.GetBytes("SHOW_POSTSTime: " + matches[i - 1] + "\n");
                                    try
                                    {
                                        thisClient.Send(buffer4);
                                        Byte[] response4 = new Byte[64];
                                        thisClient.Receive(response);
                                        string received4 = Encoding.Default.GetString(response);
                                    }
                                    catch
                                    {
                                        serverLogs.AppendText("There was a problem sending the time.\n");
                                    }
                                }
                                catch
                                {
                                    serverLogs.AppendText("There was a problem sending the post.\n");
                                }
                            }
                            catch
                            {
                                serverLogs.AppendText("There was a problem sending the post ID.\n");
                            }

                        }
                        catch
                        {
                            serverLogs.AppendText("There was a problem sending the username.\n");
                        }

                    }
                    catch
                    {
                        serverLogs.AppendText("There was a problem with the GetBytes function.\n");
                    }
                }
            }
            serverLogs.AppendText("Showed all posts for " + username + ".\n");
        }

        private void allposts(Socket thisClient, string username)
        {
            string allposts = File.ReadAllText(@"../../posts.log");
            string pattern = @"\d\d\d\d[-]\d\d[-]\d\d[T]\d\d[:]\d\d[:]\d\d";

            Regex regex = new Regex(pattern);
            string[] splitted = regex.Split(allposts);
            MatchCollection matches = Regex.Matches(allposts, pattern);
            for (int i = 1; i < splitted.Length; i++)
            {
                int beforeid = splitted[i].IndexOf("/", 2);
                int afterid = splitted[i].IndexOf("/", beforeid + 1);
                string Name = splitted[i].Substring(2, beforeid - 2);
                string pID = splitted[i].Substring(beforeid + 1, afterid - beforeid - 1);
                string post = splitted[i].Substring(afterid + 1, splitted[i].Length - 4 - afterid);
                if (username != Name)
                {
                    try
                    {
                        Byte[] buffer1 = Encoding.Default.GetBytes("SHOW_POSTSUsername: " + Name);
                        try
                        {
                            thisClient.Send(buffer1);
                            Byte[] response = new Byte[64];
                            thisClient.Receive(response);
                            string received = Encoding.Default.GetString(response);
                            Byte[] buffer2 = Encoding.Default.GetBytes("SHOW_POSTSPostID: " + pID);
                            try
                            {
                                thisClient.Send(buffer2);
                                Byte[] response2 = new Byte[64];
                                thisClient.Receive(response);
                                string received2 = Encoding.Default.GetString(response);
                                Byte[] buffer3 = Encoding.Default.GetBytes("SHOW_POSTSPost: " + post);
                                try
                                {
                                    thisClient.Send(buffer3);
                                    Byte[] response3 = new Byte[64];
                                    thisClient.Receive(response);
                                    string received3 = Encoding.Default.GetString(response);
                                    Byte[] buffer4 = Encoding.Default.GetBytes("SHOW_POSTSTime: " + matches[i - 1] + "\n");
                                    try
                                    {
                                        thisClient.Send(buffer4);
                                        Byte[] response4 = new Byte[64];
                                        thisClient.Receive(response);
                                        string received4 = Encoding.Default.GetString(response);
                                    }
                                    catch
                                    {
                                        serverLogs.AppendText("There was a problem sending the time.\n");
                                    }
                                }
                                catch
                                {
                                    serverLogs.AppendText("There was a problem sending the post.\n");
                                }
                            }
                            catch
                            {
                                serverLogs.AppendText("There was a problem sending the post ID.\n");
                            }

                        }
                        catch
                        {
                            serverLogs.AppendText("There was a problem sending the username.\n");
                        }

                    }
                    catch
                    {
                        serverLogs.AppendText("There was a problem with the GetBytes function.\n");
                    }
                }
            }
            serverLogs.AppendText("Showed all posts for " + username + ".\n");
        }


        private static int CountPost()
        {
            if (!File.Exists(@"../../posts.log"))//if not generated before.
            {
                File.Create(@"../../posts.log").Dispose();
            }

            string allPosts = File.ReadAllText(@"../../posts.log");

            if (allPosts == "")
            {
                return 0;
            }
            //maybe also line by line can be tried.
            string pattern = @"\d\d\d\d[-]\d\d[-]\d\d[T]\d\d[:]\d\d[:]\d\d";

            Regex regex = new Regex(pattern);
            string[] splitted = regex.Split(allPosts);

            int beforeID = splitted[splitted.Length - 1].IndexOf("/", 2);
            int afterID = splitted[splitted.Length - 1].IndexOf("/", beforeID + 1);

            string pID = splitted[splitted.Length - 1].Substring(beforeID + 1, afterID - beforeID - 1);

            return Int32.Parse(pID);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

      
    }
}
