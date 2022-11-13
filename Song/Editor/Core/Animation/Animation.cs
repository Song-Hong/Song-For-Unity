using System;
using System.Numerics;
using UnityEngine.UIElements;
using Unity.Collections;
using Unity.Jobs;
using Vector2 = UnityEngine.Vector2;

namespace Song.Editor.Core.Animation
{
    public static class Animation
    {
        public static void Move(this VisualElement visual,Vector2 target)
        {
            //get this VisualElement position
            var x= visual.style.left.value.value;
            var y = visual.style.height.value.value;
            var nowPo = new Vector2(x, y);
            
            var po = new NativeArray<Vector2>();
            po.CopyFrom(new []{nowPo});
            var job = new MoveIJob()
            {
                Po = po,
            };
            var jobHandle = job.Schedule();
            jobHandle.Complete();
            po.Dispose();
        }

        public static void Scale()
        {
            
        }
    }
}