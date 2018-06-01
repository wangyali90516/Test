namespace RedisToTableTool
{
    partial class RepairDataForm
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
            this.btn_DeleteALl = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_DeleteUserFailed = new System.Windows.Forms.TextBox();
            this.txb_DeleteUserSuccess = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_SDelteAzureTUser = new System.Windows.Forms.Button();
            this.lbl_showMsg = new System.Windows.Forms.Label();
            this.txb_DeleteDatabaseUserF = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_DeleteDatabaseUserS = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txb_F3 = new System.Windows.Forms.TextBox();
            this.txb_F2 = new System.Windows.Forms.TextBox();
            this.txb_F1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txb_S3 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txb_S2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txb_S1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_UpdateRatio = new System.Windows.Forms.Button();
            this.btn_UpdateRedeem = new System.Windows.Forms.Button();
            this.btn_StartRepair = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txb_F4db = new System.Windows.Forms.TextBox();
            this.txb_F4 = new System.Windows.Forms.TextBox();
            this.txb_DemandDeleteDbNumber = new System.Windows.Forms.TextBox();
            this.lbl_showMsgDelete = new System.Windows.Forms.Label();
            this.txb_S4db = new System.Windows.Forms.TextBox();
            this.txb_totalDeletePurchaseNumber = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txb_S4 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btn_DeletePurchaseOrder = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_UpdateRedemption = new System.Windows.Forms.Button();
            this.btn_UpdateAssetUserRatio = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_DeleteALl
            // 
            this.btn_DeleteALl.Location = new System.Drawing.Point(233, 247);
            this.btn_DeleteALl.Margin = new System.Windows.Forms.Padding(6);
            this.btn_DeleteALl.Name = "btn_DeleteALl";
            this.btn_DeleteALl.Size = new System.Drawing.Size(173, 30);
            this.btn_DeleteALl.TabIndex = 1;
            this.btn_DeleteALl.Text = "开始删除数据";
            this.btn_DeleteALl.UseVisualStyleBackColor = true;
            this.btn_DeleteALl.Click += new System.EventHandler(this.btn_Reload_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(120, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "删除失败数量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "成功删除AzureTable订单数量：";
            // 
            // txb_DeleteUserFailed
            // 
            this.txb_DeleteUserFailed.Enabled = false;
            this.txb_DeleteUserFailed.Location = new System.Drawing.Point(233, 76);
            this.txb_DeleteUserFailed.Name = "txb_DeleteUserFailed";
            this.txb_DeleteUserFailed.Size = new System.Drawing.Size(368, 27);
            this.txb_DeleteUserFailed.TabIndex = 9;
            this.txb_DeleteUserFailed.Text = "0";
            // 
            // txb_DeleteUserSuccess
            // 
            this.txb_DeleteUserSuccess.Enabled = false;
            this.txb_DeleteUserSuccess.Location = new System.Drawing.Point(233, 27);
            this.txb_DeleteUserSuccess.Name = "txb_DeleteUserSuccess";
            this.txb_DeleteUserSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_DeleteUserSuccess.TabIndex = 10;
            this.txb_DeleteUserSuccess.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SDelteAzureTUser);
            this.groupBox1.Controls.Add(this.lbl_showMsg);
            this.groupBox1.Controls.Add(this.txb_DeleteDatabaseUserF);
            this.groupBox1.Controls.Add(this.btn_DeleteALl);
            this.groupBox1.Controls.Add(this.txb_DeleteUserFailed);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txb_DeleteDatabaseUserS);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txb_DeleteUserSuccess);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(2, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(615, 290);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "删除AzureTable和Database订单";
            // 
            // btn_SDelteAzureTUser
            // 
            this.btn_SDelteAzureTUser.Location = new System.Drawing.Point(428, 247);
            this.btn_SDelteAzureTUser.Margin = new System.Windows.Forms.Padding(6);
            this.btn_SDelteAzureTUser.Name = "btn_SDelteAzureTUser";
            this.btn_SDelteAzureTUser.Size = new System.Drawing.Size(173, 30);
            this.btn_SDelteAzureTUser.TabIndex = 1;
            this.btn_SDelteAzureTUser.Text = "补删数据";
            this.btn_SDelteAzureTUser.UseVisualStyleBackColor = true;
            this.btn_SDelteAzureTUser.Click += new System.EventHandler(this.btn_SDelteAzureTUser_Click);
            // 
            // lbl_showMsg
            // 
            this.lbl_showMsg.AutoSize = true;
            this.lbl_showMsg.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsg.Location = new System.Drawing.Point(10, 258);
            this.lbl_showMsg.Name = "lbl_showMsg";
            this.lbl_showMsg.Size = new System.Drawing.Size(54, 19);
            this.lbl_showMsg.TabIndex = 11;
            this.lbl_showMsg.Text = "提示：";
            // 
            // txb_DeleteDatabaseUserF
            // 
            this.txb_DeleteDatabaseUserF.Enabled = false;
            this.txb_DeleteDatabaseUserF.Location = new System.Drawing.Point(233, 158);
            this.txb_DeleteDatabaseUserF.Name = "txb_DeleteDatabaseUserF";
            this.txb_DeleteDatabaseUserF.Size = new System.Drawing.Size(368, 27);
            this.txb_DeleteDatabaseUserF.TabIndex = 13;
            this.txb_DeleteDatabaseUserF.Text = "0";
            this.txb_DeleteDatabaseUserF.TextChanged += new System.EventHandler(this.txb_DeleteDatabaseUserF_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "删除失败数量：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txb_DeleteDatabaseUserS
            // 
            this.txb_DeleteDatabaseUserS.Enabled = false;
            this.txb_DeleteDatabaseUserS.Location = new System.Drawing.Point(233, 109);
            this.txb_DeleteDatabaseUserS.Name = "txb_DeleteDatabaseUserS";
            this.txb_DeleteDatabaseUserS.Size = new System.Drawing.Size(368, 27);
            this.txb_DeleteDatabaseUserS.TabIndex = 14;
            this.txb_DeleteDatabaseUserS.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(29, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "成功删除Database订单数量：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txb_F3);
            this.groupBox2.Controls.Add(this.txb_F2);
            this.groupBox2.Controls.Add(this.txb_F1);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txb_S3);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txb_S2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txb_S1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btn_UpdateAssetUserRatio);
            this.groupBox2.Controls.Add(this.btn_UpdateRatio);
            this.groupBox2.Controls.Add(this.btn_UpdateRedemption);
            this.groupBox2.Controls.Add(this.btn_UpdateRedeem);
            this.groupBox2.Controls.Add(this.btn_StartRepair);
            this.groupBox2.Location = new System.Drawing.Point(2, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(615, 456);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "修复数据2";
            // 
            // txb_F3
            // 
            this.txb_F3.Enabled = false;
            this.txb_F3.Location = new System.Drawing.Point(124, 374);
            this.txb_F3.Name = "txb_F3";
            this.txb_F3.Size = new System.Drawing.Size(368, 27);
            this.txb_F3.TabIndex = 13;
            this.txb_F3.Text = "0";
            // 
            // txb_F2
            // 
            this.txb_F2.Enabled = false;
            this.txb_F2.Location = new System.Drawing.Point(124, 220);
            this.txb_F2.Name = "txb_F2";
            this.txb_F2.Size = new System.Drawing.Size(368, 27);
            this.txb_F2.TabIndex = 13;
            this.txb_F2.Text = "0";
            // 
            // txb_F1
            // 
            this.txb_F1.Enabled = false;
            this.txb_F1.Location = new System.Drawing.Point(124, 75);
            this.txb_F1.Name = "txb_F1";
            this.txb_F1.Size = new System.Drawing.Size(368, 27);
            this.txb_F1.TabIndex = 13;
            this.txb_F1.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(11, 381);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 20);
            this.label10.TabIndex = 11;
            this.label10.Text = "删除失败数量：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 227);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 20);
            this.label8.TabIndex = 11;
            this.label8.Text = "删除失败数量：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "删除失败数量：";
            // 
            // txb_S3
            // 
            this.txb_S3.Enabled = false;
            this.txb_S3.Location = new System.Drawing.Point(124, 325);
            this.txb_S3.Name = "txb_S3";
            this.txb_S3.Size = new System.Drawing.Size(368, 27);
            this.txb_S3.TabIndex = 14;
            this.txb_S3.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(39, 332);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "成功数量：";
            // 
            // txb_S2
            // 
            this.txb_S2.Enabled = false;
            this.txb_S2.Location = new System.Drawing.Point(124, 171);
            this.txb_S2.Name = "txb_S2";
            this.txb_S2.Size = new System.Drawing.Size(368, 27);
            this.txb_S2.TabIndex = 14;
            this.txb_S2.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(39, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "成功数量：";
            // 
            // txb_S1
            // 
            this.txb_S1.Enabled = false;
            this.txb_S1.Location = new System.Drawing.Point(124, 26);
            this.txb_S1.Name = "txb_S1";
            this.txb_S1.Size = new System.Drawing.Size(368, 27);
            this.txb_S1.TabIndex = 14;
            this.txb_S1.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(39, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "成功数量：";
            // 
            // btn_UpdateRatio
            // 
            this.btn_UpdateRatio.Location = new System.Drawing.Point(111, 415);
            this.btn_UpdateRatio.Margin = new System.Windows.Forms.Padding(6);
            this.btn_UpdateRatio.Name = "btn_UpdateRatio";
            this.btn_UpdateRatio.Size = new System.Drawing.Size(173, 30);
            this.btn_UpdateRatio.TabIndex = 3;
            this.btn_UpdateRatio.Text = "开始修复比例数据";
            this.btn_UpdateRatio.UseVisualStyleBackColor = true;
            this.btn_UpdateRatio.Visible = false;
            this.btn_UpdateRatio.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_UpdateRedeem
            // 
            this.btn_UpdateRedeem.Location = new System.Drawing.Point(124, 275);
            this.btn_UpdateRedeem.Margin = new System.Windows.Forms.Padding(6);
            this.btn_UpdateRedeem.Name = "btn_UpdateRedeem";
            this.btn_UpdateRedeem.Size = new System.Drawing.Size(173, 30);
            this.btn_UpdateRedeem.TabIndex = 2;
            this.btn_UpdateRedeem.Text = "修改赎回数据";
            this.btn_UpdateRedeem.UseVisualStyleBackColor = true;
            this.btn_UpdateRedeem.Visible = false;
            this.btn_UpdateRedeem.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_StartRepair
            // 
            this.btn_StartRepair.Location = new System.Drawing.Point(319, 121);
            this.btn_StartRepair.Margin = new System.Windows.Forms.Padding(6);
            this.btn_StartRepair.Name = "btn_StartRepair";
            this.btn_StartRepair.Size = new System.Drawing.Size(173, 30);
            this.btn_StartRepair.TabIndex = 1;
            this.btn_StartRepair.Text = "修改购买数据";
            this.btn_StartRepair.UseVisualStyleBackColor = true;
            this.btn_StartRepair.Click += new System.EventHandler(this.btn_StartRepair_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txb_F4db);
            this.groupBox3.Controls.Add(this.txb_F4);
            this.groupBox3.Controls.Add(this.txb_DemandDeleteDbNumber);
            this.groupBox3.Controls.Add(this.lbl_showMsgDelete);
            this.groupBox3.Controls.Add(this.txb_S4db);
            this.groupBox3.Controls.Add(this.txb_totalDeletePurchaseNumber);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txb_S4);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.btn_DeletePurchaseOrder);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(623, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(585, 753);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "删除购买订单";
            // 
            // txb_F4db
            // 
            this.txb_F4db.Enabled = false;
            this.txb_F4db.Location = new System.Drawing.Point(188, 297);
            this.txb_F4db.Name = "txb_F4db";
            this.txb_F4db.Size = new System.Drawing.Size(368, 27);
            this.txb_F4db.TabIndex = 13;
            this.txb_F4db.Text = "0";
            // 
            // txb_F4
            // 
            this.txb_F4.Enabled = false;
            this.txb_F4.Location = new System.Drawing.Point(188, 142);
            this.txb_F4.Name = "txb_F4";
            this.txb_F4.Size = new System.Drawing.Size(368, 27);
            this.txb_F4.TabIndex = 13;
            this.txb_F4.Text = "0";
            // 
            // txb_DemandDeleteDbNumber
            // 
            this.txb_DemandDeleteDbNumber.Enabled = false;
            this.txb_DemandDeleteDbNumber.Location = new System.Drawing.Point(188, 200);
            this.txb_DemandDeleteDbNumber.Name = "txb_DemandDeleteDbNumber";
            this.txb_DemandDeleteDbNumber.Size = new System.Drawing.Size(368, 27);
            this.txb_DemandDeleteDbNumber.TabIndex = 14;
            this.txb_DemandDeleteDbNumber.Text = "0";
            // 
            // lbl_showMsgDelete
            // 
            this.lbl_showMsgDelete.AutoSize = true;
            this.lbl_showMsgDelete.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsgDelete.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsgDelete.Location = new System.Drawing.Point(24, 376);
            this.lbl_showMsgDelete.Name = "lbl_showMsgDelete";
            this.lbl_showMsgDelete.Size = new System.Drawing.Size(54, 19);
            this.lbl_showMsgDelete.TabIndex = 11;
            this.lbl_showMsgDelete.Text = "提示：";
            // 
            // txb_S4db
            // 
            this.txb_S4db.Enabled = false;
            this.txb_S4db.Location = new System.Drawing.Point(188, 254);
            this.txb_S4db.Name = "txb_S4db";
            this.txb_S4db.Size = new System.Drawing.Size(368, 27);
            this.txb_S4db.TabIndex = 14;
            this.txb_S4db.Text = "0";
            // 
            // txb_totalDeletePurchaseNumber
            // 
            this.txb_totalDeletePurchaseNumber.Enabled = false;
            this.txb_totalDeletePurchaseNumber.Location = new System.Drawing.Point(188, 45);
            this.txb_totalDeletePurchaseNumber.Name = "txb_totalDeletePurchaseNumber";
            this.txb_totalDeletePurchaseNumber.Size = new System.Drawing.Size(368, 27);
            this.txb_totalDeletePurchaseNumber.TabIndex = 14;
            this.txb_totalDeletePurchaseNumber.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(24, 203);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(170, 20);
            this.label16.TabIndex = 12;
            this.label16.Text = "需要删除Database数量：";
            // 
            // txb_S4
            // 
            this.txb_S4.Enabled = false;
            this.txb_S4.Location = new System.Drawing.Point(188, 92);
            this.txb_S4.Name = "txb_S4";
            this.txb_S4.Size = new System.Drawing.Size(368, 27);
            this.txb_S4.TabIndex = 14;
            this.txb_S4.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(11, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(183, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "需要删除AzureTable数量：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(115, 258);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 20);
            this.label15.TabIndex = 12;
            this.label15.Text = "成功数量：";
            // 
            // btn_DeletePurchaseOrder
            // 
            this.btn_DeletePurchaseOrder.Location = new System.Drawing.Point(383, 369);
            this.btn_DeletePurchaseOrder.Margin = new System.Windows.Forms.Padding(6);
            this.btn_DeletePurchaseOrder.Name = "btn_DeletePurchaseOrder";
            this.btn_DeletePurchaseOrder.Size = new System.Drawing.Size(173, 30);
            this.btn_DeletePurchaseOrder.TabIndex = 3;
            this.btn_DeletePurchaseOrder.Text = "开始删除购买数据";
            this.btn_DeletePurchaseOrder.UseVisualStyleBackColor = true;
            this.btn_DeletePurchaseOrder.Click += new System.EventHandler(this.tbn_DeletePurchaseOrder_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(87, 300);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 20);
            this.label14.TabIndex = 11;
            this.label14.Text = "删除失败数量：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(115, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 20);
            this.label11.TabIndex = 12;
            this.label11.Text = "成功数量：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(87, 145);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(107, 20);
            this.label12.TabIndex = 11;
            this.label12.Text = "删除失败数量：";
            // 
            // btn_UpdateRedemption
            // 
            this.btn_UpdateRedemption.Location = new System.Drawing.Point(309, 275);
            this.btn_UpdateRedemption.Margin = new System.Windows.Forms.Padding(6);
            this.btn_UpdateRedemption.Name = "btn_UpdateRedemption";
            this.btn_UpdateRedemption.Size = new System.Drawing.Size(279, 30);
            this.btn_UpdateRedemption.TabIndex = 2;
            this.btn_UpdateRedemption.Text = "根据RansomOrderids修改赎回数据";
            this.btn_UpdateRedemption.UseVisualStyleBackColor = true;
            this.btn_UpdateRedemption.Click += new System.EventHandler(this.btn_UpdateRedemption_Click);
            // 
            // btn_UpdateAssetUserRatio
            // 
            this.btn_UpdateAssetUserRatio.Location = new System.Drawing.Point(296, 415);
            this.btn_UpdateAssetUserRatio.Margin = new System.Windows.Forms.Padding(6);
            this.btn_UpdateAssetUserRatio.Name = "btn_UpdateAssetUserRatio";
            this.btn_UpdateAssetUserRatio.Size = new System.Drawing.Size(279, 30);
            this.btn_UpdateAssetUserRatio.TabIndex = 3;
            this.btn_UpdateAssetUserRatio.Text = "根据DebtToTransferIds开始修复比例数据";
            this.btn_UpdateAssetUserRatio.UseVisualStyleBackColor = true;
            this.btn_UpdateAssetUserRatio.Click += new System.EventHandler(this.btn_UpdateAssetUserRatio_Click);
            // 
            // RepairDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1211, 758);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "RepairDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RepairDataForm";
            this.Load += new System.EventHandler(this.RepairDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DeleteALl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_DeleteUserFailed;
        private System.Windows.Forms.TextBox txb_DeleteUserSuccess;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txb_DeleteDatabaseUserF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_DeleteDatabaseUserS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_SDelteAzureTUser;
        private System.Windows.Forms.Label lbl_showMsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_StartRepair;
        private System.Windows.Forms.Button btn_UpdateRatio;
        private System.Windows.Forms.Button btn_UpdateRedeem;
        private System.Windows.Forms.TextBox txb_F1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txb_S1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txb_F2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txb_S2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txb_F3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txb_S3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txb_F4;
        private System.Windows.Forms.TextBox txb_S4;
        private System.Windows.Forms.Button btn_DeletePurchaseOrder;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txb_totalDeletePurchaseNumber;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbl_showMsgDelete;
        private System.Windows.Forms.TextBox txb_F4db;
        private System.Windows.Forms.TextBox txb_DemandDeleteDbNumber;
        private System.Windows.Forms.TextBox txb_S4db;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_UpdateRedemption;
        private System.Windows.Forms.Button btn_UpdateAssetUserRatio;
    }
}