namespace RedisToTableTool
{
    partial class CheckDiskAssetInfo
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
            this.gb_oneAssetCheck = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_oneAssetId = new System.Windows.Forms.TextBox();
            this.lbl_CheckOneAssetInfo = new System.Windows.Forms.Label();
            this.btn_CheckOneAsset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txb_ShowMsgAll = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_checkAll = new System.Windows.Forms.Button();
            this.txb_checkError = new System.Windows.Forms.TextBox();
            this.txb_checkFailed = new System.Windows.Forms.TextBox();
            this.txb_checkSuccess = new System.Windows.Forms.TextBox();
            this.txb_DemandCheckAssetIdNums = new System.Windows.Forms.TextBox();
            this.gb_oneAssetCheck.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_oneAssetCheck
            // 
            this.gb_oneAssetCheck.Controls.Add(this.label1);
            this.gb_oneAssetCheck.Controls.Add(this.txb_oneAssetId);
            this.gb_oneAssetCheck.Controls.Add(this.lbl_CheckOneAssetInfo);
            this.gb_oneAssetCheck.Controls.Add(this.btn_CheckOneAsset);
            this.gb_oneAssetCheck.Location = new System.Drawing.Point(1, 3);
            this.gb_oneAssetCheck.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gb_oneAssetCheck.Name = "gb_oneAssetCheck";
            this.gb_oneAssetCheck.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gb_oneAssetCheck.Size = new System.Drawing.Size(652, 105);
            this.gb_oneAssetCheck.TabIndex = 0;
            this.gb_oneAssetCheck.TabStop = false;
            this.gb_oneAssetCheck.Text = "验证单个资产信息在磁盘是否正确";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "AssetId：";
            // 
            // txb_oneAssetId
            // 
            this.txb_oneAssetId.Location = new System.Drawing.Point(101, 32);
            this.txb_oneAssetId.Name = "txb_oneAssetId";
            this.txb_oneAssetId.Size = new System.Drawing.Size(363, 23);
            this.txb_oneAssetId.TabIndex = 2;
            // 
            // lbl_CheckOneAssetInfo
            // 
            this.lbl_CheckOneAssetInfo.AutoSize = true;
            this.lbl_CheckOneAssetInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_CheckOneAssetInfo.ForeColor = System.Drawing.Color.Red;
            this.lbl_CheckOneAssetInfo.Location = new System.Drawing.Point(23, 70);
            this.lbl_CheckOneAssetInfo.Name = "lbl_CheckOneAssetInfo";
            this.lbl_CheckOneAssetInfo.Size = new System.Drawing.Size(77, 19);
            this.lbl_CheckOneAssetInfo.TabIndex = 1;
            this.lbl_CheckOneAssetInfo.Text = "ShowMsg";
            // 
            // btn_CheckOneAsset
            // 
            this.btn_CheckOneAsset.Location = new System.Drawing.Point(490, 29);
            this.btn_CheckOneAsset.Name = "btn_CheckOneAsset";
            this.btn_CheckOneAsset.Size = new System.Drawing.Size(84, 29);
            this.btn_CheckOneAsset.TabIndex = 0;
            this.btn_CheckOneAsset.Text = "开始验证";
            this.btn_CheckOneAsset.UseVisualStyleBackColor = true;
            this.btn_CheckOneAsset.Click += new System.EventHandler(this.btn_CheckOneAsset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txb_ShowMsgAll);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_checkAll);
            this.groupBox1.Controls.Add(this.txb_checkError);
            this.groupBox1.Controls.Add(this.txb_checkFailed);
            this.groupBox1.Controls.Add(this.txb_checkSuccess);
            this.groupBox1.Controls.Add(this.txb_DemandCheckAssetIdNums);
            this.groupBox1.Location = new System.Drawing.Point(1, 116);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(652, 557);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "验证所有的资产数据在磁盘是否正确";
            // 
            // txb_ShowMsgAll
            // 
            this.txb_ShowMsgAll.Enabled = false;
            this.txb_ShowMsgAll.Location = new System.Drawing.Point(9, 294);
            this.txb_ShowMsgAll.Multiline = true;
            this.txb_ShowMsgAll.Name = "txb_ShowMsgAll";
            this.txb_ShowMsgAll.Size = new System.Drawing.Size(633, 256);
            this.txb_ShowMsgAll.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(65, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "验证异常资产数量：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(65, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "验证错误资产数量：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(5, 272);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "ShowProcessInfo：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(65, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "验证正确资产数量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "总共需要验证的资产数量：";
            // 
            // btn_checkAll
            // 
            this.btn_checkAll.Location = new System.Drawing.Point(49, 240);
            this.btn_checkAll.Name = "btn_checkAll";
            this.btn_checkAll.Size = new System.Drawing.Size(84, 29);
            this.btn_checkAll.TabIndex = 0;
            this.btn_checkAll.Text = "开始验证";
            this.btn_checkAll.UseVisualStyleBackColor = true;
            this.btn_checkAll.Click += new System.EventHandler(this.btn_checkAll_Click);
            // 
            // txb_checkError
            // 
            this.txb_checkError.Enabled = false;
            this.txb_checkError.Location = new System.Drawing.Point(206, 200);
            this.txb_checkError.Name = "txb_checkError";
            this.txb_checkError.Size = new System.Drawing.Size(368, 23);
            this.txb_checkError.TabIndex = 2;
            this.txb_checkError.Text = "0";
            // 
            // txb_checkFailed
            // 
            this.txb_checkFailed.Enabled = false;
            this.txb_checkFailed.Location = new System.Drawing.Point(206, 147);
            this.txb_checkFailed.Name = "txb_checkFailed";
            this.txb_checkFailed.Size = new System.Drawing.Size(368, 23);
            this.txb_checkFailed.TabIndex = 2;
            this.txb_checkFailed.Text = "0";
            // 
            // txb_checkSuccess
            // 
            this.txb_checkSuccess.Enabled = false;
            this.txb_checkSuccess.Location = new System.Drawing.Point(206, 94);
            this.txb_checkSuccess.Name = "txb_checkSuccess";
            this.txb_checkSuccess.Size = new System.Drawing.Size(368, 23);
            this.txb_checkSuccess.TabIndex = 2;
            this.txb_checkSuccess.Text = "0";
            // 
            // txb_DemandCheckAssetIdNums
            // 
            this.txb_DemandCheckAssetIdNums.Enabled = false;
            this.txb_DemandCheckAssetIdNums.Location = new System.Drawing.Point(206, 39);
            this.txb_DemandCheckAssetIdNums.Name = "txb_DemandCheckAssetIdNums";
            this.txb_DemandCheckAssetIdNums.Size = new System.Drawing.Size(368, 23);
            this.txb_DemandCheckAssetIdNums.TabIndex = 2;
            this.txb_DemandCheckAssetIdNums.Text = "0";
            // 
            // CheckDiskAssetInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(657, 674);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb_oneAssetCheck);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "CheckDiskAssetInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "验证磁盘资产信息";
            this.Load += new System.EventHandler(this.CheckDiskAssetInfo_Load);
            this.gb_oneAssetCheck.ResumeLayout(false);
            this.gb_oneAssetCheck.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_oneAssetCheck;
        private System.Windows.Forms.Button btn_CheckOneAsset;
        private System.Windows.Forms.TextBox txb_oneAssetId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_CheckOneAssetInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_DemandCheckAssetIdNums;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txb_checkFailed;
        private System.Windows.Forms.TextBox txb_checkSuccess;
        private System.Windows.Forms.Button btn_checkAll;
        private System.Windows.Forms.TextBox txb_ShowMsgAll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txb_checkError;
    }
}