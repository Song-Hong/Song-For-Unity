using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Song.Runtime.Support
{
    public class UnDo<T>
    {
        #region Action
        public event Action<T>  UnDoCallBack;
        public event Action<T>  ReDoCallBack;
        #endregion

        #region Data
        private Stack<T> DoList;
        private Stack<T> ReList;
        #endregion

        #region Init
        public UnDo()
        {
            DoList = new Stack<T>();
            ReList = new Stack<T>();
        }

        public UnDo(Action<T> UnDoCallBack, Action<T> ReDoCallBack) :this()
        {
            this.UnDoCallBack += UnDoCallBack;
            this.ReDoCallBack += ReDoCallBack;
        }
        #endregion

        #region Operation
        public virtual void Do(T data)
        {
            DoList.Push(data);
            ReList.Clear();
        }

        public virtual void Un()
        {
            if (DoList.Count <= 0) return;
            var item = DoList.Pop();
            ReList.Push(item);
            UnDoCallBack?.Invoke(item);
        }

        public virtual void Re()
        {
            if (ReList.Count <= 0) return;
            var item = ReList.Pop();
            DoList.Push(item);
            ReDoCallBack?.Invoke(item);
        }

        public virtual void Un(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Un();
            }
        }

        public virtual void Re(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Re();
            }
        }
        #endregion
    }
}
