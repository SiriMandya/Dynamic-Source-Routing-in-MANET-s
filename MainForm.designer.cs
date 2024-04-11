namespace TS_AOMDV
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabLog = new System.Windows.Forms.TabControl();
            this.tabDataLog = new System.Windows.Forms.TabPage();
            this.txtFileType = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.txtDestinationIp = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lviewDataLog = new System.Windows.Forms.ListView();
            this.chServiceId3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIPFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIPTo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabServiceLog = new System.Windows.Forms.TabPage();
            this.lviewServiceLog = new System.Windows.Forms.ListView();
            this.chServiceId1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSourceIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDestinationIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabRouteLog = new System.Windows.Forms.TabPage();
            this.lviewRouteLog = new System.Windows.Forms.ListView();
            this.chServiceId2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRouteInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabSniferLog = new System.Windows.Forms.TabPage();
            this.lboxTcpAckLog = new System.Windows.Forms.ListBox();
            this.tviewSniferLog = new System.Windows.Forms.TreeView();
            this.chkDropDataPacket = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnFindNeighbor = new System.Windows.Forms.Button();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.txtSystemIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lviewNeighborLog = new System.Windows.Forms.ListView();
            this.chNodeIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSourceTrust = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRouterTrust = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRREQCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSentCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chReceivedCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabLog.SuspendLayout();
            this.tabDataLog.SuspendLayout();
            this.tabServiceLog.SuspendLayout();
            this.tabRouteLog.SuspendLayout();
            this.tabSniferLog.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabLog
            // 
            this.tabLog.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabLog.Controls.Add(this.tabDataLog);
            this.tabLog.Controls.Add(this.tabServiceLog);
            this.tabLog.Controls.Add(this.tabRouteLog);
            this.tabLog.Controls.Add(this.tabSniferLog);
            this.tabLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabLog.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabLog.Location = new System.Drawing.Point(0, 165);
            this.tabLog.Margin = new System.Windows.Forms.Padding(2);
            this.tabLog.Name = "tabLog";
            this.tabLog.SelectedIndex = 0;
            this.tabLog.Size = new System.Drawing.Size(839, 329);
            this.tabLog.TabIndex = 11;
            // 
            // tabDataLog
            // 
            this.tabDataLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabDataLog.Controls.Add(this.txtFileType);
            this.tabDataLog.Controls.Add(this.label5);
            this.tabDataLog.Controls.Add(this.txtFileName);
            this.tabDataLog.Controls.Add(this.label4);
            this.tabDataLog.Controls.Add(this.txtFileSize);
            this.tabDataLog.Controls.Add(this.label2);
            this.tabDataLog.Controls.Add(this.txtFilePath);
            this.tabDataLog.Controls.Add(this.label1);
            this.tabDataLog.Controls.Add(this.btnSendFile);
            this.tabDataLog.Controls.Add(this.txtDestinationIp);
            this.tabDataLog.Controls.Add(this.label6);
            this.tabDataLog.Controls.Add(this.btnBrowse);
            this.tabDataLog.Controls.Add(this.lviewDataLog);
            this.tabDataLog.Location = new System.Drawing.Point(4, 30);
            this.tabDataLog.Name = "tabDataLog";
            this.tabDataLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataLog.Size = new System.Drawing.Size(831, 295);
            this.tabDataLog.TabIndex = 5;
            this.tabDataLog.Text = "Data Log";
            this.tabDataLog.UseVisualStyleBackColor = true;
            // 
            // txtFileType
            // 
            this.txtFileType.Location = new System.Drawing.Point(290, 74);
            this.txtFileType.Name = "txtFileType";
            this.txtFileType.ReadOnly = true;
            this.txtFileType.Size = new System.Drawing.Size(112, 26);
            this.txtFileType.TabIndex = 76;
            this.txtFileType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(216, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 19);
            this.label5.TabIndex = 75;
            this.label5.Text = "File Type:";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(98, 45);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(304, 26);
            this.txtFileName.TabIndex = 74;
            this.txtFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 19);
            this.label4.TabIndex = 73;
            this.label4.Text = "File Name:";
            // 
            // txtFileSize
            // 
            this.txtFileSize.Location = new System.Drawing.Point(98, 74);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.ReadOnly = true;
            this.txtFileSize.Size = new System.Drawing.Size(112, 26);
            this.txtFileSize.TabIndex = 74;
            this.txtFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 19);
            this.label2.TabIndex = 73;
            this.label2.Text = "File Size:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(98, 16);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(419, 26);
            this.txtFilePath.TabIndex = 72;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 19);
            this.label1.TabIndex = 71;
            this.label1.Text = "File Path:";
            // 
            // btnSendFile
            // 
            this.btnSendFile.Font = new System.Drawing.Font("Calibri", 12F);
            this.btnSendFile.ForeColor = System.Drawing.Color.Black;
            this.btnSendFile.Location = new System.Drawing.Point(714, 69);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(100, 33);
            this.btnSendFile.TabIndex = 69;
            this.btnSendFile.Text = "Send File";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // txtDestinationIp
            // 
            this.txtDestinationIp.Font = new System.Drawing.Font("Calibri", 12F);
            this.txtDestinationIp.ForeColor = System.Drawing.Color.Black;
            this.txtDestinationIp.Location = new System.Drawing.Point(562, 73);
            this.txtDestinationIp.Name = "txtDestinationIp";
            this.txtDestinationIp.Size = new System.Drawing.Size(149, 27);
            this.txtDestinationIp.TabIndex = 68;
            this.txtDestinationIp.Text = "192.168.42.133";
            this.txtDestinationIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(408, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 19);
            this.label6.TabIndex = 70;
            this.label6.Text = "Destination Address:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.ForeColor = System.Drawing.Color.Black;
            this.btnBrowse.Location = new System.Drawing.Point(520, 11);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(92, 34);
            this.btnBrowse.TabIndex = 67;
            this.btnBrowse.Text = "Browse..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lviewDataLog
            // 
            this.lviewDataLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chServiceId3,
            this.chFileName,
            this.chIPFrom,
            this.chIPTo});
            this.lviewDataLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lviewDataLog.FullRowSelect = true;
            this.lviewDataLog.GridLines = true;
            this.lviewDataLog.Location = new System.Drawing.Point(3, 105);
            this.lviewDataLog.Margin = new System.Windows.Forms.Padding(0);
            this.lviewDataLog.Name = "lviewDataLog";
            this.lviewDataLog.Size = new System.Drawing.Size(823, 185);
            this.lviewDataLog.TabIndex = 3;
            this.lviewDataLog.UseCompatibleStateImageBehavior = false;
            this.lviewDataLog.View = System.Windows.Forms.View.Details;
            // 
            // chServiceId3
            // 
            this.chServiceId3.Text = "ServiceId";
            this.chServiceId3.Width = 86;
            // 
            // chFileName
            // 
            this.chFileName.Text = "FileName";
            this.chFileName.Width = 297;
            // 
            // chIPFrom
            // 
            this.chIPFrom.Text = "IPFrom";
            this.chIPFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIPFrom.Width = 145;
            // 
            // chIPTo
            // 
            this.chIPTo.Text = "IPTo";
            this.chIPTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIPTo.Width = 145;
            // 
            // tabServiceLog
            // 
            this.tabServiceLog.Controls.Add(this.lviewServiceLog);
            this.tabServiceLog.Location = new System.Drawing.Point(4, 30);
            this.tabServiceLog.Name = "tabServiceLog";
            this.tabServiceLog.Size = new System.Drawing.Size(831, 295);
            this.tabServiceLog.TabIndex = 2;
            this.tabServiceLog.Text = "Service Log";
            this.tabServiceLog.UseVisualStyleBackColor = true;
            // 
            // lviewServiceLog
            // 
            this.lviewServiceLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chServiceId1,
            this.chSourceIp,
            this.chDestinationIp});
            this.lviewServiceLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lviewServiceLog.FullRowSelect = true;
            this.lviewServiceLog.GridLines = true;
            this.lviewServiceLog.Location = new System.Drawing.Point(0, 0);
            this.lviewServiceLog.Margin = new System.Windows.Forms.Padding(0);
            this.lviewServiceLog.Name = "lviewServiceLog";
            this.lviewServiceLog.Size = new System.Drawing.Size(831, 295);
            this.lviewServiceLog.TabIndex = 3;
            this.lviewServiceLog.UseCompatibleStateImageBehavior = false;
            this.lviewServiceLog.View = System.Windows.Forms.View.Details;
            // 
            // chServiceId1
            // 
            this.chServiceId1.Text = "ServiceId";
            this.chServiceId1.Width = 75;
            // 
            // chSourceIp
            // 
            this.chSourceIp.Text = "SourceIp";
            this.chSourceIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSourceIp.Width = 200;
            // 
            // chDestinationIp
            // 
            this.chDestinationIp.Text = "DestinationIp";
            this.chDestinationIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chDestinationIp.Width = 200;
            // 
            // tabRouteLog
            // 
            this.tabRouteLog.Controls.Add(this.lviewRouteLog);
            this.tabRouteLog.Location = new System.Drawing.Point(4, 30);
            this.tabRouteLog.Name = "tabRouteLog";
            this.tabRouteLog.Size = new System.Drawing.Size(831, 295);
            this.tabRouteLog.TabIndex = 3;
            this.tabRouteLog.Text = "Route Log";
            this.tabRouteLog.UseVisualStyleBackColor = true;
            // 
            // lviewRouteLog
            // 
            this.lviewRouteLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chServiceId2,
            this.chRouteInfo});
            this.lviewRouteLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lviewRouteLog.FullRowSelect = true;
            this.lviewRouteLog.GridLines = true;
            this.lviewRouteLog.Location = new System.Drawing.Point(0, 0);
            this.lviewRouteLog.Margin = new System.Windows.Forms.Padding(0);
            this.lviewRouteLog.Name = "lviewRouteLog";
            this.lviewRouteLog.Size = new System.Drawing.Size(831, 295);
            this.lviewRouteLog.TabIndex = 4;
            this.lviewRouteLog.UseCompatibleStateImageBehavior = false;
            this.lviewRouteLog.View = System.Windows.Forms.View.Details;
            // 
            // chServiceId2
            // 
            this.chServiceId2.Text = "ServiceId";
            this.chServiceId2.Width = 75;
            // 
            // chRouteInfo
            // 
            this.chRouteInfo.Text = "RouteInfo";
            this.chRouteInfo.Width = 722;
            // 
            // tabSniferLog
            // 
            this.tabSniferLog.Controls.Add(this.lboxTcpAckLog);
            this.tabSniferLog.Controls.Add(this.tviewSniferLog);
            this.tabSniferLog.Location = new System.Drawing.Point(4, 30);
            this.tabSniferLog.Name = "tabSniferLog";
            this.tabSniferLog.Size = new System.Drawing.Size(831, 295);
            this.tabSniferLog.TabIndex = 4;
            this.tabSniferLog.Text = "Snifer Log";
            this.tabSniferLog.UseVisualStyleBackColor = true;
            // 
            // lboxTcpAckLog
            // 
            this.lboxTcpAckLog.BackColor = System.Drawing.SystemColors.Control;
            this.lboxTcpAckLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lboxTcpAckLog.Font = new System.Drawing.Font("Calibri", 9F);
            this.lboxTcpAckLog.FormattingEnabled = true;
            this.lboxTcpAckLog.ItemHeight = 14;
            this.lboxTcpAckLog.Location = new System.Drawing.Point(0, 0);
            this.lboxTcpAckLog.Name = "lboxTcpAckLog";
            this.lboxTcpAckLog.Size = new System.Drawing.Size(477, 295);
            this.lboxTcpAckLog.TabIndex = 16;
            // 
            // tviewSniferLog
            // 
            this.tviewSniferLog.BackColor = System.Drawing.Color.White;
            this.tviewSniferLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.tviewSniferLog.Location = new System.Drawing.Point(477, 0);
            this.tviewSniferLog.Name = "tviewSniferLog";
            this.tviewSniferLog.Size = new System.Drawing.Size(354, 295);
            this.tviewSniferLog.TabIndex = 6;
            // 
            // chkDropDataPacket
            // 
            this.chkDropDataPacket.AutoSize = true;
            this.chkDropDataPacket.BackColor = System.Drawing.Color.LightSalmon;
            this.chkDropDataPacket.Font = new System.Drawing.Font("Calibri", 13F);
            this.chkDropDataPacket.ForeColor = System.Drawing.Color.Black;
            this.chkDropDataPacket.Location = new System.Drawing.Point(679, 167);
            this.chkDropDataPacket.Name = "chkDropDataPacket";
            this.chkDropDataPacket.Size = new System.Drawing.Size(156, 26);
            this.chkDropDataPacket.TabIndex = 15;
            this.chkDropDataPacket.Text = "Drop Data Packet";
            this.chkDropDataPacket.UseVisualStyleBackColor = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 494);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(839, 22);
            this.statusStrip.TabIndex = 16;
            this.statusStrip.Text = "statusStrip1";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnFindNeighbor);
            this.panel2.Controls.Add(this.btnResetAll);
            this.panel2.Controls.Add(this.txtSystemIp);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lviewNeighborLog);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(839, 165);
            this.panel2.TabIndex = 17;
            // 
            // btnFindNeighbor
            // 
            this.btnFindNeighbor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnFindNeighbor.Location = new System.Drawing.Point(0, 91);
            this.btnFindNeighbor.Name = "btnFindNeighbor";
            this.btnFindNeighbor.Size = new System.Drawing.Size(358, 35);
            this.btnFindNeighbor.TabIndex = 14;
            this.btnFindNeighbor.Text = "Find 1 Hop Neighbors";
            this.btnFindNeighbor.UseVisualStyleBackColor = true;
            this.btnFindNeighbor.Click += new System.EventHandler(this.btnFindNeighbor_Click);
            // 
            // btnResetAll
            // 
            this.btnResetAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnResetAll.Location = new System.Drawing.Point(0, 126);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(358, 35);
            this.btnResetAll.TabIndex = 18;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // txtSystemIp
            // 
            this.txtSystemIp.Location = new System.Drawing.Point(54, 47);
            this.txtSystemIp.Name = "txtSystemIp";
            this.txtSystemIp.ReadOnly = true;
            this.txtSystemIp.Size = new System.Drawing.Size(254, 31);
            this.txtSystemIp.TabIndex = 17;
            this.txtSystemIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(54, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(254, 35);
            this.label3.TabIndex = 16;
            this.label3.Text = "System IP Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lviewNeighborLog
            // 
            this.lviewNeighborLog.BackColor = System.Drawing.SystemColors.Control;
            this.lviewNeighborLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chNodeIp,
            this.chSourceTrust,
            this.chRouterTrust,
            this.chRREQCount,
            this.chSentCount,
            this.chReceivedCount});
            this.lviewNeighborLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.lviewNeighborLog.Font = new System.Drawing.Font("Calibri", 9F);
            this.lviewNeighborLog.FullRowSelect = true;
            this.lviewNeighborLog.Location = new System.Drawing.Point(358, 0);
            this.lviewNeighborLog.MultiSelect = false;
            this.lviewNeighborLog.Name = "lviewNeighborLog";
            this.lviewNeighborLog.Size = new System.Drawing.Size(477, 161);
            this.lviewNeighborLog.TabIndex = 15;
            this.lviewNeighborLog.UseCompatibleStateImageBehavior = false;
            this.lviewNeighborLog.View = System.Windows.Forms.View.Details;
            // 
            // chNodeIp
            // 
            this.chNodeIp.Text = "Node Ip";
            this.chNodeIp.Width = 100;
            // 
            // chSourceTrust
            // 
            this.chSourceTrust.Text = "Source Trust";
            this.chSourceTrust.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSourceTrust.Width = 85;
            // 
            // chRouterTrust
            // 
            this.chRouterTrust.Text = "Router Trust";
            this.chRouterTrust.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chRouterTrust.Width = 85;
            // 
            // chRREQCount
            // 
            this.chRREQCount.Text = "RREQs";
            this.chRREQCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chRREQCount.Width = 58;
            // 
            // chSentCount
            // 
            this.chSentCount.Text = "Sent";
            this.chSentCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSentCount.Width = 58;
            // 
            // chReceivedCount
            // 
            this.chReceivedCount.Text = "Received";
            this.chReceivedCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chReceivedCount.Width = 64;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 516);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.chkDropDataPacket);
            this.Controls.Add(this.tabLog);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TS-AOMDV: Secure Routing for MANETs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabLog.ResumeLayout(false);
            this.tabDataLog.ResumeLayout(false);
            this.tabDataLog.PerformLayout();
            this.tabServiceLog.ResumeLayout(false);
            this.tabRouteLog.ResumeLayout(false);
            this.tabSniferLog.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabLog;
        private System.Windows.Forms.TabPage tabRouteLog;
        public System.Windows.Forms.ListView lviewRouteLog;
        private System.Windows.Forms.ColumnHeader chServiceId2;
        private System.Windows.Forms.ColumnHeader chRouteInfo;
        private System.Windows.Forms.TabPage tabSniferLog;
        public System.Windows.Forms.TreeView tviewSniferLog;
        private System.Windows.Forms.TabPage tabDataLog;
        public System.Windows.Forms.ListView lviewDataLog;
        private System.Windows.Forms.ColumnHeader chServiceId3;
        private System.Windows.Forms.ColumnHeader chFileName;
        private System.Windows.Forms.ColumnHeader chIPFrom;
        private System.Windows.Forms.ColumnHeader chIPTo;
        public System.Windows.Forms.CheckBox chkDropDataPacket;
        public System.Windows.Forms.ListBox lboxTcpAckLog;
        public System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.TextBox txtDestinationIp;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.TextBox txtSystemIp;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ListView lviewNeighborLog;
        private System.Windows.Forms.ColumnHeader chNodeIp;
        private System.Windows.Forms.ColumnHeader chSourceTrust;
        private System.Windows.Forms.ColumnHeader chRouterTrust;
        private System.Windows.Forms.ColumnHeader chRREQCount;
        private System.Windows.Forms.ColumnHeader chSentCount;
        private System.Windows.Forms.ColumnHeader chReceivedCount;
        private System.Windows.Forms.Button btnFindNeighbor;
        private System.Windows.Forms.TabPage tabServiceLog;
        public System.Windows.Forms.ListView lviewServiceLog;
        private System.Windows.Forms.ColumnHeader chServiceId1;
        private System.Windows.Forms.ColumnHeader chSourceIp;
        private System.Windows.Forms.ColumnHeader chDestinationIp;
    }
}

