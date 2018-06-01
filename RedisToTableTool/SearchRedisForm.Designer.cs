namespace RedisToTableTool
{
    partial class SearchRedisForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.txb_RedisNumber = new System.Windows.Forms.TextBox();
            this.btn_checkAllAsset = new System.Windows.Forms.Button();
            this.txb_RedisKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "RedisNumber:";
            // 
            // txb_RedisNumber
            // 
            this.txb_RedisNumber.Location = new System.Drawing.Point(120, 22);
            this.txb_RedisNumber.Name = "txb_RedisNumber";
            this.txb_RedisNumber.Size = new System.Drawing.Size(368, 27);
            this.txb_RedisNumber.TabIndex = 8;
            this.txb_RedisNumber.Text = "0";
            // 
            // btn_checkAllAsset
            // 
            this.btn_checkAllAsset.Location = new System.Drawing.Point(305, 167);
            this.btn_checkAllAsset.Name = "btn_checkAllAsset";
            this.btn_checkAllAsset.Size = new System.Drawing.Size(183, 29);
            this.btn_checkAllAsset.TabIndex = 6;
            this.btn_checkAllAsset.Text = "ReadRedisContent";
            this.btn_checkAllAsset.UseVisualStyleBackColor = true;
            this.btn_checkAllAsset.Click += new System.EventHandler(this.btn_checkAllAsset_Click);
            // 
            // txb_RedisKey
            // 
            this.txb_RedisKey.Location = new System.Drawing.Point(120, 95);
            this.txb_RedisKey.Name = "txb_RedisKey";
            this.txb_RedisKey.Size = new System.Drawing.Size(368, 27);
            this.txb_RedisKey.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "RedisKeyName:";
            // 
            // SearchRedisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(534, 340);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txb_RedisKey);
            this.Controls.Add(this.txb_RedisNumber);
            this.Controls.Add(this.btn_checkAllAsset);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "SearchRedisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询Redis内容";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txb_RedisNumber;
        private System.Windows.Forms.Button btn_checkAllAsset;
        private System.Windows.Forms.TextBox txb_RedisKey;
        private System.Windows.Forms.Label label1;
    }
}