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

        public PlayerList(int id, string sName)
        {
            mID = id;
            mName = sName;
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
        public virtual string Search(int id)
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
    }
}
