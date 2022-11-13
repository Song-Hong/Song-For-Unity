using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace Song.Editor.Core
{
    /// <summary>
    /// Listening File IO
    /// </summary>
    public class IORW 
    {
        private Thread Th;

        public void Start(string path)
        {
            Th = new Thread(() =>
            {
                FileStream file = new FileStream(path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
            });
            Th.Start();
        }

        public void Close()
        {
            if (Th != null && Th.IsAlive)
            {
                Th.Abort();
                Th = null;
            }
        }
    }
}
