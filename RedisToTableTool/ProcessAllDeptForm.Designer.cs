namespace RedisToTableTool
{
    partial class ProcessAllDeptForm
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
            this.btn_StartProcess = new System.Windows.Forms.Button();
            this.txb_SuccessNums = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txb_hasProcessNums = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txb_ProcessNums = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txb_RansomOrderId = new System.Windows.Forms.TextBox();
            this.lbl_showmsg = new System.Windows.Forms.Label();
            this.btn_BatchProcess = new System.Windows.Forms.Button();
            this.btn_DoAllThings = new System.Windows.Forms.Button();
            this.btn_Redis = new System.Windows.Forms.Button();
            this.btn_batchReload = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_ShowGrantMsg = new System.Windows.Forms.Label();
            this.txb_GrantNumbers = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txb_endIndex = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txb_startIndex = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Grant = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbl_showZZMsg = new System.Windows.Forms.Label();
            this.txb_ZZNums = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_ZZ = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txb_TotalNums = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_StartProcess
            // 
            this.btn_StartProcess.Location = new System.Drawing.Point(275, 78);
            this.btn_StartProcess.Name = "btn_StartProcess";
            this.btn_StartProcess.Size = new System.Drawing.Size(97, 31);
            this.btn_StartProcess.TabIndex = 0;
            this.btn_StartProcess.Text = "开始执行";
            this.btn_StartProcess.UseVisualStyleBackColor = true;
            this.btn_StartProcess.Visible = false;
            this.btn_StartProcess.Click += new System.EventHandler(this.btn_StartProcess_Click);
            // 
            // txb_SuccessNums
            // 
            this.txb_SuccessNums.Enabled = false;
            this.txb_SuccessNums.Location = new System.Drawing.Point(133, 234);
            this.txb_SuccessNums.Name = "txb_SuccessNums";
            this.txb_SuccessNums.Size = new System.Drawing.Size(368, 27);
            this.txb_SuccessNums.TabIndex = 17;
            this.txb_SuccessNums.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(48, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "成功数量：";
            // 
            // txb_hasProcessNums
            // 
            this.txb_hasProcessNums.Enabled = false;
            this.txb_hasProcessNums.Location = new System.Drawing.Point(133, 185);
            this.txb_hasProcessNums.Name = "txb_hasProcessNums";
            this.txb_hasProcessNums.Size = new System.Drawing.Size(368, 27);
            this.txb_hasProcessNums.TabIndex = 18;
            this.txb_hasProcessNums.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "已经执行数量：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "需要执行总数量：";
            // 
            // txb_ProcessNums
            // 
            this.txb_ProcessNums.Enabled = false;
            this.txb_ProcessNums.Location = new System.Drawing.Point(133, 138);
            this.txb_ProcessNums.Name = "txb_ProcessNums";
            this.txb_ProcessNums.Size = new System.Drawing.Size(368, 27);
            this.txb_ProcessNums.TabIndex = 18;
            this.txb_ProcessNums.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "RansomOrderId：";
            // 
            // txb_RansomOrderId
            // 
            this.txb_RansomOrderId.Location = new System.Drawing.Point(146, 21);
            this.txb_RansomOrderId.Name = "txb_RansomOrderId";
            this.txb_RansomOrderId.Size = new System.Drawing.Size(368, 27);
            this.txb_RansomOrderId.TabIndex = 18;
            // 
            // lbl_showmsg
            // 
            this.lbl_showmsg.AutoSize = true;
            this.lbl_showmsg.Location = new System.Drawing.Point(12, 280);
            this.lbl_showmsg.Name = "lbl_showmsg";
            this.lbl_showmsg.Size = new System.Drawing.Size(43, 20);
            this.lbl_showmsg.TabIndex = 19;
            this.lbl_showmsg.Text = "提示:";
            this.lbl_showmsg.Click += new System.EventHandler(this.lbl_showmsg_Click);
            // 
            // btn_BatchProcess
            // 
            this.btn_BatchProcess.Location = new System.Drawing.Point(392, 78);
            this.btn_BatchProcess.Name = "btn_BatchProcess";
            this.btn_BatchProcess.Size = new System.Drawing.Size(97, 31);
            this.btn_BatchProcess.TabIndex = 0;
            this.btn_BatchProcess.Text = "批量执行";
            this.btn_BatchProcess.UseVisualStyleBackColor = true;
            this.btn_BatchProcess.Visible = false;
            this.btn_BatchProcess.Click += new System.EventHandler(this.btn_BatchProcess_Click);
            // 
            // btn_DoAllThings
            // 
            this.btn_DoAllThings.Location = new System.Drawing.Point(155, 78);
            this.btn_DoAllThings.Name = "btn_DoAllThings";
            this.btn_DoAllThings.Size = new System.Drawing.Size(97, 31);
            this.btn_DoAllThings.TabIndex = 0;
            this.btn_DoAllThings.Text = "开始执行";
            this.btn_DoAllThings.UseVisualStyleBackColor = true;
            this.btn_DoAllThings.Click += new System.EventHandler(this.btn_DoAllThings_Click);
            // 
            // btn_Redis
            // 
            this.btn_Redis.Location = new System.Drawing.Point(506, 78);
            this.btn_Redis.Name = "btn_Redis";
            this.btn_Redis.Size = new System.Drawing.Size(132, 31);
            this.btn_Redis.TabIndex = 0;
            this.btn_Redis.Text = "同步到Redis";
            this.btn_Redis.UseVisualStyleBackColor = true;
            this.btn_Redis.Visible = false;
            this.btn_Redis.Click += new System.EventHandler(this.btn_Redis_Click);
            // 
            // btn_batchReload
            // 
            this.btn_batchReload.Location = new System.Drawing.Point(507, 136);
            this.btn_batchReload.Name = "btn_batchReload";
            this.btn_batchReload.Size = new System.Drawing.Size(132, 31);
            this.btn_batchReload.TabIndex = 0;
            this.btn_batchReload.Text = "批量修改";
            this.btn_batchReload.UseVisualStyleBackColor = true;
            this.btn_batchReload.Visible = false;
            this.btn_batchReload.Click += new System.EventHandler(this.btn_batchReload_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_ShowGrantMsg);
            this.groupBox1.Controls.Add(this.txb_GrantNumbers);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txb_endIndex);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txb_startIndex);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btn_Grant);
            this.groupBox1.Location = new System.Drawing.Point(13, 566);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 227);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "批量放款操作";
            // 
            // lbl_ShowGrantMsg
            // 
            this.lbl_ShowGrantMsg.AutoSize = true;
            this.lbl_ShowGrantMsg.Location = new System.Drawing.Point(41, 179);
            this.lbl_ShowGrantMsg.Name = "lbl_ShowGrantMsg";
            this.lbl_ShowGrantMsg.Size = new System.Drawing.Size(54, 20);
            this.lbl_ShowGrantMsg.TabIndex = 18;
            this.lbl_ShowGrantMsg.Text = "提示：";
            // 
            // txb_GrantNumbers
            // 
            this.txb_GrantNumbers.Enabled = false;
            this.txb_GrantNumbers.Location = new System.Drawing.Point(122, 124);
            this.txb_GrantNumbers.Name = "txb_GrantNumbers";
            this.txb_GrantNumbers.Size = new System.Drawing.Size(397, 27);
            this.txb_GrantNumbers.TabIndex = 17;
            this.txb_GrantNumbers.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "已经执行数量：";
            // 
            // txb_endIndex
            // 
            this.txb_endIndex.Location = new System.Drawing.Point(317, 61);
            this.txb_endIndex.Name = "txb_endIndex";
            this.txb_endIndex.Size = new System.Drawing.Size(99, 27);
            this.txb_endIndex.TabIndex = 17;
            this.txb_endIndex.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(234, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "endIndex：";
            // 
            // txb_startIndex
            // 
            this.txb_startIndex.Location = new System.Drawing.Point(120, 61);
            this.txb_startIndex.Name = "txb_startIndex";
            this.txb_startIndex.Size = new System.Drawing.Size(99, 27);
            this.txb_startIndex.TabIndex = 17;
            this.txb_startIndex.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(37, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "startIndex：";
            // 
            // btn_Grant
            // 
            this.btn_Grant.Location = new System.Drawing.Point(543, 122);
            this.btn_Grant.Name = "btn_Grant";
            this.btn_Grant.Size = new System.Drawing.Size(97, 31);
            this.btn_Grant.TabIndex = 0;
            this.btn_Grant.Text = "开始执行";
            this.btn_Grant.UseVisualStyleBackColor = true;
            this.btn_Grant.Click += new System.EventHandler(this.btn_Grant_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_showZZMsg);
            this.groupBox2.Controls.Add(this.txb_TotalNums);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txb_ZZNums);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btn_ZZ);
            this.groupBox2.Location = new System.Drawing.Point(12, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(674, 227);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "批量债转并确认放款操作";
            // 
            // lbl_showZZMsg
            // 
            this.lbl_showZZMsg.AutoSize = true;
            this.lbl_showZZMsg.Location = new System.Drawing.Point(41, 179);
            this.lbl_showZZMsg.Name = "lbl_showZZMsg";
            this.lbl_showZZMsg.Size = new System.Drawing.Size(54, 20);
            this.lbl_showZZMsg.TabIndex = 18;
            this.lbl_showZZMsg.Text = "提示：";
            // 
            // txb_ZZNums
            // 
            this.txb_ZZNums.Enabled = false;
            this.txb_ZZNums.Location = new System.Drawing.Point(122, 124);
            this.txb_ZZNums.Name = "txb_ZZNums";
            this.txb_ZZNums.Size = new System.Drawing.Size(397, 27);
            this.txb_ZZNums.TabIndex = 17;
            this.txb_ZZNums.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 20);
            this.label9.TabIndex = 15;
            this.label9.Text = "已经执行数量：";
            // 
            // btn_ZZ
            // 
            this.btn_ZZ.Location = new System.Drawing.Point(543, 122);
            this.btn_ZZ.Name = "btn_ZZ";
            this.btn_ZZ.Size = new System.Drawing.Size(97, 31);
            this.btn_ZZ.TabIndex = 0;
            this.btn_ZZ.Text = "开始执行";
            this.btn_ZZ.UseVisualStyleBackColor = true;
            this.btn_ZZ.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 20);
            this.label8.TabIndex = 15;
            this.label8.Text = "需要执行数量：";
            // 
            // txb_TotalNums
            // 
            this.txb_TotalNums.Enabled = false;
            this.txb_TotalNums.Location = new System.Drawing.Point(122, 53);
            this.txb_TotalNums.Name = "txb_TotalNums";
            this.txb_TotalNums.Size = new System.Drawing.Size(397, 27);
            this.txb_TotalNums.TabIndex = 17;
            this.txb_TotalNums.Text = "0";
            // 
            // ProcessAllDeptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(737, 805);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_showmsg);
            this.Controls.Add(this.txb_SuccessNums);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txb_RansomOrderId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_ProcessNums);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txb_hasProcessNums);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_batchReload);
            this.Controls.Add(this.btn_Redis);
            this.Controls.Add(this.btn_BatchProcess);
            this.Controls.Add(this.btn_DoAllThings);
            this.Controls.Add(this.btn_StartProcess);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ProcessAllDeptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProcessAllDeptForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_StartProcess;
        private System.Windows.Forms.TextBox txb_SuccessNums;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txb_hasProcessNums;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txb_ProcessNums;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_RansomOrderId;
        private System.Windows.Forms.Label lbl_showmsg;
        private System.Windows.Forms.Button btn_BatchProcess;
        private System.Windows.Forms.Button btn_DoAllThings;
        private System.Windows.Forms.Button btn_Redis;
        private System.Windows.Forms.Button btn_batchReload;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txb_GrantNumbers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Grant;
        private System.Windows.Forms.TextBox txb_endIndex;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txb_startIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_ShowGrantMsg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbl_showZZMsg;
        private System.Windows.Forms.TextBox txb_ZZNums;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txb_zhaiEIndex;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txb_zhaiSIndex;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_ZZ;
        private System.Windows.Forms.TextBox txb_TotalNums;
        private System.Windows.Forms.Label label8;
    }
}