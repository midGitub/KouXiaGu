using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    public abstract class OperateWaiter : MonoBehaviour
    {
        protected OperateWaiter()
        {
        }

        List<IOperateAsync> operaterList;
        List<IOperateAsync> faultedList;

        public List<IOperateAsync> OperaterList
        {
            get { return operaterList; }
        }

        public List<IOperateAsync> FaultedList
        {
            get { return faultedList; }
        }

        /// <summary>
        /// 等待中的执行程序数目;若还未初始化则返回 -1;
        /// </summary>
        public int Remainder
        {
            get { return operaterList == null ? -1 : operaterList.Count; }
        }

        public bool IsFaulted
        {
            get { return faultedList != null ? faultedList.Count > 0 : false; }
        }

        protected virtual void Awake()
        {
            operaterList = new List<IOperateAsync>();
            faultedList = new List<IOperateAsync>();
        }

        protected virtual void Update()
        {
            bool isCompleted = IsCompleted();
            if (isCompleted)
            {
                enabled = false;
            }
        }

        public bool IsCompleted()
        {
            while (operaterList.Count != 0)
            {
                var operater = operaterList[0];

                if (operater.IsCompleted)
                {
                    if (operater.IsFaulted)
                        faultedList.Add(operater);

                    operaterList.RemoveAt(0);
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        protected void AddOperater(IOperateAsync operater)
        {
            operaterList.Add(operater);
        }

    }

}
