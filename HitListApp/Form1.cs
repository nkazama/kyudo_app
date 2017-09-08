using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace HitListApp
{
    public partial class Form1 : Form
    {
        const string TAG_RESULT = "result";
        const string TAG_NAME = "name";
        const string TAG_ID = "number";

        const string FILENAME_RESULT = "result_data\\Result.csv";
        const string FILENAME_OPTION = "result_data\\Option.csv";
        const string FILENAME_PLAYERLIST = "result_data\\PlayerList.csv";

        const int MAX_PLAYER_NUM = 6;
        const int MAX_PLAY_NUM = 4;
        DisplayDataList[] mDisplayDataList;
        PlayerListAL mPlayerList;

        int mTurnNumber;
        int mCurrentTurn;

        int mDisplayPlayNum;
        int mCurrent1stPlayer;

        const int PANEL_TOP = 20;
        const int PANEL_HEIGHT = 44;
        const int PANEL_HEIGHT_BORDER = 10;
        const int PANEL_NEXT_BORDER = 50;

        public int GetPanelPosY(int tag){ return PANEL_TOP + PANEL_HEIGHT * tag; }
        public int GetPanelIDfromPos(int posy) { return ( posy - PANEL_TOP ) / PANEL_HEIGHT; }
        public int GetPanelNextBorder() { return PANEL_NEXT_BORDER + PANEL_TOP + mDisplayPlayNum * PANEL_HEIGHT; }

        public Form1()
        {
            InitializeComponent();
            mDisplayDataList = new DisplayDataList[MAX_PLAYER_NUM];
            for (int i = 0; i < MAX_PLAYER_NUM; i++)
                mDisplayDataList[i] = new DisplayDataList();

            mPlayerList = new PlayerListAL();

            //First Boot this tool
            if (!System.IO.File.Exists(FILENAME_RESULT))
            {
                SetStartOption();
            }
            mTurnNumber = (int)numericUpDown_TurnNum.Value;

            InitalizeAll();
        }

# region Mouse Status
        //マウスのクリック位置を記憶
        private Point mousePoint;

        //マウスのボタンが押されたとき
        private void panel_list_MouseDown(object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
                Panel panel = (Panel)sender;
                panel.BringToFront();
            }
        }

        //マウスのボタンが離れたとき
        private void panel_list_MouseUp(object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Panel panel = (Panel)sender;
                panel.Top += e.Y - mousePoint.Y;
                //タグ情報を変更
                string s = (string)panel.Tag;
                int old_tag = Int32.Parse(s);
                int current_posY = panel.Top + PANEL_HEIGHT / 2;
                if (GetPanelNextBorder() > current_posY)
                {
                    //@@@ action 次の立ちに移動
                    //return;
                }
                int new_tag = GetPanelIDfromPos(current_posY);
                if (new_tag < 0 || new_tag >= mDisplayPlayNum || new_tag == old_tag)
                {
                    //reset the Y position of the target panel
                    panel.Top = GetPanelPosY(old_tag);
                    return;
                }

                // 移動したパネルのタグ変換
                int[] change_tag_id = { -1, -1, -1, -1, -1, -1 };
                change_tag_id[old_tag] = new_tag;
                for (int i = 0; i < MAX_PLAYER_NUM; i++)
                {
                    if (change_tag_id[i] != -1) continue;

                    int buf = 0;
                    if (old_tag < new_tag)
                    {
                        if (i > new_tag) buf++;
                        if (i >= old_tag) buf--;
                    }
                    else
                    {
                        if (i >= new_tag) buf++;
                        if (i > old_tag) buf--;
                    }
                    change_tag_id[i] = i + buf;
                }

                panel_list_ResetAllPosition(change_tag_id);
            }
        }

        private void panel_list_ResetAllPosition(int[] change_tag)
        {
            //全パネルのタグ変換。ポジション整列
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(Panel))
                {
                    Panel _panel = (Panel)obj_all;
                    string s = (string)_panel.Tag;
                    int old_tag = Int32.Parse(s);

                    if ( old_tag >= change_tag.Length ) continue;
                    
                    int new_tag = change_tag[old_tag];
                    _panel.Tag = new_tag.ToString();
                    _panel.Top = GetPanelPosY(new_tag);
                }
            }

            DisplayDataList[] displayDataList = mDisplayDataList;
            for (int i = 0; i < mDisplayPlayNum; i++)
            {
                mDisplayDataList[i] = displayDataList[change_tag[i]];
            }
        }

        //マウスが動いたとき
        private void panel_list_MouseMove(object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Panel panel = (Panel)sender;
                //panel.Left += e.X - mousePoint.X;
                panel.Top += e.Y - mousePoint.Y;
            }
        }
