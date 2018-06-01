namespace RedisToTableTool
{
    partial class ReloadUserAssetRatioForm
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
            this.btn_Reload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_AssertRangeStart = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txb_AssertRangeEnd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txb_userSNums = new System.Windows.Forms.TextBox();
            this.txb_userFNums = new System.Windows.Forms.TextBox();
            this.btn_ReloadTxt = new System.Windows.Forms.Button();
            this.lbl_showMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Reload
            // 
            this.btn_Reload.Location = new System.Drawing.Point(209, 186);
            this.btn_Reload.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Reload.Name = "btn_Reload";
            this.btn_Reload.Size = new System.Drawing.Size(200, 33);
            this.btn_Reload.TabIndex = 0;
            this.btn_Reload.Text = "Start Reload(YUNTABLE首选)";
            this.btn_Reload.UseVisualStyleBackColor = true;
            this.btn_Reload.Click += new System.EventHandler(this.btn_Reload_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "资产RangeStart：";
            // 
            // txb_AssertRangeStart
            // 
            this.txb_AssertRangeStart.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_AssertRangeStart.Location = new System.Drawing.Point(141, 13);
            this.txb_AssertRangeStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txb_AssertRangeStart.Name = "txb_AssertRangeStart";
            this.txb_AssertRangeStart.Size = new System.Drawing.Size(173, 27);
            this.txb_AssertRangeStart.TabIndex = 13;
            this.txb_AssertRangeStart.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(322, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "资产RangeEnd：";
            // 
            // txb_AssertRangeEnd
            // 
            this.txb_AssertRangeEnd.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_AssertRangeEnd.Location = new System.Drawing.Point(446, 13);
            this.txb_AssertRangeEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txb_AssertRangeEnd.Name = "txb_AssertRangeEnd";
            this.txb_AssertRangeEnd.Size = new System.Drawing.Size(173, 27);
            this.txb_AssertRangeEnd.TabIndex = 14;
            this.txb_AssertRangeEnd.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(191, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "插入用户资产关系失败数量：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "插入用户资产关系成功数量：";
            // 
            // txb_userSNums
            // 
            this.txb_userSNums.Enabled = false;
            this.txb_userSNums.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_userSNums.Location = new System.Drawing.Point(209, 71);
            this.txb_userSNums.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txb_userSNums.Name = "txb_userSNums";
            this.txb_userSNums.Size = new System.Drawing.Size(412, 27);
            this.txb_userSNums.TabIndex = 15;
            this.txb_userSNums.Text = "0";
            // 
            // txb_userFNums
            // 
            this.txb_userFNums.Enabled = false;
            this.txb_userFNums.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_userFNums.Location = new System.Drawing.Point(209, 132);
            this.txb_userFNums.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txb_userFNums.Name = "txb_userFNums";
            this.txb_userFNums.Size = new System.Drawing.Size(412, 27);
            this.txb_userFNums.TabIndex = 16;
            this.txb_userFNums.Text = "0";
            // 
            // btn_ReloadTxt
            // 
            this.btn_ReloadTxt.Location = new System.Drawing.Point(427, 186);
            this.btn_ReloadTxt.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ReloadTxt.Name = "btn_ReloadTxt";
            this.btn_ReloadTxt.Size = new System.Drawing.Size(192, 33);
            this.btn_ReloadTxt.TabIndex = 0;
            this.btn_ReloadTxt.Text = "Start Reload（TXT）";
            this.btn_ReloadTxt.UseVisualStyleBackColor = true;
            this.btn_ReloadTxt.Click += new System.EventHandler(this.btn_ReloadTxt_Click);
            // 
            // lbl_showMsg
            // 
            this.lbl_showMsg.AutoSize = true;
            this.lbl_showMsg.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_showMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsg.Location = new System.Drawing.Point(12, 245);
            this.lbl_showMsg.Name = "lbl_showMsg";
            this.lbl_showMsg.Size = new System.Drawing.Size(74, 19);
            this.lbl_showMsg.TabIndex = 19;
            this.lbl_showMsg.Text = "信息提示：";
            // 
            // ReloadUserAssetRatioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(660, 282);
            this.Controls.Add(this.lbl_showMsg);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txb_userSNums);
            this.Controls.Add(this.txb_userFNums);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txb_AssertRangeStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txb_AssertRangeEnd);
            this.Controls.Add(this.btn_ReloadTxt);
            this.Controls.Add(this.btn_Reload);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ReloadUserAssetRatioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReloadTable数据到集群磁盘";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Reload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_AssertRangeStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txb_AssertRangeEnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txb_userSNums;
        private System.Windows.Forms.TextBox txb_userFNums;
        private System.Windows.Forms.Button btn_ReloadTxt;
        private System.Windows.Forms.Label lbl_showMsg;
    }
}