namespace CS408_HW1_server
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.server_logs = new System.Windows.Forms.RichTextBox();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.button_listen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // server_logs
            // 
            this.server_logs.Enabled = false;
            this.server_logs.Location = new System.Drawing.Point(30, 71);
            this.server_logs.Name = "server_logs";
            this.server_logs.Size = new System.Drawing.Size(439, 303);
            this.server_logs.TabIndex = 1;
            this.server_logs.Text = "";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(68, 32);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(134, 23);
            this.textBox_Port.TabIndex = 2;
            // 
            // button_listen
            // 
            this.button_listen.Location = new System.Drawing.Point(394, 32);
            this.button_listen.Name = "button_listen";
            this.button_listen.Size = new System.Drawing.Size(75, 23);
            this.button_listen.TabIndex = 3;
            this.button_listen.Text = "Listen";
            this.button_listen.UseVisualStyleBackColor = true;
            this.button_listen.Click += new System.EventHandler(this.button_listen_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 443);
            this.Controls.Add(this.button_listen);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.server_logs);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private RichTextBox server_logs;
        private TextBox textBox_Port;
        private Button button_listen;
    }
}