using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Song.Runtime.Core.Data
{
    public class Set 
    {
        #region data
        public Dictionary<string, string> datas;
        public string savepath;
        #endregion

        #region
        public Set()                             =>  datas = new Dictionary<string, string>();
        public Set(string content) : this()      =>  LoadSet(content);
        public Set(List<string> items) : this()  =>  LoadSet(items);
        public Set(string[] items) : this()      =>  LoadSet(items);
        #endregion

        public string this[string key]
        {
            get
            {
                if (datas.ContainsKey(key))
                    return datas[key];
                else
                    return null;
            }
            set
            {
                if (datas.ContainsKey(key))
                    datas[key] = value;
                else
                    datas.Add(key, value);
            }
        }

        #region io
        public void LoadSet(string content) => LoadSet(content.Split("\n"));

        public void LoadSet(List<string> items) => LoadSet(items.ToArray());

        public void LoadSet(string[] items)
        {
            foreach (var item in items)
            {
                string[] item_dic = item.Split(":");
                if (item_dic.Length < 2)
                    continue;
                string key = item_dic[0].Trim();
                if (!datas.ContainsKey(key))
                    datas.Add(key, item_dic[1].Trim());
            }
        }
        #endregion

        #region convert
        public override string ToString()
        {
            string content = "";
            foreach (var item in datas)
            {
                content += string.Format($"{item.Key} : {item.Value}\n");
            }
            return content;
        }
        #endregion
    }
}
