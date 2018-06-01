namespace RedisToTableTool
{
    partial class ComputerUserTotalMoneyForm
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
            this.btn_CUserTotalMoney = new System.Windows.Forms.Button();
            this.txb_UserNums = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_userId = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_CUserTotalMoney
            // 
            this.btn_CUserTotalMoney.Location = new System.Drawing.Point(293, 11);
            this.btn_CUserTotalMoney.Name = "btn_CUserTotalMoney";
            this.btn_CUserTotalMoney.Size = new System.Drawing.Size(81, 26);
            this.btn_CUserTotalMoney.TabIndex = 0;
            this.btn_CUserTotalMoney.Text = "开始计算";
            this.btn_CUserTotalMoney.UseVisualStyleBackColor = true;
            this.btn_CUserTotalMoney.Click += new System.EventHandler(this.btn_CUserTotalMoney_Click);
            // 
            // txb_UserNums
            // 
            this.txb_UserNums.Enabled = false;
            this.txb_UserNums.Location = new System.Drawing.Point(115, 12);
            this.txb_UserNums.Name = "txb_UserNums";
            this.txb_UserNums.Size = new System.Drawing.Size(172, 25);
            this.txb_UserNums.TabIndex = 37;
            this.txb_UserNums.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 36;
            this.label2.Text = "已执行总数量：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 28);
            this.button1.TabIndex = 38;
            this.button1.Text = "获取流水";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "用户Id：";
            // 
            // txb_userId
            // 
            this.txb_userId.Location = new System.Drawing.Point(104, 113);
            this.txb_userId.Name = "txb_userId";
            this.txb_userId.Size = new System.Drawing.Size(183, 25);
            this.txb_userId.TabIndex = 37;
            // 
            // ComputerUserTotalMoneyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(821, 721);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txb_userId);
            this.Controls.Add(this.txb_UserNums);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_CUserTotalMoney);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ComputerUserTotalMoneyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算用户总资产";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_CUserTotalMoney;
        private System.Windows.Forms.TextBox txb_UserNums;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_userId;
    }
}