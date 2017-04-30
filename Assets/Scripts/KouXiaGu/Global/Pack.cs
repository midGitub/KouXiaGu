using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public struct Pack<T1, T2>
    {
        public Pack(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        readonly T1 value1;
        readonly T2 value2;

        public T1 Value1
        {
            get { return value1; }
        }

        public T2 Value2
        {
            get { return value2; }
        }

        public override string ToString()
        {
            return "[value1:" + value1.ToString() + ",value2:" + value2.ToString() + "]";
        }
    }

    public struct Pack<T1, T2, T3>
    {
        public Pack(T1 value1, T2 value2, T3 value3)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

        readonly T1 value1;
        readonly T2 value2;
        readonly T3 value3;

        public T1 Value1
        {
            get { return value1; }
        }

        public T2 Value2
        {
            get { return value2; }
        }

        public T3 Value3
        {
            get { return value3; }
        }

        public override string ToString()
        {
            return "[value1:" + value1.ToString() + ",value2:" + value2.ToString() + ",value3:" + value3.ToString() + "]";
        }
    }

}
