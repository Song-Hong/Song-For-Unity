using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

namespace Song.Editor.Core.Animation
{
    public static partial class Animation
    {
        /// <summary>
        /// Center zoom animation (Scale)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static VisualElement AScale(this VisualElement self,Vector2 target,float time)
        {
            var size = self.style.scale.value.value;
            Thread animationThread = null;
            var animationThreadController = new AutoResetEvent(false);
            var mainThread = SynchronizationContext.Current;
            animationThread = new Thread(() =>
            {
                //init
                mainThread.Post(new SendOrPostCallback(delegate(object state)
                {
                    if (self.name.Contains("[ANIMATIONTASK:SCALE]"))
                    {
                        if(animationThread.IsAlive && animationThread!=null)
                            animationThread.Abort();
                        return;
                    }
                    self.name += "[ANIMATIONTASK:SCALE]";
                    if (self.name.Contains("[FROM]"))
                    {
                        var nsize = size;
                        size = target;
                        target = nsize;
                    }
                    animationThreadController.Set();
                }),"");
                
                //run
                animationThreadController.WaitOne();
                var itime = Mathf.FloorToInt(time*1000);
                var sizes = new Line().Straight(size, new Vector2(target.x,target.y),itime);
                for (int i = 0; i < itime; i++)
                {
                    mainThread.Post(new SendOrPostCallback(delegate(object state)
                    {
                        self.style.scale = (Vector2)state;
                    }), new Vector2(sizes[i].x,sizes[i].y));
                    Thread.Sleep(1);
                }
                
                //end
                mainThread.Post(new SendOrPostCallback(delegate(object state)
                {
                    self.style.scale = (Vector2)state;
                    self.name = self.name.Replace("[ANIMATIONTASK:SCALE]", "");
                    if(animationThread is { IsAlive: true })
                        animationThread.Abort();
                }), new Vector2(target.x,target.y));
            });
            animationThread.Name = self.name+"[ANIMATIONTASK:SCALE]";
            animationThread.IsBackground = true;
            animationThread.Start();
            return self;
        }

        /// <summary>
        /// Center zoom animation (Width and Height)
        /// </summary>
        /// <param name="self">self</param>
        /// <param name="target">TargetPo</param>
        /// <param name="time">Time</param>
        public static VisualElement ASize(this VisualElement self,Vector2 target,float time)
        {
            float w=0, h=0, x=0, y =0;
            Thread animationThread = null;
            var animationThreadController = new AutoResetEvent(false);
            var mainThread = SynchronizationContext.Current;
            animationThread = new Thread(() =>
            {
                mainThread.Post(new SendOrPostCallback(delegate(object state)
                {
                    if (self.name.Contains("[ANIMATIONTASK:SIZE]"))
                    {
                        if(animationThread.IsAlive && animationThread!=null)
                            animationThread.Abort();
                        return;
                    }
                    self.name += "[ANIMATIONTASK:SIZE]";
                    if (self.name.Contains("[FROM]"))
                    {
                        var a = target.x;
                        var b = target.y;
                        target.x = self.style.width.value.value;
                        target.y = self.style.height.value.value;
                        self.style.width  = a;
                        self.style.height = b;
                        self.name = self.name.Replace("[FROM]","");
                        w  = self.style.width.value.value;
                        h  = self.style.height.value.value;
                        x  = self.style.left.value.value + (target.x / 2 - w/2);
                        y  = self.style.top.value.value  + (target.y / 2 - h/2);
                        self.style.left = x ;
                        self.style.top  = y;
                    }
                    else
                    {
                        w  = self.style.width.value.value;
                        h  = self.style.height.value.value;
                        x  = self.style.left.value.value;
                        y  = self.style.top.value.value;
                    }
                    animationThreadController.Set();  
                }),"");
                animationThreadController.WaitOne();
                var itime = Mathf.FloorToInt(time*1000);
                var targetx = x - ((target.x / 2) - w / 2);
                var targety = y - ((target.y / 2) - h / 2);
                var pos = new Line().Straight(new Vector2(x, y), new Vector2(targetx,targety),itime);
                var sizes = new Line().Straight(new Vector2(w, h), new Vector2(target.x, target.y), itime);
                for (int i = 0; i < itime; i++)
                {
                    mainThread.Post(new SendOrPostCallback(delegate(object state)
                    {
                        var r = (Rect)state;
                        self.style.width   = r.width;
                        self.style.height  = r.height;
                        self.style.left    = r.x;
                        self.style.top     = r.y;
                    }), new Rect(pos[i].x,pos[i].y,sizes[i].x,sizes[i].y));
                    Thread.Sleep(1);
                }
                mainThread.Post(new SendOrPostCallback(delegate(object state)
                {
                    var r = (Rect)state;
                    self.style.width   = r.width;
                    self.style.height  = r.height;
                    self.style.left    = r.x;
                    self.style.top     = r.y;
                    self.name = self.name.Replace("[ANIMATIONTASK:SIZE]", "");
                }), new  Rect(targetx,targety,target.x,target.y));
            });
            animationThread.Name = self.name+"[ANIMATIONTASK:SIZE]";
            animationThread.IsBackground = true;
            animationThread.Start();
            return self;
        }
        
        /// <summary>
        /// tag From
        /// </summary>
        public static VisualElement From(this VisualElement self)
        {
            self.name += "[FROM]";
            return self;
        }
        
        /// <summary>
        /// Set Excessiveness
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static VisualElement Exce(this VisualElement self,Excessive ex)
        {
            self.name += $"[EXCE]->[{ex.ToString()}]";
            return self;
        }
    }
}