namespace _2021_SpringStep1Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.button_Disconnect = new System.Windows.Forms.Button();
            this.Button_connect = new System.Windows.Forms.Button();
            this.clientLogs = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPost = new System.Windows.Forms.TextBox();
            this.button_SendPost = new System.Windows.Forms.Button();
            this.button_AllPosts = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_friend = new System.Windows.Forms.TextBox();
            this.button_AddFriend = new System.Windows.Forms.Button();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.button_remove = new System.Windows.Forms.Button();
            this.button_DeletePost = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button_FriendPost = new System.Windows.Forms.Button();
            this.textBox_PostID = new System.Windows.Forms.TextBox();
            this.button_ShowFriends = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 99);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(75, 20);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(156, 20);
            this.textBoxIP.TabIndex = 3;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(75, 59);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(156, 20);
            this.textBoxPort.TabIndex = 4;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(75, 99);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(156, 20);
            this.textBoxUsername.TabIndex = 5;
            // 
            // button_Disconnect
            // 
            this.button_Disconnect.Enabled = false;
            this.button_Disconnect.Location = new System.Drawing.Point(269, 83);
            this.button_Disconnect.Margin = new System.Windows.Forms.Padding(2);
            this.button_Disconnect.Name = "button_Disconnect";
            this.button_Disconnect.Size = new System.Drawing.Size(76, 32);
            this.button_Disconnect.TabIndex = 7;
            this.button_Disconnect.Text = "Disconnect";
            this.button_Disconnect.UseVisualStyleBackColor = true;
            this.button_Disconnect.Click += new System.EventHandler(this.button_Disconnect_Click);
            // 
            // Button_connect
            // 
            this.Button_connect.Location = new System.Drawing.Point(269, 20);
            this.Button_connect.Margin = new System.Windows.Forms.Padding(2);
            this.Button_connect.Name = "Button_connect";
            this.Button_connect.Size = new System.Drawing.Size(76, 32);
            this.Button_connect.TabIndex = 8;
            this.Button_connect.Text = "Connect";
            this.Button_connect.UseVisualStyleBackColor = true;
            this.Button_connect.Click += new System.EventHandler(this.Button_connect_Click);
            // 
            // clientLogs
            // 
            this.clientLogs.Location = new System.Drawing.Point(376, 20);
            this.clientLogs.Margin = new System.Windows.Forms.Padding(2);
            this.clientLogs.Name = "clientLogs";
            this.clientLogs.ReadOnly = true;
            this.clientLogs.Size = new System.Drawing.Size(262, 367);
            this.clientLogs.TabIndex = 9;
            this.clientLogs.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 142);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Post:";
            // 
            // textBoxPost
            // 
            this.textBoxPost.Enabled = false;
            this.textBoxPost.Location = new System.Drawing.Point(75, 139);
            this.textBoxPost.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPost.Name = "textBoxPost";
            this.textBoxPost.Size = new System.Drawing.Size(156, 20);
            this.textBoxPost.TabIndex = 11;
            // 
            // button_SendPost
            // 
            this.button_SendPost.Enabled = false;
            this.button_SendPost.Location = new System.Drawing.Point(268, 134);
            this.button_SendPost.Margin = new System.Windows.Forms.Padding(2);
            this.button_SendPost.Name = "button_SendPost";
            this.button_SendPost.Size = new System.Drawing.Size(75, 29);
            this.button_SendPost.TabIndex = 12;
            this.button_SendPost.Text = "Send";
            this.button_SendPost.UseVisualStyleBackColor = true;
            this.button_SendPost.Click += new System.EventHandler(this.button_SendPost_Click);
            // 
            // button_AllPosts
            // 
            this.button_AllPosts.Enabled = false;
            this.button_AllPosts.Location = new System.Drawing.Point(376, 411);
            this.button_AllPosts.Margin = new System.Windows.Forms.Padding(2);
            this.button_AllPosts.Name = "button_AllPosts";
            this.button_AllPosts.Size = new System.Drawing.Size(75, 29);
            this.button_AllPosts.TabIndex = 13;
            this.button_AllPosts.Text = "All Posts";
            this.button_AllPosts.UseVisualStyleBackColor = true;
            this.button_AllPosts.Click += new System.EventHandler(this.button_AllPosts_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Username:";
            // 
            // textBox_friend
            // 
            this.textBox_friend.Enabled = false;
            this.textBox_friend.Location = new System.Drawing.Point(75, 240);
            this.textBox_friend.Name = "textBox_friend";
            this.textBox_friend.Size = new System.Drawing.Size(156, 20);
            this.textBox_friend.TabIndex = 15;
            // 
            // button_AddFriend
            // 
            this.button_AddFriend.Enabled = false;
            this.button_AddFriend.Location = new System.Drawing.Point(266, 236);
            this.button_AddFriend.Name = "button_AddFriend";
            this.button_AddFriend.Size = new System.Drawing.Size(76, 27);
            this.button_AddFriend.TabIndex = 16;
            this.button_AddFriend.Text = "Add friend";
            this.button_AddFriend.UseVisualStyleBackColor = true;
            this.button_AddFriend.Click += new System.EventHandler(this.button_AddFriend_Click);
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // button_remove
            // 
            this.button_remove.Enabled = false;
            this.button_remove.Location = new System.Drawing.Point(266, 279);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(76, 45);
            this.button_remove.TabIndex = 17;
            this.button_remove.Text = "Remove Friend";
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // button_DeletePost
            // 
            this.button_DeletePost.Enabled = false;
            this.button_DeletePost.Location = new System.Drawing.Point(266, 178);
            this.button_DeletePost.Margin = new System.Windows.Forms.Padding(2);
            this.button_DeletePost.Name = "button_DeletePost";
            this.button_DeletePost.Size = new System.Drawing.Size(77, 29);
            this.button_DeletePost.TabIndex = 18;
            this.button_DeletePost.Text = "Delete Post";
            this.button_DeletePost.UseVisualStyleBackColor = true;
            this.button_DeletePost.Click += new System.EventHandler(this.button_DeletePost_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 186);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "PostID:";
            // 
            // button_FriendPost
            // 
            this.button_FriendPost.Enabled = false;
            this.button_FriendPost.Location = new System.Drawing.Point(563, 407);
            this.button_FriendPost.Margin = new System.Windows.Forms.Padding(2);
            this.button_FriendPost.Name = "button_FriendPost";
            this.button_FriendPost.Size = new System.Drawing.Size(75, 38);
            this.button_FriendPost.TabIndex = 20;
            this.button_FriendPost.Text = "Friend\'s Posts";
            this.button_FriendPost.UseVisualStyleBackColor = true;
            this.button_FriendPost.Click += new System.EventHandler(this.button_FriendPost_Click);
            // 
            // textBox_PostID
            // 
            this.textBox_PostID.Enabled = false;
            this.textBox_PostID.Location = new System.Drawing.Point(75, 183);
            this.textBox_PostID.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_PostID.Name = "textBox_PostID";
            this.textBox_PostID.Size = new System.Drawing.Size(156, 20);
            this.textBox_PostID.TabIndex = 21;
            // 
            // button_ShowFriends
            // 
            this.button_ShowFriends.Enabled = false;
            this.button_ShowFriends.Location = new System.Drawing.Point(155, 279);
            this.button_ShowFriends.Name = "button_ShowFriends";
            this.button_ShowFriends.Size = new System.Drawing.Size(76, 45);
            this.button_ShowFriends.TabIndex = 23;
            this.button_ShowFriends.Text = "Show Friends";
            this.button_ShowFriends.UseVisualStyleBackColor = true;
            this.button_ShowFriends.Click += new System.EventHandler(this.button_ShowFriends_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 519);
            this.Controls.Add(this.button_ShowFriends);
            this.Controls.Add(this.textBox_PostID);
            this.Controls.Add(this.button_FriendPost);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_DeletePost);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.button_AddFriend);
            this.Controls.Add(this.textBox_friend);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_AllPosts);
            this.Controls.Add(this.button_SendPost);
            this.Controls.Add(this.textBoxPost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.clientLogs);
            this.Controls.Add(this.Button_connect);
            this.Controls.Add(this.button_Disconnect);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Button button_Disconnect;
        private System.Windows.Forms.Button Button_connect;
        private System.Windows.Forms.RichTextBox clientLogs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPost;
        private System.Windows.Forms.Button button_SendPost;
        private System.Windows.Forms.Button button_AllPosts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_friend;
        private System.Windows.Forms.Button button_AddFriend;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.Button button_remove;
        private System.Windows.Forms.Button button_DeletePost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_PostID;
        private System.Windows.Forms.Button button_FriendPost;
        private System.Windows.Forms.Button button_ShowFriends;
    }
}