#endregion

        private void SetStartOption()
        {
            mCurrent1stPlayer = 1;
            mCurrentTurn = 1;
            StartOptionScreen();
            SaveData_FromCSVOption();
        }
        private void StartOptionScreen()
        {
            StartScreen mOptionData = new StartScreen();
            mOptionData.ShowDialog(this);
            mDisplayPlayNum = mOptionData.GetOption_DisplayNum();
            mOptionData.Dispose();
        }

        private void InitalizeAll()
        {
            LoadData_FromCSVOption();
            LoadData_FromCSVPlayerList();

            numericUpDown_TurnNum.Value = mCurrentTurn;

            InitalizePlayer();
        }
        private void InitalizePlayer()
        {
            ResetAllDisplay();
            if (mTurnNumber < mCurrentTurn) LoadData_FromCSVResult();
            else NextTurnDisplay();
            SetCurrentTurn();
        }
        private void SetCurrentTurn()
        {
            next_player_button.Enabled = false;
            if (mTurnNumber == mCurrentTurn)
            {
                next_player_button.Enabled = true;
                highlight_label.Text = "今の立";
            }
            else if (mTurnNumber == mCurrentTurn+1)
            {
                highlight_label.Text = "次の立";
            }
            else
            {
                highlight_label.Text = "";
            }
        }
        private void ResetAllDisplay()
        {
            Panel group = null;
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(Panel))
                {
                    group = (Panel)obj_all;
                    string s = (string)group.Tag;
                    int display_count = Int32.Parse(s);
                    if (display_count >= mDisplayPlayNum)
                    {
                        group_all.Controls.Remove(group);
                    }
                    
                    SetLabel(TAG_ID, "None", group);
                    SetLabel(TAG_NAME, "None", group);
                    foreach (object obj in group.Controls)
                    {
                        if (obj.GetType() == typeof(Button))
                        {
                            Button button = obj as Button;
                            string tag_s = (string)button.Tag;
                            int tag = Int32.Parse(tag_s);
                            mDisplayDataList[display_count].SetCheckButton(tag, (int)CheckType.CHECK_NONE);
                            button.BackgroundImage = GetButtonBG(mDisplayDataList[display_count].GetCheckButton(tag));
                        }
                    }
                    SetLabel(TAG_RESULT, "None", group);
                }
            }
            int[] tag_num = { 0, 1, 2, 3, 4, 5, 6 };
            panel_list_ResetAllPosition(tag_num);
        }

