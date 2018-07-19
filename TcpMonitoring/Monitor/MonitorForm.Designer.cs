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
			this.ListViewIDColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ListViewItemColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblWaiting = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(11, 30);
			this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 27);
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// lblIpAddress
			// 
			this.lblIpAddress.AutoSize = true;
			this.lblIpAddress.Location = new System.Drawing.Point(8, 8);
			this.lblIpAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblIpAddress.Name = "lblIpAddress";
			this.lblIpAddress.Size = new System.Drawing.Size(60, 13);
			this.lblIpAddress.TabIndex = 1;
			this.lblIpAddress.Text = "Ip Address:";
			// 
			// txtBoxIpAddress
			// 
			this.txtBoxIpAddress.Location = new System.Drawing.Point(72, 6);
			this.txtBoxIpAddress.Margin = new System.Windows.Forms.Padding(2);
			this.txtBoxIpAddress.Name = "txtBoxIpAddress";
			this.txtBoxIpAddress.Size = new System.Drawing.Size(55, 20);
			this.txtBoxIpAddress.TabIndex = 2;
			this.txtBoxIpAddress.Text = "127.0.0.1";
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(130, 8);
			this.lblPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(32, 13);
			this.lblPort.TabIndex = 3;
			this.lblPort.Text = "Port: ";
			// 
			// txtBoxPort
			// 
			this.txtBoxPort.Location = new System.Drawing.Point(165, 6);
			this.txtBoxPort.Margin = new System.Windows.Forms.Padding(2);
			this.txtBoxPort.Name = "txtBoxPort";
			this.txtBoxPort.Size = new System.Drawing.Size(39, 20);
			this.txtBoxPort.TabIndex = 4;
			this.txtBoxPort.Text = "9000";
			// 
			// listViewWaiting
			// 
			this.listViewWaiting.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.ListViewIDColumnHeader,
			this.ListViewItemColumnHeader});
			this.listViewWaiting.GridLines = true;
			this.listViewWaiting.Location = new System.Drawing.Point(11, 83);
			this.listViewWaiting.Margin = new System.Windows.Forms.Padding(2);
			this.listViewWaiting.Name = "listViewWaiting";
			this.listViewWaiting.Size = new System.Drawing.Size(200, 376);
			this.listViewWaiting.TabIndex = 5;
			this.listViewWaiting.UseCompatibleStateImageBehavior = false;
			this.listViewWaiting.View = System.Windows.Forms.View.Details;
			// 
			// ListViewIDColumnHeader
			// 
			this.ListViewIDColumnHeader.Text = "ID";
			this.ListViewIDColumnHeader.Width = 30;
			// 
			// ListViewItemColumnHeader
			// 
			this.ListViewItemColumnHeader.Text = "Queue Item";
			this.ListViewItemColumnHeader.Width = 170;
			// 
			// lblWaiting
			// 
			this.lblWaiting.AutoSize = true;
			this.lblWaiting.Location = new System.Drawing.Point(11, 68);
			this.lblWaiting.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblWaiting.Name = "lblWaiting";
			this.lblWaiting.Size = new System.Drawing.Size(43, 13);
			this.lblWaiting.TabIndex = 6;
			this.lblWaiting.Text = "Waiting";
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader1,
			this.columnHeader2});
			this.listView1.GridLines = true;
			this.listView1.Location = new System.Drawing.Point(216, 83);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(200, 376);
			this.listView1.TabIndex = 7;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "ID";
			this.columnHeader1.Width = 30;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Queue Item";
			this.columnHeader2.Width = 170;
			// 
			// MonitorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(830, 473);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.lblWaiting);
			this.Controls.Add(this.listViewWaiting);
			this.Controls.Add(this.txtBoxPort);
			this.Controls.Add(this.lblPort);
			this.Controls.Add(this.txtBoxIpAddress);
			this.Controls.Add(this.lblIpAddress);
			this.Controls.Add(this.btnConnect);
			this.Margin = new System.Windows.Forms.Padding(2);
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
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
	}
}

