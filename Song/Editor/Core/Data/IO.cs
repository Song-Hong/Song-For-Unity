using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Song.Editor.Core
{
    /// <summary>
    /// song editor file system
    /// </summary>
    public class IO
    {
        /// <summary>
        /// async read file
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="EndCallBack">end call back</param>
        public async void ReadA(string path, Action<string> EndCallBack=null)
        {
            var file_task = new Task<string>(() => StartRead(path));
            file_task.Start();
            var file_value = await file_task;
            EndCallBack?.Invoke(file_value);
        }

        private string StartRead(string path)
        {
            if (!File.Exists(path)) throw new Exception("file can't exists");
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        public void SaveTCover(string path,string content)
        {
            Thread thread = new Thread(() =>
            {
                File.WriteAllText(path, content);
            });
            thread.Start();
        }
    }
}
