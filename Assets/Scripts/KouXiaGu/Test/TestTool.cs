using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D;

namespace KouXiaGu.Test
{

    public static class TestTool
    {
        static Random random = new Random();


        public static short RandomShort
        {
           get { return (short)random.Next(short.MinValue, short.MaxValue); }
        }

        public static ShortVector2 RandomShortVector2
        {
            get { return new ShortVector2(RandomShort, RandomShort); }
        }

        public static IEnumerable<ShortVector2> RandomShortVector2s(uint number)
        {
            for (uint i = 0; i < number; i++)
            {
                yield return RandomShortVector2;
            }
        }


        //[ContextMenu("Test")]
        //private void Test()
        //{

        //    foreach (var item in KouXiaGu.Test.TestTool.RandomShortVector2s(1000))
        //    {
        //        ShortVector2 newShortVector2 = ShortVector2.HashCodeToShortVector2(item.GetHashCode());
        //        if (newShortVector2.GetHashCode() != item.GetHashCode())
        //        {
        //            Debug.Log(item.ToString() + item.GetHashCode() + 
        //                newShortVector2.ToString() + newShortVector2.GetHashCode());
        //        }
        //        //Debug.Log(item.ToString() + item.GetHashCode() +
        //        //        newShortVector2.ToString() + newShortVector2.GetHashCode());
        //    }
        //    Debug.Log("完毕!");
        //}

        //[ContextMenu("Test2")]
        //private void Test2()
        //{
        //    Debug.Log(((short.MinValue) & 0xFFFF));
        //    Debug.Log(new ShortVector2(short.MaxValue, short.MaxValue).GetHashCode() == int.MaxValue);
        //    Debug.Log(new ShortVector2(short.MaxValue, short.MaxValue).GetHashCode());
        //    Debug.Log(int.MaxValue);
        //}

    }

}
