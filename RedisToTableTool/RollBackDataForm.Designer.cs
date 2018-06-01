namespace RedisToTableTool
{
    partial class RollBackDataForm
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
            this.btn_Rollback = new System.Windows.Forms.Button();
            this.cbx_SelectItem = new System.Windows.Forms.ComboBox();
            this.ck_assetDtos = new System.Windows.Forms.CheckBox();
            this.ck_yemUserPurchase = new System.Windows.Forms.CheckBox();
            this.ck_userAsset = new System.Windows.Forms.CheckBox();
            this.ck_assetUser = new System.Windows.Forms.CheckBox();
            this.lbl_assetDtsMsg = new System.Windows.Forms.Label();
            this.lbl_yemUserPurchaseMsg = new System.Windows.Forms.Label();
            this.lbl_userassetMsg = new System.Windows.Forms.Label();
            this.lbl_assetuserMsg = new System.Windows.Forms.Label();
            this.gb = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_userAssetDelete = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_assetuserDelete = new System.Windows.Forms.Label();
            this.gb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Rollback
            // 
            this.btn_Rollback.Location = new System.Drawing.Point(389, 23);
            this.btn_Rollback.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Rollback.Name = "btn_Rollback";
            this.btn_Rollback.Size = new System.Drawing.Size(87, 30);
            this.btn_Rollback.TabIndex = 0;
            this.btn_Rollback.Text = "开始回滚";
            this.btn_Rollback.UseVisualStyleBackColor = true;
            this.btn_Rollback.Click += new System.EventHandler(this.btn_Rollback_Click);
            // 
            // cbx_SelectItem
            // 
            this.cbx_SelectItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_SelectItem.FormattingEnabled = true;
            this.cbx_SelectItem.Items.AddRange(new object[] {
            "Redis",
            "文本文件"});
            this.cbx_SelectItem.Location = new System.Drawing.Point(111, 27);
            this.cbx_SelectItem.Name = "cbx_SelectItem";
            this.cbx_SelectItem.Size = new System.Drawing.Size(262, 25);
            this.cbx_SelectItem.TabIndex = 2;
            // 
            // ck_assetDtos
            // 
            this.ck_assetDtos.AutoSize = true;
            this.ck_assetDtos.Checked = true;
            this.ck_assetDtos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_assetDtos.Location = new System.Drawing.Point(6, 33);
            this.ck_assetDtos.Name = "ck_assetDtos";
            this.ck_assetDtos.Size = new System.Drawing.Size(123, 21);
            this.ck_assetDtos.TabIndex = 8;
            this.ck_assetDtos.Text = "回滚所有资产数据";
            this.ck_assetDtos.UseVisualStyleBackColor = true;
            // 
            // ck_yemUserPurchase
            // 
            this.ck_yemUserPurchase.AutoSize = true;
            this.ck_yemUserPurchase.Checked = true;
            this.ck_yemUserPurchase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_yemUserPurchase.Location = new System.Drawing.Point(6, 73);
            this.ck_yemUserPurchase.Name = "ck_yemUserPurchase";
            this.ck_yemUserPurchase.Size = new System.Drawing.Size(123, 21);
            this.ck_yemUserPurchase.TabIndex = 8;
            this.ck_yemUserPurchase.Text = "回滚用户订单数据";
            this.ck_yemUserPurchase.UseVisualStyleBackColor = true;
            // 
            // ck_userAsset
            // 
            this.ck_userAsset.AutoSize = true;
            this.ck_userAsset.Checked = true;
            this.ck_userAsset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_userAsset.Location = new System.Drawing.Point(6, 113);
            this.ck_userAsset.Name = "ck_userAsset";
            this.ck_userAsset.Size = new System.Drawing.Size(164, 21);
            this.ck_userAsset.TabIndex = 8;
            this.ck_userAsset.Text = "删除redis中用户资产关系";
            this.ck_userAsset.UseVisualStyleBackColor = true;
            // 
            // ck_assetUser
            // 
            this.ck_assetUser.AutoSize = true;
            this.ck_assetUser.Checked = true;
            this.ck_assetUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ck_assetUser.Location = new System.Drawing.Point(6, 156);
            this.ck_assetUser.Name = "ck_assetUser";
            this.ck_assetUser.Size = new System.Drawing.Size(164, 21);
            this.ck_assetUser.TabIndex = 8;
            this.ck_assetUser.Text = "删除redis中资产用户关系";
            this.ck_assetUser.UseVisualStyleBackColor = true;
            // 
            // lbl_assetDtsMsg
            // 
            this.lbl_assetDtsMsg.AutoSize = true;
            this.lbl_assetDtsMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_assetDtsMsg.Location = new System.Drawing.Point(135, 34);
            this.lbl_assetDtsMsg.Name = "lbl_assetDtsMsg";
            this.lbl_assetDtsMsg.Size = new System.Drawing.Size(0, 17);
            this.lbl_assetDtsMsg.TabIndex = 9;
            // 
            // lbl_yemUserPurchaseMsg
            // 
            this.lbl_yemUserPurchaseMsg.AutoSize = true;
            this.lbl_yemUserPurchaseMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_yemUserPurchaseMsg.Location = new System.Drawing.Point(135, 74);
            this.lbl_yemUserPurchaseMsg.Name = "lbl_yemUserPurchaseMsg";
            this.lbl_yemUserPurchaseMsg.Size = new System.Drawing.Size(0, 17);
            this.lbl_yemUserPurchaseMsg.TabIndex = 9;
            // 
            // lbl_userassetMsg
            // 
            this.lbl_userassetMsg.AutoSize = true;
            this.lbl_userassetMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_userassetMsg.Location = new System.Drawing.Point(176, 114);
            this.lbl_userassetMsg.Name = "lbl_userassetMsg";
            this.lbl_userassetMsg.Size = new System.Drawing.Size(0, 17);
            this.lbl_userassetMsg.TabIndex = 9;
            // 
            // lbl_assetuserMsg
            // 
            this.lbl_assetuserMsg.AutoSize = true;
            this.lbl_assetuserMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_assetuserMsg.Location = new System.Drawing.Point(176, 157);
            this.lbl_assetuserMsg.Name = "lbl_assetuserMsg";
            this.lbl_assetuserMsg.Size = new System.Drawing.Size(0, 17);
            this.lbl_assetuserMsg.TabIndex = 9;
            // 
            // gb
            // 
            this.gb.Controls.Add(this.ck_assetDtos);
            this.gb.Controls.Add(this.lbl_assetuserMsg);
            this.gb.Controls.Add(this.ck_yemUserPurchase);
            this.gb.Controls.Add(this.lbl_userassetMsg);
            this.gb.Controls.Add(this.ck_userAsset);
            this.gb.Controls.Add(this.lbl_yemUserPurchaseMsg);
            this.gb.Controls.Add(this.ck_assetUser);
            this.gb.Controls.Add(this.lbl_assetDtsMsg);
            this.gb.Location = new System.Drawing.Point(16, 74);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(460, 192);
            this.gb.TabIndex = 10;
            this.gb.TabStop = false;
            this.gb.Text = "操作栏目";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_assetuserDelete);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lbl_userAssetDelete);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 272);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 126);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检查Redis是否删除";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "回滚数据源：";
            // 
            // lbl_userAssetDelete
            // 
            this.lbl_userAssetDelete.AutoSize = true;
            this.lbl_userAssetDelete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_userAssetDelete.ForeColor = System.Drawing.Color.Red;
            this.lbl_userAssetDelete.Location = new System.Drawing.Point(175, 41);
            this.lbl_userAssetDelete.Name = "lbl_userAssetDelete";
            this.lbl_userAssetDelete.Size = new System.Drawing.Size(0, 17);
            this.lbl_userAssetDelete.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "检查删除用户资产关系：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "检查删除资产用户关系：";
            // 
            // lbl_assetuserDelete
            // 
            this.lbl_assetuserDelete.AutoSize = true;
            this.lbl_assetuserDelete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_assetuserDelete.ForeColor = System.Drawing.Color.Red;
            this.lbl_assetuserDelete.Location = new System.Drawing.Point(175, 83);
            this.lbl_assetuserDelete.Name = "lbl_assetuserDelete";
            this.lbl_assetuserDelete.Size = new System.Drawing.Size(0, 17);
            this.lbl_assetuserDelete.TabIndex = 1;
            // 
            // RollBackDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(482, 405);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbx_SelectItem);
            this.Controls.Add(this.btn_Rollback);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "RollBackDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "回滚资产和用户订单数据";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RollBackDataForm_FormClosing);
            this.Load += new System.EventHandler(this.RollBackDataForm_Load);
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Rollback;
        private System.Windows.Forms.ComboBox cbx_SelectItem;
        private System.Windows.Forms.CheckBox ck_assetDtos;
        private System.Windows.Forms.CheckBox ck_yemUserPurchase;
        private System.Windows.Forms.CheckBox ck_userAsset;
        private System.Windows.Forms.CheckBox ck_assetUser;
        private System.Windows.Forms.Label lbl_assetDtsMsg;
        private System.Windows.Forms.Label lbl_yemUserPurchaseMsg;
        private System.Windows.Forms.Label lbl_userassetMsg;
        private System.Windows.Forms.Label lbl_assetuserMsg;
        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_assetuserDelete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_userAssetDelete;
        private System.Windows.Forms.Label label1;
    }
}