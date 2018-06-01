namespace RedisToTableTool
{
    partial class CheckOneRedisUserAssetInfoForm
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
            this.btn_checkAllAsset = new System.Windows.Forms.Button();
            this.lbl_ShowAllUserInfo = new System.Windows.Forms.Label();
            this.btn_checkAllUser = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txb_checkAssetFailed = new System.Windows.Forms.TextBox();
            this.txb_checkAssetSuccess = new System.Windows.Forms.TextBox();
            this.lbl_showMsg_Asset = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_checkUserFailed = new System.Windows.Forms.TextBox();
            this.lbl_showMsg_User = new System.Windows.Forms.Label();
            this.txb_checkUserSuccess = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_CheckAllUserInfo = new System.Windows.Forms.Button();
            this.txb_allAssetSuccess = new System.Windows.Forms.TextBox();
            this.txb_allAssetFail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txb_allUserSuccess = new System.Windows.Forms.TextBox();
            this.txb_allUserFail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_CheckAllAssetInfo = new System.Windows.Forms.Button();
            this.lbl_ShowAllAssetInfo = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_allTableAsset = new System.Windows.Forms.Button();
            this.btn_allTableUser = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txb_AllTableUserFail = new System.Windows.Forms.TextBox();
            this.txb_AllTableUserSuccess = new System.Windows.Forms.TextBox();
            this.txb_AllTableAssetFail = new System.Windows.Forms.TextBox();
            this.txb_AllTableAssetSuccess = new System.Windows.Forms.TextBox();
            this.lbl_showMsgAllTableAsset = new System.Windows.Forms.Label();
            this.lbl_showMsgAllTableUser = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_checkAllAsset
            // 
            this.btn_checkAllAsset.Location = new System.Drawing.Point(407, 145);
            this.btn_checkAllAsset.Name = "btn_checkAllAsset";
            this.btn_checkAllAsset.Size = new System.Drawing.Size(183, 29);
            this.btn_checkAllAsset.TabIndex = 0;
            this.btn_checkAllAsset.Text = "StartCheckAssetInfo";
            this.btn_checkAllAsset.UseVisualStyleBackColor = true;
            this.btn_checkAllAsset.Click += new System.EventHandler(this.btn_checkAll_Click);
            // 
            // lbl_ShowAllUserInfo
            // 
            this.lbl_ShowAllUserInfo.AutoSize = true;
            this.lbl_ShowAllUserInfo.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_ShowAllUserInfo.ForeColor = System.Drawing.Color.Red;
            this.lbl_ShowAllUserInfo.Location = new System.Drawing.Point(20, 347);
            this.lbl_ShowAllUserInfo.Name = "lbl_ShowAllUserInfo";
            this.lbl_ShowAllUserInfo.Size = new System.Drawing.Size(54, 19);
            this.lbl_ShowAllUserInfo.TabIndex = 1;
            this.lbl_ShowAllUserInfo.Text = "提示：";
            // 
            // btn_checkAllUser
            // 
            this.btn_checkAllUser.Location = new System.Drawing.Point(407, 154);
            this.btn_checkAllUser.Name = "btn_checkAllUser";
            this.btn_checkAllUser.Size = new System.Drawing.Size(183, 29);
            this.btn_checkAllUser.TabIndex = 0;
            this.btn_checkAllUser.Text = "StartCheckUserInfo";
            this.btn_checkAllUser.UseVisualStyleBackColor = true;
            this.btn_checkAllUser.Click += new System.EventHandler(this.btn_checkAllUser_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txb_checkAssetFailed);
            this.groupBox1.Controls.Add(this.txb_checkAssetSuccess);
            this.groupBox1.Controls.Add(this.lbl_showMsg_Asset);
            this.groupBox1.Controls.Add(this.btn_checkAllAsset);
            this.groupBox1.Location = new System.Drawing.Point(5, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 180);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "验证单个Redis资产信息";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "验证错误资产数量：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "验证正确资产数量：";
            // 
            // txb_checkAssetFailed
            // 
            this.txb_checkAssetFailed.Enabled = false;
            this.txb_checkAssetFailed.Location = new System.Drawing.Point(161, 89);
            this.txb_checkAssetFailed.Name = "txb_checkAssetFailed";
            this.txb_checkAssetFailed.Size = new System.Drawing.Size(368, 27);
            this.txb_checkAssetFailed.TabIndex = 5;
            this.txb_checkAssetFailed.Text = "0";
            // 
            // txb_checkAssetSuccess
            // 
            this.txb_checkAssetSuccess.Enabled = false;
            this.txb_checkAssetSuccess.Location = new System.Drawing.Point(161, 36);
            this.txb_checkAssetSuccess.Name = "txb_checkAssetSuccess";
            this.txb_checkAssetSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_checkAssetSuccess.TabIndex = 6;
            this.txb_checkAssetSuccess.Text = "0";
            // 
            // lbl_showMsg_Asset
            // 
            this.lbl_showMsg_Asset.AutoSize = true;
            this.lbl_showMsg_Asset.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsg_Asset.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsg_Asset.Location = new System.Drawing.Point(20, 150);
            this.lbl_showMsg_Asset.Name = "lbl_showMsg_Asset";
            this.lbl_showMsg_Asset.Size = new System.Drawing.Size(54, 19);
            this.lbl_showMsg_Asset.TabIndex = 1;
            this.lbl_showMsg_Asset.Text = "提示：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btn_checkAllUser);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txb_checkUserFailed);
            this.groupBox2.Controls.Add(this.lbl_showMsg_User);
            this.groupBox2.Controls.Add(this.txb_checkUserSuccess);
            this.groupBox2.Location = new System.Drawing.Point(5, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(597, 189);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "验证单个Redis用户信息";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "验证错误订单数量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "验证正确订单数量：";
            // 
            // txb_checkUserFailed
            // 
            this.txb_checkUserFailed.Enabled = false;
            this.txb_checkUserFailed.Location = new System.Drawing.Point(161, 93);
            this.txb_checkUserFailed.Name = "txb_checkUserFailed";
            this.txb_checkUserFailed.Size = new System.Drawing.Size(368, 27);
            this.txb_checkUserFailed.TabIndex = 5;
            this.txb_checkUserFailed.Text = "0";
            // 
            // lbl_showMsg_User
            // 
            this.lbl_showMsg_User.AutoSize = true;
            this.lbl_showMsg_User.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsg_User.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsg_User.Location = new System.Drawing.Point(20, 159);
            this.lbl_showMsg_User.Name = "lbl_showMsg_User";
            this.lbl_showMsg_User.Size = new System.Drawing.Size(54, 19);
            this.lbl_showMsg_User.TabIndex = 1;
            this.lbl_showMsg_User.Text = "提示：";
            // 
            // txb_checkUserSuccess
            // 
            this.txb_checkUserSuccess.Enabled = false;
            this.txb_checkUserSuccess.Location = new System.Drawing.Point(161, 40);
            this.txb_checkUserSuccess.Name = "txb_checkUserSuccess";
            this.txb_checkUserSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_checkUserSuccess.TabIndex = 6;
            this.txb_checkUserSuccess.Text = "0";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btn_CheckAllAssetInfo);
            this.groupBox3.Controls.Add(this.btn_CheckAllUserInfo);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.lbl_ShowAllAssetInfo);
            this.groupBox3.Controls.Add(this.lbl_ShowAllUserInfo);
            this.groupBox3.Controls.Add(this.txb_allUserFail);
            this.groupBox3.Controls.Add(this.txb_allUserSuccess);
            this.groupBox3.Controls.Add(this.txb_allAssetFail);
            this.groupBox3.Controls.Add(this.txb_allAssetSuccess);
            this.groupBox3.Location = new System.Drawing.Point(5, 382);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(597, 377);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "验证所有匹配资产用户信息";
            // 
            // btn_CheckAllUserInfo
            // 
            this.btn_CheckAllUserInfo.Location = new System.Drawing.Point(408, 342);
            this.btn_CheckAllUserInfo.Name = "btn_CheckAllUserInfo";
            this.btn_CheckAllUserInfo.Size = new System.Drawing.Size(183, 29);
            this.btn_CheckAllUserInfo.TabIndex = 0;
            this.btn_CheckAllUserInfo.Text = "StartCheckAlUserlnfo";
            this.btn_CheckAllUserInfo.UseVisualStyleBackColor = true;
            this.btn_CheckAllUserInfo.Click += new System.EventHandler(this.btn_CheckAllUserInfo_Click);
            // 
            // txb_allAssetSuccess
            // 
            this.txb_allAssetSuccess.Enabled = false;
            this.txb_allAssetSuccess.Location = new System.Drawing.Point(161, 35);
            this.txb_allAssetSuccess.Name = "txb_allAssetSuccess";
            this.txb_allAssetSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_allAssetSuccess.TabIndex = 6;
            this.txb_allAssetSuccess.Text = "0";
            // 
            // txb_allAssetFail
            // 
            this.txb_allAssetFail.Enabled = false;
            this.txb_allAssetFail.Location = new System.Drawing.Point(161, 88);
            this.txb_allAssetFail.Name = "txb_allAssetFail";
            this.txb_allAssetFail.Size = new System.Drawing.Size(368, 27);
            this.txb_allAssetFail.TabIndex = 5;
            this.txb_allAssetFail.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "验证正确资产数量：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "验证错误资产单数量：";
            // 
            // txb_allUserSuccess
            // 
            this.txb_allUserSuccess.Enabled = false;
            this.txb_allUserSuccess.Location = new System.Drawing.Point(161, 233);
            this.txb_allUserSuccess.Name = "txb_allUserSuccess";
            this.txb_allUserSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_allUserSuccess.TabIndex = 6;
            this.txb_allUserSuccess.Text = "0";
            // 
            // txb_allUserFail
            // 
            this.txb_allUserFail.Enabled = false;
            this.txb_allUserFail.Location = new System.Drawing.Point(161, 286);
            this.txb_allUserFail.Name = "txb_allUserFail";
            this.txb_allUserFail.Size = new System.Drawing.Size(368, 27);
            this.txb_allUserFail.TabIndex = 5;
            this.txb_allUserFail.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(20, 236);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 20);
            this.label8.TabIndex = 4;
            this.label8.Text = "验证正确订单数量：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(20, 286);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 20);
            this.label9.TabIndex = 3;
            this.label9.Text = "验证错误订单数量：";
            // 
            // btn_CheckAllAssetInfo
            // 
            this.btn_CheckAllAssetInfo.Location = new System.Drawing.Point(407, 168);
            this.btn_CheckAllAssetInfo.Name = "btn_CheckAllAssetInfo";
            this.btn_CheckAllAssetInfo.Size = new System.Drawing.Size(183, 29);
            this.btn_CheckAllAssetInfo.TabIndex = 0;
            this.btn_CheckAllAssetInfo.Text = "StartCheckAlAssetInfo";
            this.btn_CheckAllAssetInfo.UseVisualStyleBackColor = true;
            this.btn_CheckAllAssetInfo.Click += new System.EventHandler(this.btn_CheckAllInfo_Click);
            // 
            // lbl_ShowAllAssetInfo
            // 
            this.lbl_ShowAllAssetInfo.AutoSize = true;
            this.lbl_ShowAllAssetInfo.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_ShowAllAssetInfo.ForeColor = System.Drawing.Color.Red;
            this.lbl_ShowAllAssetInfo.Location = new System.Drawing.Point(20, 173);
            this.lbl_ShowAllAssetInfo.Name = "lbl_ShowAllAssetInfo";
            this.lbl_ShowAllAssetInfo.Size = new System.Drawing.Size(54, 19);
            this.lbl_ShowAllAssetInfo.TabIndex = 1;
            this.lbl_ShowAllAssetInfo.Text = "提示：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.btn_allTableAsset);
            this.groupBox4.Controls.Add(this.btn_allTableUser);
            this.groupBox4.Controls.Add(this.lbl_showMsgAllTableUser);
            this.groupBox4.Controls.Add(this.lbl_showMsgAllTableAsset);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.txb_AllTableUserFail);
            this.groupBox4.Controls.Add(this.txb_AllTableUserSuccess);
            this.groupBox4.Controls.Add(this.txb_AllTableAssetFail);
            this.groupBox4.Controls.Add(this.txb_AllTableAssetSuccess);
            this.groupBox4.Location = new System.Drawing.Point(608, 1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(541, 767);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Reload到AzureTable后验证资产和用户订单信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "验证错误订单数量：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(8, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(149, 20);
            this.label10.TabIndex = 10;
            this.label10.Text = "验证错误资产单数量：";
            // 
            // btn_allTableAsset
            // 
            this.btn_allTableAsset.Location = new System.Drawing.Point(288, 145);
            this.btn_allTableAsset.Name = "btn_allTableAsset";
            this.btn_allTableAsset.Size = new System.Drawing.Size(243, 29);
            this.btn_allTableAsset.TabIndex = 7;
            this.btn_allTableAsset.Text = "StartCheckAllTableAssetInfo";
            this.btn_allTableAsset.UseVisualStyleBackColor = true;
            this.btn_allTableAsset.Click += new System.EventHandler(this.btn_allTableAsset_Click);
            // 
            // btn_allTableUser
            // 
            this.btn_allTableUser.Location = new System.Drawing.Point(288, 340);
            this.btn_allTableUser.Name = "btn_allTableUser";
            this.btn_allTableUser.Size = new System.Drawing.Size(243, 29);
            this.btn_allTableUser.TabIndex = 8;
            this.btn_allTableUser.Text = "StartCheckAllTableUserlnfo";
            this.btn_allTableUser.UseVisualStyleBackColor = true;
            this.btn_allTableUser.Click += new System.EventHandler(this.btn_allTableUser_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(22, 234);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 20);
            this.label11.TabIndex = 11;
            this.label11.Text = "验证正确订单数量：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(22, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 20);
            this.label12.TabIndex = 12;
            this.label12.Text = "验证正确资产数量：";
            // 
            // txb_AllTableUserFail
            // 
            this.txb_AllTableUserFail.Enabled = false;
            this.txb_AllTableUserFail.Location = new System.Drawing.Point(163, 284);
            this.txb_AllTableUserFail.Name = "txb_AllTableUserFail";
            this.txb_AllTableUserFail.Size = new System.Drawing.Size(368, 27);
            this.txb_AllTableUserFail.TabIndex = 13;
            this.txb_AllTableUserFail.Text = "0";
            // 
            // txb_AllTableUserSuccess
            // 
            this.txb_AllTableUserSuccess.Enabled = false;
            this.txb_AllTableUserSuccess.Location = new System.Drawing.Point(163, 231);
            this.txb_AllTableUserSuccess.Name = "txb_AllTableUserSuccess";
            this.txb_AllTableUserSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_AllTableUserSuccess.TabIndex = 15;
            this.txb_AllTableUserSuccess.Text = "0";
            // 
            // txb_AllTableAssetFail
            // 
            this.txb_AllTableAssetFail.Enabled = false;
            this.txb_AllTableAssetFail.Location = new System.Drawing.Point(163, 86);
            this.txb_AllTableAssetFail.Name = "txb_AllTableAssetFail";
            this.txb_AllTableAssetFail.Size = new System.Drawing.Size(368, 27);
            this.txb_AllTableAssetFail.TabIndex = 14;
            this.txb_AllTableAssetFail.Text = "0";
            // 
            // txb_AllTableAssetSuccess
            // 
            this.txb_AllTableAssetSuccess.Enabled = false;
            this.txb_AllTableAssetSuccess.Location = new System.Drawing.Point(163, 33);
            this.txb_AllTableAssetSuccess.Name = "txb_AllTableAssetSuccess";
            this.txb_AllTableAssetSuccess.Size = new System.Drawing.Size(368, 27);
            this.txb_AllTableAssetSuccess.TabIndex = 16;
            this.txb_AllTableAssetSuccess.Text = "0";
            // 
            // lbl_showMsgAllTableAsset
            // 
            this.lbl_showMsgAllTableAsset.AutoSize = true;
            this.lbl_showMsgAllTableAsset.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsgAllTableAsset.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsgAllTableAsset.Location = new System.Drawing.Point(22, 150);
            this.lbl_showMsgAllTableAsset.Name = "lbl_showMsgAllTableAsset";
            this.lbl_showMsgAllTableAsset.Size = new System.Drawing.Size(54, 19);
            this.lbl_showMsgAllTableAsset.TabIndex = 1;
            this.lbl_showMsgAllTableAsset.Text = "提示：";
            // 
            // lbl_showMsgAllTableUser
            // 
            this.lbl_showMsgAllTableUser.AutoSize = true;
            this.lbl_showMsgAllTableUser.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsgAllTableUser.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsgAllTableUser.Location = new System.Drawing.Point(22, 345);
            this.lbl_showMsgAllTableUser.Name = "lbl_showMsgAllTableUser";
            this.lbl_showMsgAllTableUser.Size = new System.Drawing.Size(54, 19);
            this.lbl_showMsgAllTableUser.TabIndex = 1;
            this.lbl_showMsgAllTableUser.Text = "提示：";
            // 
            // CheckOneRedisUserAssetInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1153, 771);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "CheckOneRedisUserAssetInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "验证单个redis中数据";
            this.Load += new System.EventHandler(this.CheckOneRedisUserAssetInfoForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_checkAllAsset;
        private System.Windows.Forms.Label lbl_ShowAllUserInfo;
        private System.Windows.Forms.Button btn_checkAllUser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_CheckAllUserInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txb_checkAssetFailed;
        private System.Windows.Forms.TextBox txb_checkAssetSuccess;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_checkUserFailed;
        private System.Windows.Forms.TextBox txb_checkUserSuccess;
        private System.Windows.Forms.Label lbl_showMsg_Asset;
        private System.Windows.Forms.Label lbl_showMsg_User;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txb_allUserFail;
        private System.Windows.Forms.TextBox txb_allUserSuccess;
        private System.Windows.Forms.TextBox txb_allAssetFail;
        private System.Windows.Forms.TextBox txb_allAssetSuccess;
        private System.Windows.Forms.Button btn_CheckAllAssetInfo;
        private System.Windows.Forms.Label lbl_ShowAllAssetInfo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_allTableAsset;
        private System.Windows.Forms.Button btn_allTableUser;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txb_AllTableUserFail;
        private System.Windows.Forms.TextBox txb_AllTableUserSuccess;
        private System.Windows.Forms.TextBox txb_AllTableAssetFail;
        private System.Windows.Forms.TextBox txb_AllTableAssetSuccess;
        private System.Windows.Forms.Label lbl_showMsgAllTableUser;
        private System.Windows.Forms.Label lbl_showMsgAllTableAsset;
    }
}