#region Save Datas
        private void SaveData_FromCSVOption()
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FILENAME_OPTION, false, System.Text.Encoding.GetEncoding("shift_jis"));
            sw.WriteLine("CurrentTurn" + "," + mCurrentTurn);
            sw.WriteLine("DisplayPlayerNum" + "," + mDisplayPlayNum);
            sw.WriteLine("NextPlayer" + "," + mCurrent1stPlayer);
            sw.Close();
        }
        private void SaveData_FromCSVResult()
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FILENAME_RESULT, true, System.Text.Encoding.GetEncoding("shift_jis"));
            
            for (int i = 0; i < mDisplayPlayNum; i++ )
            {
                    int player = mDisplayDataList[i].GetPlayerID();
                    int result_1 = (int)mDisplayDataList[i].GetCheckButton(0);
                    int result_2 = (int)mDisplayDataList[i].GetCheckButton(1);
                    int result_3 = (int)mDisplayDataList[i].GetCheckButton(2);
                    int result_4 = (int)mDisplayDataList[i].GetCheckButton(3);
                    int result_total = (int)mDisplayDataList[i].GetTotalResult();
                    string ss_result_total = result_total.ToString();
                    if(mDisplayDataList[i].IsVacancie())
                    {
                        ss_result_total = "欠";
                    }
                    sw.WriteLine(mCurrentTurn + "," + player + "," + result_1 + "," + result_2 + "," + result_3 + "," + result_4 + "," + ss_result_total);
            }
            sw.Close();
        }
        private void SaveVacancieData_ToCSVResult(int tag)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FILENAME_RESULT, true, System.Text.Encoding.GetEncoding("shift_jis"));
            
            int player = mDisplayDataList[tag].GetPlayerID();
            int result_1 = (int)mDisplayDataList[tag].GetCheckButton(0);
            int result_2 = (int)mDisplayDataList[tag].GetCheckButton(1);
            int result_3 = (int)mDisplayDataList[tag].GetCheckButton(2);
            int result_4 = (int)mDisplayDataList[tag].GetCheckButton(3);
            string ss_result_total = ss_result_total = "欠詰";
            sw.WriteLine(mCurrentTurn + "," + player + "," + result_1 + "," + result_2 + "," + result_3 + "," + result_4 + "," + ss_result_total);
            sw.Close();
        }
        private void SaveData_ToCSVPlayerList()
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(FILENAME_PLAYERLIST, false, System.Text.Encoding.GetEncoding("shift_jis"));
            foreach (PlayerList list in mPlayerList)
            {
                sw.WriteLine(list.mID + "," + list.mName + "," + list.GetMemberVacancieType());
            }
            sw.Close();
        }

        #endregion

        #region Load Datas
        private void LoadData_FromCSVOption()
        {
            string text = "";
            using (StreamReader sr = new StreamReader(FILENAME_OPTION, Encoding.GetEncoding("Shift_JIS")))
            {
                while ((text = sr.ReadLine()) != null)
                {
                    string[] stArrayData = text.Split(',');
                    string title = stArrayData[0];
                    int value = Int32.Parse(stArrayData[1]);
                    switch (title)
                    {
                        case "CurrentTurn":
                            mCurrentTurn = value;
                            break;
                        case "DisplayPlayerNum":
                            mDisplayPlayNum = value;
                            break;
                        case "NextPlayer":
                            mCurrent1stPlayer = value;
                            break;
                    }
                }
            }
        }
        private void LoadData_FromCSVPlayerList()
        {
            string text = "";
            using (StreamReader sr = new StreamReader(FILENAME_PLAYERLIST, Encoding.GetEncoding("Shift_JIS")))
            {
                while ((text = sr.ReadLine()) != null)
                {
                    string[] stArrayData = text.Split(',');
                    PlayerList list = new PlayerList( Int32.Parse(stArrayData[0]), stArrayData[1] );
                    if(stArrayData.Length >= 3)
                    {
                        if (stArrayData[2] == PlayerList.GetVacancieType(1) ) list.mVacancieType = 1;
                        else if (stArrayData[2] == PlayerList.GetVacancieType(2)) list.mVacancieType = 2;
                    }
                    mPlayerList.Add(list);
                }
            }
        }
        private void LoadData_FromCSVResult()
        {
            string text = "";
            using (StreamReader sr = new StreamReader(FILENAME_RESULT, Encoding.GetEncoding("Shift_JIS")))
            {
                int display_count = 0;
                while (( text = sr.ReadLine()) != null)
                {
                    string[] stArrayData = text.Split(',');
                    if (Int32.Parse(stArrayData[0]) == mTurnNumber)
                    {
                        int player_id = Int32.Parse(stArrayData[1]);
                        string player_name = mPlayerList.SearchPlayerName(player_id);
                        int[] result = {Int32.Parse(stArrayData[2]),
                                       Int32.Parse(stArrayData[3]),
                                       Int32.Parse(stArrayData[4]),
                                       Int32.Parse(stArrayData[5])
                                       };
                        bool isVacancie = (stArrayData[6] == "欠");
                        LoadData(display_count, player_id, player_name, result, isVacancie);
                        display_count++;
                    }
                }
            }
        }
        private void LoadData(int display_count, int player_id, string player_name, int[] result, bool isVacancie)
        {
            Panel group = null;
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(Panel))
                {
                    group = (Panel)obj_all;
                    string s = (string)group.Tag;
                    if (display_count == Int32.Parse(s)) break;
                }
            }

            mDisplayDataList[display_count].SetPlayerID(player_id);

            SetLabel(TAG_ID, player_id.ToString(), group);
            SetLabel(TAG_NAME, player_name, group);
            
            foreach (object obj in group.Controls)
            {
                if (obj.GetType() == typeof(Button))
                {
                    Button button = obj as Button;
                    string tag_s = (string)button.Tag;
                    int tag = Int32.Parse(tag_s);
                    mDisplayDataList[display_count].SetCheckButton(tag, result[tag]);
                    button.BackgroundImage = GetButtonBG(mDisplayDataList[display_count].GetCheckButton(tag));
                   
                }
            }

            string ss_result = mDisplayDataList[display_count].GetTotalResult().ToString();
            if (isVacancie) ss_result = "欠";
            SetLabel(TAG_RESULT, ss_result, group);
        }
