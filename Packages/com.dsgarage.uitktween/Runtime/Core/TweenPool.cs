using System;
using System.Collections.Generic;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// Tweener/Sequence のオブジェクトプール。
    /// Stack ベースでGCアロケーションを削減する。
    /// </summary>
    internal static class TweenPool
    {
        // 型別に Stack を保持する汎用プール
        private static class Pool<T> where T : Tween, new()
        {
            internal static readonly Stack<T> stack = new(32);
            internal static int totalCreated;
        }

        /// <summary>プールから取得、または新規作成</summary>
        internal static T Get<T>() where T : Tween, new()
        {
            if (Pool<T>.stack.Count > 0)
            {
                return Pool<T>.stack.Pop();
            }
            Pool<T>.totalCreated++;
            return new T();
        }

        /// <summary>プールに返却</summary>
        internal static void Return<T>(T tween) where T : Tween, new()
        {
            tween.Reset();
            Pool<T>.stack.Push(tween);
        }

        /// <summary>プールの事前確保</summary>
        internal static void Warmup<T>(int count) where T : Tween, new()
        {
            for (int i = 0; i < count; i++)
            {
                Pool<T>.stack.Push(new T());
                Pool<T>.totalCreated++;
            }
        }

        /// <summary>プール内のオブジェクト数</summary>
        internal static int PooledCount<T>() where T : Tween, new()
        {
            return Pool<T>.stack.Count;
        }

        /// <summary>全プールをクリア</summary>
        internal static void ClearAll()
        {
            // ジェネリック型ごとにクリアする必要があるが、
            // 静的ジェネリッククラスのため、既知の型のみ明示的にクリア
            ClearPool<Tweener<float>>();
            ClearPool<Tweener<UnityEngine.Vector2>>();
            ClearPool<Tweener<UnityEngine.Vector3>>();
            ClearPool<Tweener<UnityEngine.Color>>();
            ClearPool<Tweener<UnityEngine.Quaternion>>();
            ClearPool<Sequence>();
        }

        private static void ClearPool<T>() where T : Tween, new()
        {
            Pool<T>.stack.Clear();
        }
    }
}
