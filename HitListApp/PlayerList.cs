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
        public int mID;
        public string mName;
        public int mVacancieType;

        public PlayerList(int id, string sName)
        {
            mID = id;
            mName = sName;
            mVacancieType = 0;
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
                            return p.mID;
                        case 1:
                            return p.mName;
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
                    if (p.mID == id)
                    {
                        return p.mName;
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
                    if (p.mID == id)
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
                    if (p.mID == id)
                    {
                        p.mVacancieType = value;
                    }
                }
            }
        }
    }
}
