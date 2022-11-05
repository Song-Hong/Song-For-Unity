using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Song.Runtime.Core.Data;
using Song.Editor.Core.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Ping = System.Net.NetworkInformation.Ping;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// net page
    /// </summary>
    public class NetPage : SongSetPage
    {
        public override VisualElement Show()
        {
            Set set = Config.LoadSet("Assets/Song/Extend/SongSet/Editor/Config/netpage.songset");

            VisualElement root = new VisualElement();
            VisualElement pc = new VisualElement();
            VisualElement net = new VisualElement();

            Texture2D pc_icon =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Song/Extend/SongSet/Editor/Art/computer.png");
            Texture2D net_icon =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Song/Extend/SongSet/Editor/Art/server.png");
            if (pc_icon != null) pc.style.backgroundImage = pc_icon;
            if (net_icon != null) net.style.backgroundImage = net_icon;

            pc.style.position = Position.Absolute;
            pc.style.width = 64;
            pc.style.height = 64;
            pc.style.marginLeft = Length.Percent(10);
            pc.style.marginTop = 10;

            net.style.position = Position.Absolute;
            net.style.width = 64;
            net.style.height = 64;
            net.style.marginLeft = Length.Percent(80);
            net.style.marginTop = 10;

            Label pc_ip = new Label(set["pc_ip"]);
            Label net_ip = new Label(set["net_ip"]);
            pc_ip.style.marginTop = 70;
            pc_ip.style.unityTextAlign = TextAnchor.UpperCenter;

            net_ip.style.position = Position.Absolute;
            net_ip.name = "net_ip";
            net_ip.style.marginTop = 70;
            net_ip.style.unityTextAlign = TextAnchor.UpperCenter;

            //Ping ping = new Ping();
            //PingReply pingReply = ping.Send(set["net_ip"]);
            //if (pingReply.Status == IPStatus.Success)
            //{
            //    Debug.Log("当前在线，已ping通！");
            //}
            //else
            //{
            //    Debug.Log("不在线，ping不通！");
            //}

            root.Add(pc);
            root.Add(net);
            pc.Add(pc_ip);
            net.Add(net_ip);
            return root;
        }
    }
}
