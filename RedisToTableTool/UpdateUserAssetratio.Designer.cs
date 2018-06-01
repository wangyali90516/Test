namespace RedisToTableTool
{
    partial class UpdateUserAssetratio
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
            this.btn_handeluserids = new System.Windows.Forms.Button();
            this.txb_handleUserIds = new System.Windows.Forms.TextBox();
            this.txb_UserNums = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_handeluserids
            // 
            this.btn_handeluserids.Location = new System.Drawing.Point(513, 334);
            this.btn_handeluserids.Name = "btn_handeluserids";
            this.btn_handeluserids.Size = new System.Drawing.Size(75, 23);
            this.btn_handeluserids.TabIndex = 0;
            this.btn_handeluserids.Text = "执行";
            this.btn_handeluserids.UseVisualStyleBackColor = true;
            this.btn_handeluserids.Click += new System.EventHandler(this.button1_Click);
            // 
            // txb_handleUserIds
            // 
            this.txb_handleUserIds.Location = new System.Drawing.Point(13, 39);
            this.txb_handleUserIds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txb_handleUserIds.Multiline = true;
            this.txb_handleUserIds.Name = "txb_handleUserIds";
            this.txb_handleUserIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txb_handleUserIds.Size = new System.Drawing.Size(493, 318);
            this.txb_handleUserIds.TabIndex = 27;
            // 
            // txb_UserNums
            // 
            this.txb_UserNums.Enabled = false;
            this.txb_UserNums.Location = new System.Drawing.Point(117, 405);
            this.txb_UserNums.Name = "txb_UserNums";
            this.txb_UserNums.Size = new System.Drawing.Size(403, 20);
            this.txb_UserNums.TabIndex = 37;
            this.txb_UserNums.Text = "0";
            this.txb_UserNums.TextChanged += new System.EventHandler(this.txb_UserNums_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 403);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 36;
            this.label2.Text = "执行完成数量：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // UpdateUserAssetratio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 523);
            this.Controls.Add(this.txb_UserNums);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txb_handleUserIds);
            this.Controls.Add(this.btn_handeluserids);
            this.Name = "UpdateUserAssetratio";
            this.Text = "UpdateUserAssetratio";
            this.Load += new System.EventHandler(this.UpdateUserAssetratio_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_handeluserids;
        private System.Windows.Forms.TextBox txb_handleUserIds;
        private System.Windows.Forms.TextBox txb_UserNums;
        private System.Windows.Forms.Label label2;
    }
}