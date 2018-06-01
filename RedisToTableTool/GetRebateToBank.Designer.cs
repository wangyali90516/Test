namespace RedisToTableTool
{
    partial class GetRebateToBank
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
            this.lbl_showmsg = new System.Windows.Forms.Label();
            this.txb_UserNums = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_SuccessNums = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_HandleThings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_handleUserIds = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_showmsg
            // 
            this.lbl_showmsg.AutoSize = true;
            this.lbl_showmsg.Location = new System.Drawing.Point(23, 690);
            this.lbl_showmsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_showmsg.Name = "lbl_showmsg";
            this.lbl_showmsg.Size = new System.Drawing.Size(34, 13);
            this.lbl_showmsg.TabIndex = 53;
            this.lbl_showmsg.Text = "提示:";
            // 
            // txb_UserNums
            // 
            this.txb_UserNums.Enabled = false;
            this.txb_UserNums.Location = new System.Drawing.Point(120, 598);
            this.txb_UserNums.Margin = new System.Windows.Forms.Padding(4);
            this.txb_UserNums.Name = "txb_UserNums";
            this.txb_UserNums.Size = new System.Drawing.Size(610, 20);
            this.txb_UserNums.TabIndex = 51;
            this.txb_UserNums.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 598);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 49;
            this.label2.Text = "用户总数量：";
            // 
            // txb_SuccessNums
            // 
            this.txb_SuccessNums.Enabled = false;
            this.txb_SuccessNums.Location = new System.Drawing.Point(120, 641);
            this.txb_SuccessNums.Margin = new System.Windows.Forms.Padding(4);
            this.txb_SuccessNums.Name = "txb_SuccessNums";
            this.txb_SuccessNums.Size = new System.Drawing.Size(614, 20);
            this.txb_SuccessNums.TabIndex = 52;
            this.txb_SuccessNums.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 646);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 50;
            this.label6.Text = "已经执行数量：";
            // 
            // btn_HandleThings
            // 
            this.btn_HandleThings.Location = new System.Drawing.Point(155, 15);
            this.btn_HandleThings.Margin = new System.Windows.Forms.Padding(4);
            this.btn_HandleThings.Name = "btn_HandleThings";
            this.btn_HandleThings.Size = new System.Drawing.Size(103, 28);
            this.btn_HandleThings.TabIndex = 48;
            this.btn_HandleThings.Text = "开始执行";
            this.btn_HandleThings.UseVisualStyleBackColor = true;
            this.btn_HandleThings.Click += new System.EventHandler(this.btn_HandleThings_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "HandleUserIds:";
            // 
            // txb_handleUserIds
            // 
            this.txb_handleUserIds.Location = new System.Drawing.Point(14, 54);
            this.txb_handleUserIds.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txb_handleUserIds.Multiline = true;
            this.txb_handleUserIds.Name = "txb_handleUserIds";
            this.txb_handleUserIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txb_handleUserIds.Size = new System.Drawing.Size(716, 522);
            this.txb_handleUserIds.TabIndex = 46;
            // 
            // GetRebateToBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 724);
            this.Controls.Add(this.lbl_showmsg);
            this.Controls.Add(this.txb_UserNums);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_SuccessNums);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_HandleThings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txb_handleUserIds);
            this.Name = "GetRebateToBank";
            this.Text = "GetRebateToBank";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_showmsg;
        private System.Windows.Forms.TextBox txb_UserNums;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_SuccessNums;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_HandleThings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_handleUserIds;
    }
}