using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairyEngine.BuffSystem
{
    public abstract class BuffableValue<T>
    {
        public readonly T baseValue;
        public T value { get; protected set; }

        public BuffableValue(T baseValue)
        {
            this.baseValue = baseValue;
            value = baseValue;
        }
        public abstract void Update(float multiK, float addN);
    }
    public class BuffableXXLNum : BuffableValue<XXLNum>
    {
        public BuffableXXLNum(XXLNum baseValue) : base(baseValue)
        {
        }

        public override void Update(float multiK, float addN)
        {
            value = baseValue * multiK + addN;
        }
    }
    public class BuffableArrayXXLNum : BuffableValue<XXLNum[]>
    {
        public BuffableArrayXXLNum(XXLNum[] baseValue) : base(baseValue)
        {
        }

        public override void Update(float multiK, float addN)
        {
            for(int i = 0; i < baseValue.Length; i++)
                value[i] = baseValue[i] * multiK + addN;
        }
    }
    public class BuffableFloat : BuffableValue<float>
    {
        public BuffableFloat(float baseValue) : base(baseValue)
        {
        }

        public override void Update(float multiK, float addN)
        {
            value = baseValue * multiK + addN;
        }
    }
}
