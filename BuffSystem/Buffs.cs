using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.BuffSystem
{
    public enum BuffType
    {
        Multiply,
        Sumator
    }
    public class Buff
    {
        public float buffValue;
        public BuffType buffType;

        public Buff(float buffValue, BuffType buffType)
        {
            this.buffValue = buffValue;
            this.buffType = buffType;
        }
        public virtual bool IsEnd()
        {
            return false;
        }
    }
    class TimerBuff : Buff
    {
        private float _timer;
        /// <summary>
        /// временный бафф
        /// </summary>
        /// <param name="time">время в секундах</param>
        public TimerBuff(float buffValue, BuffType buffType, float time) : base(buffValue, buffType)
        {
            _timer = time;
        }
        public override bool IsEnd()
        {
            _timer -= Time.deltaTime;
            UnityEngine.Debug.Log(_timer);
            return _timer < 0;
        }
    }
    class ConditionBuff : Buff
    {
        private Func<bool> EndCondition;
        /// <summary>
        /// временный бафф
        /// </summary>
        /// <param name="time">время в секундах</param>
        public ConditionBuff(float buffValue, BuffType buffType, Func<bool> endCondition) : base(buffValue, buffType)
        {
            EndCondition = endCondition;
        }
        public override bool IsEnd()
        {
            return EndCondition();
        }
    }
}
