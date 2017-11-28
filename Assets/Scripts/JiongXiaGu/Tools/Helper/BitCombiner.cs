using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{


    /// <summary>
    /// 对基础数据类型进行位合并;
    /// 将多个
    /// </summary>
    public static class BitCombiner
    {
        

        public static short GetInt16(int collection, int index)
        {
            if (index < 0 || index > 2)
                throw new ArgumentOutOfRangeException("index");

            byte[] array = BitConverter.GetBytes(collection);
            return BitConverter.ToInt16(array, index);
        }

        public static int SetValue(int collection, short value, int index)
        {
            if (index < 0 || index > 2)
                throw new ArgumentOutOfRangeException("index");

            var destArray = BitConverter.GetBytes(collection);
            var valueArray = BitConverter.GetBytes(value);
            Array.Copy(valueArray, 0, destArray, index, 2);
            return BitConverter.ToInt32(destArray, 0);
        }
    }
}
