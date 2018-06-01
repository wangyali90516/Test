namespace RedisToTableTool
{
    partial class UpdateDataFromBank
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
            this.txb_handleAssetIds = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_HandleThings = new System.Windows.Forms.Button();
            this.txb_SuccessNums = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.txb_CurrentAssetId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_CurrentAssetIdNums = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txb_totalNums = new System.Windows.Forms.TextBox();
            this.lbl_showmsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txb_handleAssetIds
            // 
            this.txb_handleAssetIds.Location = new System.Drawing.Point(13, 45);
            this.txb_handleAssetIds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txb_handleAssetIds.Multiline = true;
            this.txb_handleAssetIds.Name = "txb_handleAssetIds";
            this.txb_handleAssetIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txb_handleAssetIds.Size = new System.Drawing.Size(678, 446);
            this.txb_handleAssetIds.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "HandleAssetIds:";
            // 
            // btn_HandleThings
            // 
            this.btn_HandleThings.Location = new System.Drawing.Point(146, 15);
            this.btn_HandleThings.Name = "btn_HandleThings";
            this.btn_HandleThings.Size = new System.Drawing.Size(97, 25);
            this.btn_HandleThings.TabIndex = 2;
            this.btn_HandleThings.Text = "开始执行";
            this.btn_HandleThings.UseVisualStyleBackColor = true;
            this.btn_HandleThings.Click += new System.EventHandler(this.btn_HandleThings_Click);
            // 
            // txb_SuccessNums
            // 
            this.txb_SuccessNums.Enabled = false;
            this.txb_SuccessNums.Location = new System.Drawing.Point(164, 611);
            this.txb_SuccessNums.Name = "txb_SuccessNums";
            this.txb_SuccessNums.Size = new System.Drawing.Size(527, 27);
            this.txb_SuccessNums.TabIndex = 22;
            this.txb_SuccessNums.Text = "0";
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(3, 611);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(155, 20);
            this.lbl.TabIndex = 19;
            this.lbl.Text = "当前Assetid成功数量：";
            // 
            // txb_CurrentAssetId
            // 
            this.txb_CurrentAssetId.Enabled = false;
            this.txb_CurrentAssetId.Location = new System.Drawing.Point(164, 515);
            this.txb_CurrentAssetId.Name = "txb_CurrentAssetId";
            this.txb_CurrentAssetId.Size = new System.Drawing.Size(527, 27);
            this.txb_CurrentAssetId.TabIndex = 23;
            this.txb_CurrentAssetId.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 518);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "正在执行的AssetId：";
            // 
            // txb_CurrentAssetIdNums
            // 
            this.txb_CurrentAssetIdNums.Enabled = false;
            this.txb_CurrentAssetIdNums.Location = new System.Drawing.Point(164, 565);
            this.txb_CurrentAssetIdNums.Name = "txb_CurrentAssetIdNums";
            this.txb_CurrentAssetIdNums.Size = new System.Drawing.Size(527, 27);
            this.txb_CurrentAssetIdNums.TabIndex = 24;
            this.txb_CurrentAssetIdNums.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 565);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "当前Assetid总共数量：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 668);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "总共成功数量：";
            // 
            // txb_totalNums
            // 
            this.txb_totalNums.Enabled = false;
            this.txb_totalNums.Location = new System.Drawing.Point(164, 661);
            this.txb_totalNums.Name = "txb_totalNums";
            this.txb_totalNums.Size = new System.Drawing.Size(527, 27);
            this.txb_totalNums.TabIndex = 22;
            this.txb_totalNums.Text = "0";
            // 
            // lbl_showmsg
            // 
            this.lbl_showmsg.AutoSize = true;
            this.lbl_showmsg.Location = new System.Drawing.Point(21, 706);
            this.lbl_showmsg.Name = "lbl_showmsg";
            this.lbl_showmsg.Size = new System.Drawing.Size(43, 20);
            this.lbl_showmsg.TabIndex = 25;
            this.lbl_showmsg.Text = "提示:";
            // 
            // UpdateDataFromBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(711, 731);
            this.Controls.Add(this.lbl_showmsg);
            this.Controls.Add(this.txb_totalNums);
            this.Controls.Add(this.txb_SuccessNums);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.txb_CurrentAssetId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_CurrentAssetIdNums);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_HandleThings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txb_handleAssetIds);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "UpdateDataFromBank";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateDataFromBank";
            this.Load += new System.EventHandler(this.UpdateDataFromBank_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txb_handleAssetIds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_HandleThings;
        private System.Windows.Forms.TextBox txb_SuccessNums;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.TextBox txb_CurrentAssetId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_CurrentAssetIdNums;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txb_totalNums;
        private System.Windows.Forms.Label lbl_showmsg;
    }
}