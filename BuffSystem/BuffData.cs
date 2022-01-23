using HairyEngine.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairyEngine.BuffSystem
{
    public struct BuffData
    {
        public string name;
        public Tuple<float, float> buffParams { get; private set; }
        public bool isNotifyRequire;
        private List<Buff> buffs;

        public BuffData(string name, Buff[] buffs, bool isNotifyRequire)
        {
            this.name = name;
            this.buffs = new List<Buff>(buffs);
            this.isNotifyRequire = isNotifyRequire;
            buffParams = null;
            InitBuffParamsPair();
        }
        public BuffData(string name, List<Buff> buffs, bool isNotifyRequire)
        {
            this.name = name;
            this.buffs = buffs;
            this.isNotifyRequire = isNotifyRequire;
            buffParams = null;
            InitBuffParamsPair();
        }
        public void AddBuff(Buff buff)
        {
            if (isNotifyRequire)
                EventManager.TriggerEvent(new BuffNotifyData(buff, name, true));
            buffs.Add(buff);
            InitBuffParamsPair();
        }
        public void RemoveBuff(Buff buff)
        {
            if (isNotifyRequire)
                EventManager.TriggerEvent(new BuffNotifyData(buff, name, false));
            buffs.Remove(buff);
            InitBuffParamsPair();
        }
        public void Update()
        {
            foreach (Buff buff in buffs.ToArray())
                if (buff.IsEnd())
                    RemoveBuff(buff);
        }
        //используй этот метод для всего.
        //для баффа цен в манагере цен
        private void InitBuffParamsPair()
        {
            float multiK = 1, addN = 0;
            foreach (Buff buff in buffs)
                switch (buff.buffType)
                {
                    case BuffType.Multiply:
                        multiK *= buff.buffValue;
                        break;
                    case BuffType.Sumator:
                        addN += buff.buffValue;
                        break;
                }
            buffParams = new Tuple<float, float>(multiK, addN);
        }
    }
    //ивент дата при добавлении/удалении баффа
    public struct BuffNotifyData
    {
        public Buff newOrDeletedBuff;
        public string buffClass;
        public bool isAdded;
        public BuffNotifyData(Buff newOrDeletedBuff, string buffClass, bool isAdded)
        {
            this.newOrDeletedBuff = newOrDeletedBuff;
            this.buffClass = buffClass;
            this.isAdded = isAdded;
        }
    }
}
