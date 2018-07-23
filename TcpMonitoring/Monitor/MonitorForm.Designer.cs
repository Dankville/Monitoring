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
			this.ListViewItemColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblWaiting = new System.Windows.Forms.Label();
			this.listViewQueued = new System.Windows.Forms.ListView();
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.listViewInProgress = new System.Windows.Forms.ListView();
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblQueued = new System.Windows.Forms.Label();
			this.lblInProgress = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(16, 46);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(112, 42);
			this.btnConnect.TabIndex = 0;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// lblIpAddress
			// 
			this.lblIpAddress.AutoSize = true;
			this.lblIpAddress.Location = new System.Drawing.Point(12, 12);
			this.lblIpAddress.Name = "lblIpAddress";
			this.lblIpAddress.Size = new System.Drawing.Size(90, 20);
			this.lblIpAddress.TabIndex = 1;
			this.lblIpAddress.Text = "Ip Address:";
			// 
			// txtBoxIpAddress
			// 
			this.txtBoxIpAddress.Location = new System.Drawing.Point(108, 9);
			this.txtBoxIpAddress.Name = "txtBoxIpAddress";
			this.txtBoxIpAddress.Size = new System.Drawing.Size(80, 26);
			this.txtBoxIpAddress.TabIndex = 2;
			this.txtBoxIpAddress.Text = "127.0.0.1";
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(195, 12);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(46, 20);
			this.lblPort.TabIndex = 3;
			this.lblPort.Text = "Port: ";
			// 
			// txtBoxPort
			// 
			this.txtBoxPort.Location = new System.Drawing.Point(248, 9);
			this.txtBoxPort.Name = "txtBoxPort";
			this.txtBoxPort.Size = new System.Drawing.Size(56, 26);
			this.txtBoxPort.TabIndex = 4;
			this.txtBoxPort.Text = "9000";
			// 
			// listViewWaiting
			// 
			this.listViewWaiting.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.ListViewItemColumnHeader});
			this.listViewWaiting.GridLines = true;
			this.listViewWaiting.Location = new System.Drawing.Point(16, 128);
			this.listViewWaiting.Name = "listViewWaiting";
			this.listViewWaiting.Size = new System.Drawing.Size(300, 600);
			this.listViewWaiting.TabIndex = 5;
			this.listViewWaiting.UseCompatibleStateImageBehavior = false;
			this.listViewWaiting.View = System.Windows.Forms.View.Details;
			this.listViewWaiting.OwnerDraw = false;
			// 
			// ListViewItemColumnHeader
			// 
			this.ListViewItemColumnHeader.Text = "Queue Item";
			this.ListViewItemColumnHeader.Width = 254;
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
			// listViewQueued
			// 
			this.listViewQueued.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader2});
			this.listViewQueued.GridLines = true;
			this.listViewQueued.Location = new System.Drawing.Point(328, 128);
			this.listViewQueued.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.listViewQueued.Name = "listViewQueued";
			this.listViewQueued.Size = new System.Drawing.Size(300, 600);
			this.listViewQueued.TabIndex = 7;
			this.listViewQueued.UseCompatibleStateImageBehavior = false;
			this.listViewQueued.View = System.Windows.Forms.View.Details;
			this.listViewQueued.OwnerDraw = false;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Queue Item";
			this.columnHeader2.Width = 243;
			// 
			// listViewInProgress
			// 
			this.listViewInProgress.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader4});
			this.listViewInProgress.GridLines = true;
			this.listViewInProgress.Location = new System.Drawing.Point(635, 128);
			this.listViewInProgress.Name = "listViewInProgress";
			this.listViewInProgress.Size = new System.Drawing.Size(300, 600);
			this.listViewInProgress.TabIndex = 8;
			this.listViewInProgress.UseCompatibleStateImageBehavior = false;
			this.listViewInProgress.View = System.Windows.Forms.View.Details;
			this.listViewInProgress.OwnerDraw = false;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Queue Item";
			this.columnHeader4.Width = 227;
			// 
			// lblQueued
			// 
			this.lblQueued.AutoSize = true;
			this.lblQueued.Location = new System.Drawing.Point(324, 104);
			this.lblQueued.Name = "lblQueued";
			this.lblQueued.Size = new System.Drawing.Size(66, 20);
			this.lblQueued.TabIndex = 9;
			this.lblQueued.Text = "Queued";
			// 
			// lblInProgress
			// 
			this.lblInProgress.AutoSize = true;
			this.lblInProgress.Location = new System.Drawing.Point(631, 105);
			this.lblInProgress.Name = "lblInProgress";
			this.lblInProgress.Size = new System.Drawing.Size(89, 20);
			this.lblInProgress.TabIndex = 10;
			this.lblInProgress.Text = "In progress";
			// 
			// MonitorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(955, 728);
			this.Controls.Add(this.lblInProgress);
			this.Controls.Add(this.lblQueued);
			this.Controls.Add(this.listViewInProgress);
			this.Controls.Add(this.listViewQueued);
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
		private System.Windows.Forms.ColumnHeader ListViewItemColumnHeader;
		private System.Windows.Forms.ListView listViewQueued;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView listViewInProgress;
		private System.Windows.Forms.Label lblQueued;
		private System.Windows.Forms.Label lblInProgress;
		private System.Windows.Forms.ColumnHeader columnHeader4;
	}
}

