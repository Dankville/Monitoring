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
			this.listViewWaiting = new System.Windows.Forms.ListView();
			this.ListViewItemColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblWaiting = new System.Windows.Forms.Label();
			this.listViewQueued = new System.Windows.Forms.ListView();
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.listViewInProgress = new System.Windows.Forms.ListView();
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblQueued = new System.Windows.Forms.Label();
			this.lblInProgress = new System.Windows.Forms.Label();
			this.lblAlive = new System.Windows.Forms.Label();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.lblWaitingAmount = new System.Windows.Forms.Label();
			this.lblQueuedAmount = new System.Windows.Forms.Label();
			this.lblInProgressAmount = new System.Windows.Forms.Label();
			this.lblTotal = new System.Windows.Forms.Label();
			this.lblTotalAmount = new System.Windows.Forms.Label();
			this.lblUsername = new System.Windows.Forms.Label();
			this.txtBoxUsername = new System.Windows.Forms.TextBox();
			this.lblPasswd = new System.Windows.Forms.Label();
			this.txtPasswd = new System.Windows.Forms.TextBox();
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
			// listViewWaiting
			// 
			this.listViewWaiting.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ListViewItemColumnHeader});
			this.listViewWaiting.GridLines = true;
			this.listViewWaiting.Location = new System.Drawing.Point(13, 83);
			this.listViewWaiting.Margin = new System.Windows.Forms.Padding(2);
			this.listViewWaiting.Name = "listViewWaiting";
			this.listViewWaiting.Size = new System.Drawing.Size(201, 744);
			this.listViewWaiting.TabIndex = 5;
			this.listViewWaiting.UseCompatibleStateImageBehavior = false;
			this.listViewWaiting.View = System.Windows.Forms.View.Details;
			// 
			// ListViewItemColumnHeader
			// 
			this.ListViewItemColumnHeader.Text = "Queue Item";
			this.ListViewItemColumnHeader.Width = 254;
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
			// listViewQueued
			// 
			this.listViewQueued.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
			this.listViewQueued.GridLines = true;
			this.listViewQueued.Location = new System.Drawing.Point(219, 83);
			this.listViewQueued.Name = "listViewQueued";
			this.listViewQueued.Size = new System.Drawing.Size(201, 743);
			this.listViewQueued.TabIndex = 7;
			this.listViewQueued.UseCompatibleStateImageBehavior = false;
			this.listViewQueued.View = System.Windows.Forms.View.Details;
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
			this.listViewInProgress.Location = new System.Drawing.Point(423, 83);
			this.listViewInProgress.Margin = new System.Windows.Forms.Padding(2);
			this.listViewInProgress.Name = "listViewInProgress";
			this.listViewInProgress.Size = new System.Drawing.Size(201, 743);
			this.listViewInProgress.TabIndex = 8;
			this.listViewInProgress.UseCompatibleStateImageBehavior = false;
			this.listViewInProgress.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Queue Item";
			this.columnHeader4.Width = 227;
			// 
			// lblQueued
			// 
			this.lblQueued.AutoSize = true;
			this.lblQueued.Location = new System.Drawing.Point(216, 68);
			this.lblQueued.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblQueued.Name = "lblQueued";
			this.lblQueued.Size = new System.Drawing.Size(45, 13);
			this.lblQueued.TabIndex = 9;
			this.lblQueued.Text = "Queued";
			// 
			// lblInProgress
			// 
			this.lblInProgress.AutoSize = true;
			this.lblInProgress.Location = new System.Drawing.Point(421, 68);
			this.lblInProgress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblInProgress.Name = "lblInProgress";
			this.lblInProgress.Size = new System.Drawing.Size(59, 13);
			this.lblInProgress.TabIndex = 10;
			this.lblInProgress.Text = "In progress";
			// 
			// lblAlive
			// 
			this.lblAlive.AutoSize = true;
			this.lblAlive.ForeColor = System.Drawing.Color.Red;
			this.lblAlive.Location = new System.Drawing.Point(551, 8);
			this.lblAlive.Name = "lblAlive";
			this.lblAlive.Size = new System.Drawing.Size(73, 13);
			this.lblAlive.TabIndex = 11;
			this.lblAlive.Text = "Disconnected";
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Location = new System.Drawing.Point(91, 30);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(75, 27);
			this.btnDisconnect.TabIndex = 12;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// lblWaitingAmount
			// 
			this.lblWaitingAmount.AutoSize = true;
			this.lblWaitingAmount.Location = new System.Drawing.Point(191, 68);
			this.lblWaitingAmount.Name = "lblWaitingAmount";
			this.lblWaitingAmount.Size = new System.Drawing.Size(13, 13);
			this.lblWaitingAmount.TabIndex = 13;
			this.lblWaitingAmount.Text = "0";
			// 
			// lblQueuedAmount
			// 
			this.lblQueuedAmount.AutoSize = true;
			this.lblQueuedAmount.Location = new System.Drawing.Point(393, 68);
			this.lblQueuedAmount.Name = "lblQueuedAmount";
			this.lblQueuedAmount.Size = new System.Drawing.Size(13, 13);
			this.lblQueuedAmount.TabIndex = 14;
			this.lblQueuedAmount.Text = "0";
			// 
			// lblInProgressAmount
			// 
			this.lblInProgressAmount.AutoSize = true;
			this.lblInProgressAmount.Location = new System.Drawing.Point(602, 68);
			this.lblInProgressAmount.Name = "lblInProgressAmount";
			this.lblInProgressAmount.Size = new System.Drawing.Size(13, 13);
			this.lblInProgressAmount.TabIndex = 15;
			this.lblInProgressAmount.Text = "0";
			// 
			// lblTotal
			// 
			this.lblTotal.AutoSize = true;
			this.lblTotal.Location = new System.Drawing.Point(521, 30);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Size = new System.Drawing.Size(62, 13);
			this.lblTotal.TabIndex = 16;
			this.lblTotal.Text = "Total Items:";
			// 
			// lblTotalAmount
			// 
			this.lblTotalAmount.AutoSize = true;
			this.lblTotalAmount.Location = new System.Drawing.Point(602, 30);
			this.lblTotalAmount.Name = "lblTotalAmount";
			this.lblTotalAmount.Size = new System.Drawing.Size(13, 13);
			this.lblTotalAmount.TabIndex = 17;
			this.lblTotalAmount.Text = "0";
			// 
			// lblUsername
			// 
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(11, 8);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(58, 13);
			this.lblUsername.TabIndex = 18;
			this.lblUsername.Text = "Username:";
			// 
			// txtBoxUsername
			// 
			this.txtBoxUsername.Location = new System.Drawing.Point(75, 5);
			this.txtBoxUsername.Name = "txtBoxUsername";
			this.txtBoxUsername.Size = new System.Drawing.Size(100, 20);
			this.txtBoxUsername.TabIndex = 19;
			this.txtBoxUsername.Text = "@@rootadmin";
			// 
			// lblPasswd
			// 
			this.lblPasswd.AutoSize = true;
			this.lblPasswd.Location = new System.Drawing.Point(191, 8);
			this.lblPasswd.Name = "lblPasswd";
			this.lblPasswd.Size = new System.Drawing.Size(56, 13);
			this.lblPasswd.TabIndex = 20;
			this.lblPasswd.Text = "Password:";
			// 
			// txtPasswd
			// 
			this.txtPasswd.Location = new System.Drawing.Point(253, 5);
			this.txtPasswd.Name = "txtPasswd";
			this.txtPasswd.PasswordChar = '*';
			this.txtPasswd.Size = new System.Drawing.Size(100, 20);
			this.txtPasswd.TabIndex = 21;
			this.txtPasswd.Text = "geheim";
			// 
			// MonitorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(644, 838);
			this.Controls.Add(this.txtPasswd);
			this.Controls.Add(this.lblPasswd);
			this.Controls.Add(this.txtBoxUsername);
			this.Controls.Add(this.lblUsername);
			this.Controls.Add(this.lblTotalAmount);
			this.Controls.Add(this.lblTotal);
			this.Controls.Add(this.lblInProgressAmount);
			this.Controls.Add(this.lblQueuedAmount);
			this.Controls.Add(this.lblWaitingAmount);
			this.Controls.Add(this.btnDisconnect);
			this.Controls.Add(this.lblAlive);
			this.Controls.Add(this.lblInProgress);
			this.Controls.Add(this.lblQueued);
			this.Controls.Add(this.listViewInProgress);
			this.Controls.Add(this.listViewQueued);
			this.Controls.Add(this.lblWaiting);
			this.Controls.Add(this.listViewWaiting);
			this.Controls.Add(this.btnConnect);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "MonitorForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.ListView listViewWaiting;
		private System.Windows.Forms.Label lblWaiting;
		private System.Windows.Forms.ColumnHeader ListViewItemColumnHeader;
		private System.Windows.Forms.ListView listViewQueued;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView listViewInProgress;
		private System.Windows.Forms.Label lblQueued;
		private System.Windows.Forms.Label lblInProgress;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Label lblAlive;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Label lblWaitingAmount;
		private System.Windows.Forms.Label lblQueuedAmount;
		private System.Windows.Forms.Label lblInProgressAmount;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.Label lblTotalAmount;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.TextBox txtBoxUsername;
		private System.Windows.Forms.Label lblPasswd;
		private System.Windows.Forms.TextBox txtPasswd;
	}
}

