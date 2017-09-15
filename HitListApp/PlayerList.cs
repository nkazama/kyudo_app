using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace HitListApp
{
    class PlayerList
    {
        public enum CSV
        {
            ID,
            NAME,
            GROUP,
            VACANCIE,
            MEMO,
        };

        protected int mID;
        protected string mName;
        protected string mGroup;
        protected int mVacancieType;
        protected string mMemo;

        protected bool mIsUpdated;

        public PlayerList(int id, string sName, string group, string vacancie, string memo)
        {
            mID = id;
            mName = sName;
            mGroup = group;
            mVacancieType = 0;
            mMemo = memo;

            mIsUpdated = false;

            if ( vacancie == GetVacancieType(1)) mVacancieType = 1;
            else if (vacancie == GetVacancieType(2)) mVacancieType = 2;
        }
        static public string GetVacancieType(int type)
        {
            switch (type)
            {
                case 1: return "欠";
                case 2: return "欠詰";
                default: return "";
            }
        }
        public string GetMemberVacancieType()
        {
            return GetVacancieType(mVacancieType);
        }

        public int GetID() { return mID; }
        public string GetName() { return mName; }
        public string GetGroup() { return mGroup; }
        public string GetMemo() { return mMemo; }
        public int GetVacancie() { return mVacancieType; }

        public void SetVacancie(int type)
        {
            if (type == mVacancieType) return;
            mVacancieType = type;
            mIsUpdated = true;
        }

        public bool IsNeedUpdated() { return mIsUpdated; }
        public void ResetUpdated()
        {
            mIsUpdated = false;
        }
    }

    class PlayerListAL : ArrayList
    {
        public virtual object this[int nIndex, int nType]
        {
            get
            {
                object obj = this[nIndex];
                if (obj.GetType() == typeof(PlayerList))
                {
                    PlayerList p = (PlayerList)obj;
                    switch (nType)
                    {
                        case 0:
                            return p.GetID();
                        case 1:
                            return p.GetName();
                    }
                }
                return null;
            }
        }
        public virtual string SearchPlayerName(int id)
        {
            foreach (object obj in this)
            {
                if (obj.GetType() == typeof(PlayerList))
                {
                    PlayerList p = (PlayerList)obj;
                    if (p.GetID() == id)
                    {
                        return p.GetName();
                    }
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        public virtual string SearchVacancieType(int id)
        {
            foreach (object obj in this)
            {
                if (obj.GetType() == typeof(PlayerList))
                {
                    PlayerList p = (PlayerList)obj;
                    if (p.GetID() == id)
                    {
                        return p.GetMemberVacancieType();
                    }
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        public virtual void SearchAndSetVacancieType(int id, int value)
        {
            foreach (object obj in this)
            {
                if (obj.GetType() == typeof(PlayerList))
                {
                    PlayerList p = (PlayerList)obj;
                    if (p.GetID() == id)
                    {
                        p.SetVacancie(value);
                    }
                }
            }
        }
        public virtual int GetNumOfVancancies(int current_num = -1, int start_num = 0)
        {
            int total = 0;
            int count = 0;
            foreach (object obj in this)
            {
                if (obj.GetType() == typeof(PlayerList))
                {
                    if (count >= start_num)
                    {
                        PlayerList p = (PlayerList)obj;
                        if (p.GetVacancie() == 2)
                        {
                            total++;
                        }
                    }
                    count++;
                    if (count > current_num && current_num > 0) break;
                }
            }
            return total;
        }
    }
}
