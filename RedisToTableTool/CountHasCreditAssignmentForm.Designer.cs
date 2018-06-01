namespace RedisToTableTool
{
    partial class CountHasCreditAssignmentForm
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
            this.btn_StartComputer = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_notCreditNums = new System.Windows.Forms.TextBox();
            this.txb_AllNumbers = new System.Windows.Forms.TextBox();
            this.txb_CreditNotifyFNums = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_CreditNotifySNums = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_showMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_StartComputer
            // 
            this.btn_StartComputer.Location = new System.Drawing.Point(199, 254);
            this.btn_StartComputer.Name = "btn_StartComputer";
            this.btn_StartComputer.Size = new System.Drawing.Size(153, 29);
            this.btn_StartComputer.TabIndex = 0;
            this.btn_StartComputer.Text = "StartCompute";
            this.btn_StartComputer.UseVisualStyleBackColor = true;
            this.btn_StartComputer.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "从未发生过债转数量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "总共需要分类计算的数量：";
            // 
            // txb_notCreditNums
            // 
            this.txb_notCreditNums.Enabled = false;
            this.txb_notCreditNums.Location = new System.Drawing.Point(199, 88);
            this.txb_notCreditNums.Name = "txb_notCreditNums";
            this.txb_notCreditNums.Size = new System.Drawing.Size(368, 27);
            this.txb_notCreditNums.TabIndex = 5;
            this.txb_notCreditNums.Text = "0";
            // 
            // txb_AllNumbers
            // 
            this.txb_AllNumbers.Enabled = false;
            this.txb_AllNumbers.Location = new System.Drawing.Point(199, 33);
            this.txb_AllNumbers.Name = "txb_AllNumbers";
            this.txb_AllNumbers.Size = new System.Drawing.Size(368, 27);
            this.txb_AllNumbers.TabIndex = 6;
            this.txb_AllNumbers.Text = "0";
            // 
            // txb_CreditNotifyFNums
            // 
            this.txb_CreditNotifyFNums.Enabled = false;
            this.txb_CreditNotifyFNums.Location = new System.Drawing.Point(199, 145);
            this.txb_CreditNotifyFNums.Name = "txb_CreditNotifyFNums";
            this.txb_CreditNotifyFNums.Size = new System.Drawing.Size(368, 27);
            this.txb_CreditNotifyFNums.TabIndex = 5;
            this.txb_CreditNotifyFNums.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "发生过债转通知银行成功：";
            // 
            // txb_CreditNotifySNums
            // 
            this.txb_CreditNotifySNums.Enabled = false;
            this.txb_CreditNotifySNums.Location = new System.Drawing.Point(199, 197);
            this.txb_CreditNotifySNums.Name = "txb_CreditNotifySNums";
            this.txb_CreditNotifySNums.Size = new System.Drawing.Size(368, 27);
            this.txb_CreditNotifySNums.TabIndex = 5;
            this.txb_CreditNotifySNums.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(30, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "发生过债转未通知银行：";
            // 
            // lbl_showMsg
            // 
            this.lbl_showMsg.AutoSize = true;
            this.lbl_showMsg.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_showMsg.ForeColor = System.Drawing.Color.Red;
            this.lbl_showMsg.Location = new System.Drawing.Point(16, 315);
            this.lbl_showMsg.Name = "lbl_showMsg";
            this.lbl_showMsg.Size = new System.Drawing.Size(41, 19);
            this.lbl_showMsg.TabIndex = 7;
            this.lbl_showMsg.Text = "提示:";
            // 
            // CountHasCreditAssignmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(612, 534);
            this.Controls.Add(this.lbl_showMsg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_CreditNotifySNums);
            this.Controls.Add(this.txb_CreditNotifyFNums);
            this.Controls.Add(this.txb_notCreditNums);
            this.Controls.Add(this.txb_AllNumbers);
            this.Controls.Add(this.btn_StartComputer);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "CountHasCreditAssignmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CountHasCreditAssignmentForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_StartComputer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_notCreditNums;
        private System.Windows.Forms.TextBox txb_AllNumbers;
        private System.Windows.Forms.TextBox txb_CreditNotifyFNums;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_CreditNotifySNums;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_showMsg;
    }
}