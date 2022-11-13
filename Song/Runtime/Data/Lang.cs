using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Song.Runtime.Core.Data
{
    /// <summary>
    /// lang file
    /// </summary>
    public class Lang
    {
        #region Data
        public Dictionary<string, Set>          datas;
        public string                           savepath;
        #endregion

        #region 
        public Lang()                        => datas = new Dictionary<string, Set>();
        public Lang(string content) : this() => LoadLang(content);
        #endregion

        #region
        public Set this[string key]
        {
            get
            {
                if (datas.ContainsKey(key))
                    return datas[key];
                else
                {
                    Set set = new Set();
                    datas.Add(key, set);
                    return set;
                }
            }
            set
            {
                if (datas.ContainsKey(key))
                    datas[key] = value;
                else
                    datas.Add(key, value);
            }
        }

        public void LoadLang(string langstr)
        {
            if (string.IsNullOrWhiteSpace(langstr)) return;
            while (langstr.Length > 0)
            {
                var l_index = langstr.IndexOf("{");
                if (l_index <= 0) return;
                var key = langstr.Substring(0, l_index).Trim();
                var r_index = langstr.IndexOf("}");
                if (r_index <= 0) return;
                var content = langstr.Substring(l_index + 1, r_index-l_index-1);
                content = content.Replace("\t","").Trim();
                Set set = new Set(content);
                datas.Add(key, set);
                langstr = langstr.Substring(r_index+1, langstr.Length - r_index-1).Trim();
                if (string.IsNullOrWhiteSpace(langstr)) return;
            }
        }

        public override string ToString()
        {
            string content = "";
            foreach (var item in datas.Keys)
            {
                string data_content = "";
                foreach (var data in datas[item].datas)
                {
                    data_content += string.Format($"\t{data.Key} : {data.Value}\n");
                }
                content +=
                    item+
                    "\n{\n"+
                    data_content
                    + "}\n";
            }
            return content;
        }
        #endregion
    }
}