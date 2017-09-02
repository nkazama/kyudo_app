﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace WindowsFormsApplication1
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
            GroupBox group = null;
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(GroupBox))
                {
                    group = (GroupBox)obj_all;
                    string s = (string)group.Tag;
                    int display_count = Int32.Parse(s);

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
            for(int i = 0; i < mDisplayPlayNum;i++)
            {
                int player = mDisplayDataList[i].GetPlayerID();
                int result_1 = (int)mDisplayDataList[i].GetCheckButton(0);
                int result_2 = (int)mDisplayDataList[i].GetCheckButton(1);
                int result_3 = (int)mDisplayDataList[i].GetCheckButton(2);
                int result_4 = (int)mDisplayDataList[i].GetCheckButton(3);
                int result_total = (int)mDisplayDataList[i].GetTotalResult();
                sw.WriteLine(mCurrentTurn + "," + i + "," + player + "," + result_1 + "," + result_2 + "," + result_3 + "," + result_4 + "," + result_total);
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
                    mPlayerList.Add(list);
                }
            }
        }
        private void LoadData_FromCSVResult()
        {
            string text = "";
            using (StreamReader sr = new StreamReader(FILENAME_RESULT, Encoding.GetEncoding("Shift_JIS")))
            {
                while (( text = sr.ReadLine()) != null)
                {
                    string[] stArrayData = text.Split(',');
                    if (Int32.Parse(stArrayData[0]) == mTurnNumber)
                    {
                        int display_count = Int32.Parse(stArrayData[1]);
                        int player_id = Int32.Parse(stArrayData[2]);
                        string player_name = mPlayerList.Search(player_id);
                        int[] result = {Int32.Parse(stArrayData[3]),
                                       Int32.Parse(stArrayData[4]),
                                       Int32.Parse(stArrayData[5]),
                                       Int32.Parse(stArrayData[6])
                                       };
                        LoadData(display_count, player_id, player_name, result);
                    }
                }
            }
        }
        private void LoadData(int display_count, int player_id, string player_name, int[] result)
        {
            GroupBox group = null;
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(GroupBox))
                {
                    group = (GroupBox)obj_all;
                    string s = (string)group.Tag;
                    if (display_count == Int32.Parse(s)) break;
                }
            }

            mDisplayDataList[display_count].SetPlayerID(player_id);

            SetLabel(TAG_ID, player_id.ToString(), group);
            SetLabel(TAG_NAME, player_name, group);

            //int i = 0;
            foreach (object obj in group.Controls)
            {
                if (obj.GetType() == typeof(Button))
                {
                    Button button = obj as Button;
                    string tag_s = (string)button.Tag;
                    int tag = Int32.Parse(tag_s);
                    mDisplayDataList[display_count].SetCheckButton(tag, result[tag]);
                    button.BackgroundImage = GetButtonBG(mDisplayDataList[display_count].GetCheckButton(tag));
                    //i++;
                    //if (i >= MAX_PLAY_NUM) break;
                }
            }

            SetLabel(TAG_RESULT, mDisplayDataList[display_count].GetTotalResult().ToString(), group);
        }
#endregion

        private void NextTurnDisplay()
        {
            GroupBox group = null;
            int player_num = mCurrent1stPlayer + (mTurnNumber - mCurrentTurn) * mDisplayPlayNum;
            foreach (object obj_all in group_all.Controls)
            {
                if (obj_all.GetType() == typeof(GroupBox))
                {
                    group = (GroupBox)obj_all;
                    string s = (string)group.Tag;
                    int display_count = Int32.Parse(s);

                    mDisplayDataList[display_count].SetPlayerID(player_num + display_count);
                    SetLabel(TAG_ID, mPlayerList[player_num + display_count, 0].ToString(), group);
                    SetLabel(TAG_NAME, mPlayerList[player_num + display_count, 1].ToString(), group);
                }
            }
        }
        
        private void checkbutton_click(object sender, EventArgs e)
        {
            int display_count = 0;
            Button button = (Button)sender;
            object obj = (Object)button.Parent;
            GroupBox group = null;
            if (obj.GetType() == typeof(GroupBox))
            {
                group = (GroupBox)obj;
                string s = (string)group.Tag;
                display_count = Int32.Parse(s);
            }

            string ss = (string)button.Tag;
            int play_count = Int32.Parse(ss);

            mDisplayDataList[display_count].ClickCheckButton(play_count);
            button.BackgroundImage = GetButtonBG(mDisplayDataList[display_count].GetCheckButton(play_count));

            SetLabel(TAG_RESULT, mDisplayDataList[display_count].GetTotalResult().ToString(), group);
        }

        private void SetLabel(string tag, string value, GroupBox group)
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
            mCurrentTurn++;
            mCurrent1stPlayer += mDisplayPlayNum; 
            numericUpDown_TurnNum.Value++;

            SaveData_FromCSVOption();
        }

    }
}