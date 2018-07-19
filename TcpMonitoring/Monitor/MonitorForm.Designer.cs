namespace Monitor
{
	partial class MonitorForm
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
			this.btnConnect = new System.Windows.Forms.Button();
			this.lblIpAddress = new System.Windows.Forms.Label();
			this.txtBoxIpAddress = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtBoxPort = new System.Windows.Forms.TextBox();
			this.listViewWaiting = new System.Windows.Forms.ListView();
			this.lblWaiting = new System.Windows.Forms.Label();
			this.ListViewIDColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ListViewItemColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(16, 42);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(113, 41);
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// lblIpAddress
			// 
			this.lblIpAddress.AutoSize = true;
			this.lblIpAddress.Location = new System.Drawing.Point(12, 13);
			this.lblIpAddress.Name = "lblIpAddress";
			this.lblIpAddress.Size = new System.Drawing.Size(90, 20);
			this.lblIpAddress.TabIndex = 1;
			this.lblIpAddress.Text = "Ip Address:";
			// 
			// txtBoxIpAddress
			// 
			this.txtBoxIpAddress.Location = new System.Drawing.Point(108, 10);
			this.txtBoxIpAddress.Name = "txtBoxIpAddress";
			this.txtBoxIpAddress.Size = new System.Drawing.Size(81, 26);
			this.txtBoxIpAddress.TabIndex = 2;
			this.txtBoxIpAddress.Text = "127.0.0.1";
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(195, 13);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(46, 20);
			this.lblPort.TabIndex = 3;
			this.lblPort.Text = "Port: ";
			// 
			// txtBoxPort
			// 
			this.txtBoxPort.Location = new System.Drawing.Point(247, 10);
			this.txtBoxPort.Name = "txtBoxPort";
			this.txtBoxPort.Size = new System.Drawing.Size(57, 26);
			this.txtBoxPort.TabIndex = 4;
			this.txtBoxPort.Text = "9000";
			// 
			// listViewWaiting
			// 
			this.listViewWaiting.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.ListViewIDColumnHeader,
			this.ListViewItemColumnHeader});
			this.listViewWaiting.Location = new System.Drawing.Point(16, 131);
			this.listViewWaiting.Name = "listViewWaiting";
			this.listViewWaiting.Size = new System.Drawing.Size(288, 577);
			this.listViewWaiting.TabIndex = 5;
			this.listViewWaiting.UseCompatibleStateImageBehavior = false;
			// 
			// lblWaiting
			// 
			this.lblWaiting.AutoSize = true;
			this.lblWaiting.Location = new System.Drawing.Point(16, 105);
			this.lblWaiting.Name = "lblWaiting";
			this.lblWaiting.Size = new System.Drawing.Size(62, 20);
			this.lblWaiting.TabIndex = 6;
			this.lblWaiting.Text = "Waiting";
			// 
			// ListViewIDColumnHeader
			// 
			this.ListViewIDColumnHeader.Text = "ID";
			this.ListViewIDColumnHeader.Width = 10;
			// 
			// ListViewItemColumnHeader
			// 
			this.ListViewItemColumnHeader.Width = 50;
			// 
			// MonitorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(831, 720);
			this.Controls.Add(this.lblWaiting);
			this.Controls.Add(this.listViewWaiting);
			this.Controls.Add(this.txtBoxPort);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.txtBoxIpAddress);
			this.Controls.Add(this.lblIpAddress);
			this.Controls.Add(this.btnConnect);
			this.Name = "MonitorForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Label lblIpAddress;
		private System.Windows.Forms.TextBox txtBoxIpAddress;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtBoxPort;
		private System.Windows.Forms.ListView listViewWaiting;
		private System.Windows.Forms.Label lblWaiting;
		private System.Windows.Forms.ColumnHeader ListViewIDColumnHeader;
		private System.Windows.Forms.ColumnHeader ListViewItemColumnHeader;
	}
}

