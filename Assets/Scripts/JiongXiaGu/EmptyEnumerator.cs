using System.Collections;

namespace JiongXiaGu
{

    /// <summary>
    /// 一个空的枚举结构;
    /// </summary>
    internal class EmptyEnumerator : IEnumerator
    {
        EmptyEnumerator() { }

        static readonly EmptyEnumerator instance = new EmptyEnumerator();

        public static EmptyEnumerator GetInstance
        {
            get { return instance; }
        }

        object IEnumerator.Current
        {
            get { return null; }
        }

        bool IEnumerator.MoveNext()
        {
            return false;
        }

        void IEnumerator.Reset()
        {
            return;
        }
    }

}
