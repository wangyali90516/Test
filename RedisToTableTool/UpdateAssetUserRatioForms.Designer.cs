namespace RedisToTableTool
{
    partial class UpdateAssetUserRatioForms
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
            this.label3 = new System.Windows.Forms.Label();
            this.txb_updateTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_showmsg
            // 
            this.lbl_showmsg.AutoSize = true;
            this.lbl_showmsg.Location = new System.Drawing.Point(24, 675);
            this.lbl_showmsg.Name = "lbl_showmsg";
            this.lbl_showmsg.Size = new System.Drawing.Size(43, 20);
            this.lbl_showmsg.TabIndex = 37;
            this.lbl_showmsg.Text = "提示:";
            this.lbl_showmsg.Click += new System.EventHandler(this.lbl_showmsg_Click);
            // 
            // txb_UserNums
            // 
            this.txb_UserNums.Enabled = false;
            this.txb_UserNums.Location = new System.Drawing.Point(123, 576);
            this.txb_UserNums.Name = "txb_UserNums";
            this.txb_UserNums.Size = new System.Drawing.Size(527, 27);
            this.txb_UserNums.TabIndex = 35;
            this.txb_UserNums.Text = "0";
            this.txb_UserNums.TextChanged += new System.EventHandler(this.txb_UserNums_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 579);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 31;
            this.label2.Text = "用户总数量：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txb_SuccessNums
            // 
            this.txb_SuccessNums.Enabled = false;
            this.txb_SuccessNums.Location = new System.Drawing.Point(123, 626);
            this.txb_SuccessNums.Name = "txb_SuccessNums";
            this.txb_SuccessNums.Size = new System.Drawing.Size(527, 27);
            this.txb_SuccessNums.TabIndex = 36;
            this.txb_SuccessNums.Text = "0";
            this.txb_SuccessNums.TextChanged += new System.EventHandler(this.txb_SuccessNums_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 629);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 32;
            this.label6.Text = "已经执行数量：";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // btn_HandleThings
            // 
            this.btn_HandleThings.Location = new System.Drawing.Point(151, 12);
            this.btn_HandleThings.Name = "btn_HandleThings";
            this.btn_HandleThings.Size = new System.Drawing.Size(97, 25);
            this.btn_HandleThings.TabIndex = 28;
            this.btn_HandleThings.Text = "开始执行";
            this.btn_HandleThings.UseVisualStyleBackColor = true;
            this.btn_HandleThings.Click += new System.EventHandler(this.btn_HandleThings_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 27;
            this.label1.Text = "HandleUserIds:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txb_handleUserIds
            // 
            this.txb_handleUserIds.Location = new System.Drawing.Point(18, 42);
            this.txb_handleUserIds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txb_handleUserIds.Multiline = true;
            this.txb_handleUserIds.Name = "txb_handleUserIds";
            this.txb_handleUserIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txb_handleUserIds.Size = new System.Drawing.Size(678, 446);
            this.txb_handleUserIds.TabIndex = 26;
            this.txb_handleUserIds.TextChanged += new System.EventHandler(this.txb_handleUserIds_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 523);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 31;
            this.label3.Text = "打款时间：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txb_updateTime
            // 
            this.txb_updateTime.Location = new System.Drawing.Point(123, 520);
            this.txb_updateTime.Name = "txb_updateTime";
            this.txb_updateTime.Size = new System.Drawing.Size(527, 27);
            this.txb_updateTime.TabIndex = 35;
            this.txb_updateTime.TextChanged += new System.EventHandler(this.txb_updateTime_TextChanged);
            // 
            // UpdateAssetUserRatioForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(705, 720);
            this.Controls.Add(this.lbl_showmsg);
            this.Controls.Add(this.txb_updateTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txb_UserNums);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_SuccessNums);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_HandleThings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txb_handleUserIds);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UpdateAssetUserRatioForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateAssetUserRatioForms";
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txb_updateTime;
    }
}