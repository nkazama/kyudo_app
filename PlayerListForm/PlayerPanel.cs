using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerListForm
{
    class PlayerPanel : System.Windows.Forms.Panel
    {
        #region Windows フォーム デザイナーで生成されたコード
        public void InitializeComponent(int index)
        {
            int posx = 12;
            int posy = 28 + index * 30;
            //this.player_panel = new System.Windows.Forms.Panel();
            this.player_name = new System.Windows.Forms.TextBox();
            this.playerID = new System.Windows.Forms.TextBox();
            this.comboBox_group = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // player_panel
            // 
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.playerID);
            this.Controls.Add(this.player_name);
            this.Controls.Add(this.comboBox_group);
            this.Location = new System.Drawing.Point(posx, posy);
            this.Name = "player_panel";
            this.Size = new System.Drawing.Size(527, 28);
            this.TabIndex = 0;
            // 
            // player_name
            // 
            this.player_name.Location = new System.Drawing.Point(112, 4);
            this.player_name.Name = "player_name";
            this.player_name.Size = new System.Drawing.Size(200, 19);
            this.player_name.TabIndex = 0;
            // 
            // playerID
            // 
            this.playerID.Location = new System.Drawing.Point(46, 4);
            this.playerID.Name = "playerID";
            this.playerID.Size = new System.Drawing.Size(55, 19);
            this.playerID.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox_group.FormattingEnabled = true;
            this.comboBox_group.Items.AddRange(new object[] {
            "大和市弓道協会",
            "あいうえお",
            "かきくけこ",
            "さしすせそ",
            "たちつてと"});
            this.comboBox_group.Location = new System.Drawing.Point(320, 4);
            this.comboBox_group.Name = "player_group";
            this.comboBox_group.Size = new System.Drawing.Size(200, 19);
            this.comboBox_group.TabIndex = 3;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        public string GetPlayerName()
        {
            return player_name.Text;
        }
        public string GetPlayerID()
        {
            return playerID.Text;
        }

        private System.Windows.Forms.TextBox playerID;
        private System.Windows.Forms.TextBox player_name;
        private System.Windows.Forms.ComboBox comboBox_group;
    }
}
