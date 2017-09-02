using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    enum CheckType
    {
        CHECK_BATU,
        CHECK_MARU,
        CHECK_NONE,

        CHECK_MAX,
    };

    class DisplayDataList
    {
        const int MAX_PLAY_NUM = 4;
        public DisplayDataList()
        {
            mCheck = new int[MAX_PLAY_NUM];
            InitDatas();
        }

        public void InitDatas()
        {
            for (int i = 0; i < MAX_PLAY_NUM; i++ )
                mCheck[i] = (int)CheckType.CHECK_NONE;

            mTotalResult = 0;
        }

        public void ClickCheckButton(int s)
        {
            mCheck[s]++;
            if (mCheck[s] >= (int)CheckType.CHECK_MAX)
            {
                mCheck[s] = 0;
            }
            CalculateTotalSum();
        }
        protected void CalculateTotalSum()
        {
            int count = 0;
            for (int i = 0; i < MAX_PLAY_NUM; i++)
            {
                if (mCheck[i] == (int)CheckType.CHECK_MARU)
                    count++;
            }
            mTotalResult = count;
        }
        public CheckType GetCheckButton(int s)
        {
            return (CheckType)mCheck[s];
        }
        public void SetCheckButton(int s, int value)
        {
            mCheck[s] = value;
            CalculateTotalSum();
        }
        public int GetTotalResult()
        {
            return mTotalResult;
        }

        public void SetPlayerID(int playerID)
        {
            mPlayerID = playerID;
        }
        public int GetPlayerID()
        {
            return mPlayerID;
        }

        protected int[] mCheck;
        protected int mTotalResult;
        protected int mPlayerID;
    }
}
