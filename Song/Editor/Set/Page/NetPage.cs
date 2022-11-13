using System.Net.NetworkInformation;
using Song.Editor.Core.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Ping = System.Net.NetworkInformation.Ping;
using PingReply = System.Net.NetworkInformation.PingReply;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// net page
    /// </summary>
    public class NetPage : SongSetPage
    {
        public override string Name() => "Net";

        public override VisualElement Show()
        {
            var set = Config.LoadSet("Assets/Song/Editor/Others/Config/Set/netpage.songset");
            var root = new VisualElement();
            var pc = new VisualElement()
            {
                style =
                {
                    position = Position.Absolute,
                    width = 64,
                    height = 64,
                    marginLeft = Length.Percent(10),
                    marginTop = 10
                }
            };
            var net = new VisualElement()
            {
                style =
                {
                    position = Position.Absolute,
                    width = 64,
                    height = 64,
                    marginLeft = Length.Percent(80),
                    marginTop = 10
                }
            };
            var server = new VisualElement()
            {
                style =
                {
                    position = Position.Absolute,
                    width = 64,
                    height = 64,
                    marginLeft = Length.Percent(45),
                    marginTop = 10
                }
            };
            var pcIcon =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Song/Editor/Others/Art/Icons/computer.png");
            var netIcon =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Song/Editor/Others/Art/Icons/server.png");
            var serverIcon =
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Song/Editor/Others/Art/Icons/Cloud.png");
            if (pcIcon != null) pc.style.backgroundImage = pcIcon;
            if (netIcon != null) net.style.backgroundImage = netIcon;
            if (serverIcon != null) server.style.backgroundImage = serverIcon;
            root.Add(pc);
            root.Add(net);
            root.Add(server);
            
            //input 
            var pcIP = new TextField()
            {
                value = set["pc_ip"],
                name = "pc_ip",
                style =
                {
                    unityTextAlign = TextAnchor.UpperCenter,
                    position = Position.Absolute,
                    marginTop = 70,
                }
            };
            var netIP = new TextField()
            {
                value = set["net_ip"],
                name = "net_ip",
                style =
                {
                    unityTextAlign = TextAnchor.UpperCenter,
                    position = Position.Absolute,
                    marginTop = 70,
                }
            };
            pcIP.style.width = pcIP.text.Length * 10;
            netIP.style.width = netIP.text.Length * 10;
            pc.Add(pcIP);
            net.Add(netIP);

            var pingBtn = new Button()
            {
                text = "Ping",
                style =
                {
                    top = 200,
                    left= 200,
                    width = 100,
                    height = 20,
                }
            };
            pingBtn.clicked+= delegate
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(set["net_ip"]);
                if (pingReply != null && pingReply.Status == IPStatus.Success)
                {
                    Debug.Log("当前在线，已ping通！");
                }
                else
                {
                    Debug.Log("不在线，ping不通！");
                }
            };
            root.Add(pingBtn);
            
            return root;
        }
    }
}
