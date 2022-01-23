using HairyEngine.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HairyEngine.BuffSystem
{
    static class BuffSystem
    {
        public static int BuffValueCount { get; private set; }
        private static Dictionary<string, BuffData> _buffs;
        static BuffSystem()
        {
            BuffValueCount = 0;
            _buffs = new Dictionary<string, BuffData>();
        }
        public static void Init(Dictionary<string, Buff[]> valueBuffs)
        {
            _buffs.Clear();
            foreach (KeyValuePair<string, Buff[]> pair in valueBuffs)
            {
                _buffs.Add(pair.Key, new BuffData(pair.Key, pair.Value, false));
            }
        }
        public static void AddBuff(string valueClass, Buff buff)
        {
            _buffs[valueClass].AddBuff(buff);
        }
        public static void RemoveBuff(string valueClass, Buff buff)
        {
            _buffs[valueClass].RemoveBuff(buff);
        }
        public static void Update(float deltaTime)
        {
            foreach (KeyValuePair<string, BuffData> pair in _buffs)
            {
                pair.Value.Update();
            }
        }
        public static void GetBuffParams(string valueClass, out float multiK, out float addN)
        {
            Tuple<float, float> paramsPair = _buffs[valueClass].buffParams;
            multiK = paramsPair.Item1;
            addN = paramsPair.Item2;
        }
    }
}
