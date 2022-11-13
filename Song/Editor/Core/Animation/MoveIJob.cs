using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Song.Editor.Core.Animation
{
    public struct MoveIJob : IJob
    {
        public NativeArray<Vector2> Po;

        public void Execute()
        {
            
        }
    }
}
