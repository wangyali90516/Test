namespace RedisToTableTool
{
    partial class UpdateYemUserProductInfoForm
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
            this.txb_handleUserIds = new System.Windows.Forms.TextBox();
            this.btn_batchUpdate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_CurrentUserId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_CurrentAssetIdNums = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_showmsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txb_handleUserIds
            // 
            this.txb_handleUserIds.Location = new System.Drawing.Point(17, 69);
            this.txb_handleUserIds.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.txb_handleUserIds.Multiline = true;
            this.txb_handleUserIds.Name = "txb_handleUserIds";
            this.txb_handleUserIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txb_handleUserIds.Size = new System.Drawing.Size(677, 578);
            this.txb_handleUserIds.TabIndex = 2;
            // 
            // btn_batchUpdate
            // 
            this.btn_batchUpdate.Location = new System.Drawing.Point(226, 23);
            this.btn_batchUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_batchUpdate.Name = "btn_batchUpdate";
            this.btn_batchUpdate.Size = new System.Drawing.Size(118, 33);
            this.btn_batchUpdate.TabIndex = 3;
            this.btn_batchUpdate.Text = "批量修改";
            this.btn_batchUpdate.UseVisualStyleBackColor = true;
            this.btn_batchUpdate.Click += new System.EventHandler(this.btn_batchUpdate_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "HandleUserIds:";
            // 
            // txb_CurrentUserId
            // 
            this.txb_CurrentUserId.Enabled = false;
            this.txb_CurrentUserId.Location = new System.Drawing.Point(167, 670);
            this.txb_CurrentUserId.Name = "txb_CurrentUserId";
            this.txb_CurrentUserId.Size = new System.Drawing.Size(527, 27);
            this.txb_CurrentUserId.TabIndex = 27;
            this.txb_CurrentUserId.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 673);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 20);
            this.label2.TabIndex = 25;
            this.label2.Text = "正在执行的UserId：";
            // 
            // txb_CurrentAssetIdNums
            // 
            this.txb_CurrentAssetIdNums.Enabled = false;
            this.txb_CurrentAssetIdNums.Location = new System.Drawing.Point(167, 720);
            this.txb_CurrentAssetIdNums.Name = "txb_CurrentAssetIdNums";
            this.txb_CurrentAssetIdNums.Size = new System.Drawing.Size(527, 27);
            this.txb_CurrentAssetIdNums.TabIndex = 28;
            this.txb_CurrentAssetIdNums.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(76, 723);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 26;
            this.label6.Text = "成功数量：";
            // 
            // lbl_showmsg
            // 
            this.lbl_showmsg.AutoSize = true;
            this.lbl_showmsg.Location = new System.Drawing.Point(20, 772);
            this.lbl_showmsg.Name = "lbl_showmsg";
            this.lbl_showmsg.Size = new System.Drawing.Size(43, 20);
            this.lbl_showmsg.TabIndex = 29;
            this.lbl_showmsg.Text = "提示:";
            // 
            // UpdateYemUserProductInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(721, 817);
            this.Controls.Add(this.lbl_showmsg);
            this.Controls.Add(this.txb_CurrentUserId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_CurrentAssetIdNums);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_batchUpdate);
            this.Controls.Add(this.txb_handleUserIds);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "UpdateYemUserProductInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateYemUserProductInfoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txb_handleUserIds;
        private System.Windows.Forms.Button btn_batchUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_CurrentUserId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_CurrentAssetIdNums;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_showmsg;
    }
}