#endregion

        private void NextTurnDisplay()
        {
            Panel group = null;
            int player_num = mCurrent1stPlayer + (mTurnNumber - mCurrentTurn) * mDisplayPlayNum;
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(Panel))
                {
                    group = (Panel)obj_all;
                    string s = (string)group.Tag;
                    int display_count = Int32.Parse(s);

                    int player_id = player_num + display_count;
                    while( player_id < mPlayerList.Count )
                    {
                        if (mPlayerList.SearchVacancieType(player_id) == "") break;
                        player_id++;
                    }
                    mDisplayDataList[display_count].SetPlayerID(player_id);
                    SetLabel(TAG_ID, mPlayerList[player_id - 1, 0].ToString(), group);
                    SetLabel(TAG_NAME, mPlayerList[player_id - 1, 1].ToString(), group);
                }
            }
        }

        private void checkbutton_click(object sender, EventArgs e)
        {
            int display_count = 0;
            Button button = (Button)sender;
            object obj = (Object)button.Parent;
            Panel group = null;
            if (obj.GetType() == typeof(Panel))
            {
                group = (Panel)obj;
                string s = (string)group.Tag;
                display_count = Int32.Parse(s);
            }

            string ss = (string)button.Tag;
            int play_count = Int32.Parse(ss);

            mDisplayDataList[display_count].ClickCheckButton(play_count);
            button.BackgroundImage = GetButtonBG(mDisplayDataList[display_count].GetCheckButton(play_count));

            SetLabel(TAG_RESULT, mDisplayDataList[display_count].GetTotalResult().ToString(), group);
        }

        private void SetLabel(string tag, string value, Panel group)
        {
            foreach (Control c in group.Controls)
            {
                if ((string)c.Tag == tag)
                {
                    Label result_label = (Label)c;
                    result_label.Text = value;
                    break;
                }
            }
        }

        private System.Drawing.Image GetButtonBG(CheckType type)
        {
            if (type == CheckType.CHECK_NONE)
            {
                return System.Drawing.Image.FromFile("image\\none.jpg");
            }
            else if (type == CheckType.CHECK_MARU)
            {
                return System.Drawing.Image.FromFile("image\\maru.jpg");
            }
            else
            {
                return System.Drawing.Image.FromFile("image\\batu.jpg");
            }
        }

        private void Changed_TurmNum(object sender, EventArgs e)
        {
            mTurnNumber = (int)numericUpDown_TurnNum.Value;
            InitalizePlayer();
        }

        private void Click_NextTurn(object sender, EventArgs e)
        {
            SaveData_FromCSVResult();
            SaveData_ToCSVPlayerList();
            mCurrentTurn++;
            mCurrent1stPlayer += mDisplayPlayNum; 
            numericUpDown_TurnNum.Value++;

            SaveData_FromCSVOption();
        }

        private Control contextMenuSourceControl = null;

        //ContextMenuStrip1のOpenedイベントハンドラ
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            ContextMenuStrip menu = (ContextMenuStrip)sender;
            contextMenuSourceControl = menu.SourceControl;
        }

        private void ToolStripMenuItem_Click_Vacancies_KeepPos(object sender, EventArgs e)
        {
            Panel panel = (Panel)contextMenuSourceControl;
            panel.Enabled = false;
            string s = (string)panel.Tag;
            int display_count = Int32.Parse(s);
            mDisplayDataList[display_count].SetVacancie();
            mPlayerList.SearchAndSetVacancieType(mDisplayDataList[display_count].GetPlayerID(), 1);
        }

        private void 欠員指定を解除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)contextMenuSourceControl;
            panel.Enabled = true;
        }

        private void 全ての欠員指定を解除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(Panel))
                {
                    Panel panel = (Panel)obj_all;
                    panel.Enabled = true;
                    string s = (string)panel.Tag;
                    int display_count = Int32.Parse(s);
                    mDisplayDataList[display_count].ResetVacancie();

                    mPlayerList.SearchAndSetVacancieType(mDisplayDataList[display_count].GetPlayerID(), 0);
                }
            }
        }

        private void 削除して詰めるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)contextMenuSourceControl;
            string s = (string)panel.Tag;
            int display_count = Int32.Parse(s);
            SaveVacancieData_ToCSVResult(display_count);
            mPlayerList.SearchAndSetVacancieType(mDisplayDataList[display_count].GetPlayerID(), 2);
            SaveData_ToCSVPlayerList();

            DisplayDataList[] displayDataList = mDisplayDataList;
            for (int i = 0; i < mDisplayPlayNum; i++)
            {
                if (i < display_count) continue;
                if( i+1 >= mDisplayPlayNum )
                {
                    mDisplayDataList[i].InitDatas();
                    continue;
                }
                mDisplayDataList[i] = displayDataList[i+1];
            }

            ResetAllDisplay();
            NextTurnDisplay();
        }
    }
}
