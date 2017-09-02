namespace WindowsFormsApplication1
{
    partial class StartScreen
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.MaxPlayerNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.start_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPlayerNum)).BeginInit();
            this.SuspendLayout();
            // 
            // MaxPlayerNum
            // 
            this.MaxPlayerNum.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.MaxPlayerNum.Location = new System.Drawing.Point(124, 39);
            this.MaxPlayerNum.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.MaxPlayerNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxPlayerNum.Name = "MaxPlayerNum";
            this.MaxPlayerNum.Size = new System.Drawing.Size(41, 22);
            this.MaxPlayerNum.TabIndex = 0;
            this.MaxPlayerNum.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.label1.Location = new System.Drawing.Point(51, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "人数";
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(149, 169);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(117, 27);
            this.start_button.TabIndex = 2;
            this.start_button.Text = "STRAT";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // StartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 217);
            this.Controls.Add(this.start_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MaxPlayerNum);
            this.Name = "StartScreen";
            this.Text = "StartScreen";
            ((System.ComponentModel.ISupportInitialize)(this.MaxPlayerNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown MaxPlayerNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button start_button;
    }
}