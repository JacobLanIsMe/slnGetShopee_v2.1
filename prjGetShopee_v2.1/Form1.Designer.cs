
namespace prjGetShopee_v2._1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSmallType = new System.Windows.Forms.Button();
            this.btnGetPanasonic = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnGetAllItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSmallType
            // 
            this.btnSmallType.Location = new System.Drawing.Point(30, 386);
            this.btnSmallType.Name = "btnSmallType";
            this.btnSmallType.Size = new System.Drawing.Size(254, 82);
            this.btnSmallType.TabIndex = 0;
            this.btnSmallType.Text = "btnSmallType";
            this.btnSmallType.UseVisualStyleBackColor = true;
            this.btnSmallType.Click += new System.EventHandler(this.btnSmallType_Click);
            // 
            // btnGetPanasonic
            // 
            this.btnGetPanasonic.Location = new System.Drawing.Point(30, 279);
            this.btnGetPanasonic.Name = "btnGetPanasonic";
            this.btnGetPanasonic.Size = new System.Drawing.Size(254, 82);
            this.btnGetPanasonic.TabIndex = 1;
            this.btnGetPanasonic.Text = "GetPanasonic";
            this.btnGetPanasonic.UseVisualStyleBackColor = true;
            this.btnGetPanasonic.Click += new System.EventHandler(this.btnGetPanasonic_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 24;
            this.listBox1.Location = new System.Drawing.Point(324, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1420, 940);
            this.listBox1.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(30, 583);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(196, 45);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "DeleteSpecificData";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnGetAllItem
            // 
            this.btnGetAllItem.Location = new System.Drawing.Point(30, 164);
            this.btnGetAllItem.Name = "btnGetAllItem";
            this.btnGetAllItem.Size = new System.Drawing.Size(254, 82);
            this.btnGetAllItem.TabIndex = 4;
            this.btnGetAllItem.Text = "GetAllItem";
            this.btnGetAllItem.UseVisualStyleBackColor = true;
            this.btnGetAllItem.Click += new System.EventHandler(this.btnGetAllItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1829, 993);
            this.Controls.Add(this.btnGetAllItem);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnGetPanasonic);
            this.Controls.Add(this.btnSmallType);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSmallType;
        private System.Windows.Forms.Button btnGetPanasonic;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnGetAllItem;
    }
}

