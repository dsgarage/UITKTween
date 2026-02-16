using System;
using System.Collections.Generic;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// 全Tweenの管理と更新を担当する静的マネージャー
    /// </summary>
    internal static class TweenManager
    {
        private static readonly List<Tween> activeTweens = new(64);
        private static readonly List<Tween> pendingAdd = new(16);
        private static readonly List<Tween> pendingRemove = new(16);
        private static bool isUpdating;

        internal static int ActiveCount => activeTweens.Count;
        internal static System.Collections.Generic.IReadOnlyList<Tween> ActiveTweens => activeTweens;

        internal static void Add(Tween tween)
        {
            if (isUpdating)
                pendingAdd.Add(tween);
            else
                activeTweens.Add(tween);
        }

        internal static void MarkForRemoval(Tween tween)
        {
            if (isUpdating)
                pendingRemove.Add(tween);
            else
                activeTweens.Remove(tween);
        }

        internal static void Update(float deltaTime, UpdateType updateType)
        {
            if (activeTweens.Count == 0) return;

            isUpdating = true;

            for (int i = activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = activeTweens[i];
                if (tween == null || !tween.isActive)
                {
                    activeTweens.RemoveAt(i);
                    continue;
                }

                if (tween.updateType != updateType) continue;

                tween.UpdateInternal(deltaTime);
            }

            isUpdating = false;

            // 保留中の追加・削除を処理
            if (pendingRemove.Count > 0)
            {
                for (int i = 0; i < pendingRemove.Count; i++)
                    activeTweens.Remove(pendingRemove[i]);
                pendingRemove.Clear();
            }

            if (pendingAdd.Count > 0)
            {
                for (int i = 0; i < pendingAdd.Count; i++)
                    activeTweens.Add(pendingAdd[i]);
                pendingAdd.Clear();
            }
        }

        // === グローバル制御 ===

        internal static void PauseAll()
        {
            for (int i = 0; i < activeTweens.Count; i++)
                activeTweens[i].Pause();
        }

        internal static void PlayAll()
        {
            for (int i = 0; i < activeTweens.Count; i++)
                activeTweens[i].Play();
        }

        internal static void KillAll(bool complete = false)
        {
            isUpdating = true;
            for (int i = activeTweens.Count - 1; i >= 0; i--)
                activeTweens[i].Kill(complete);
            isUpdating = false;

            activeTweens.Clear();
            pendingAdd.Clear();
            pendingRemove.Clear();
        }

        // === ID/Target による制御 ===

        internal static void PauseById(object id)
        {
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (Equals(activeTweens[i].id, id))
                    activeTweens[i].Pause();
            }
        }

        internal static void PlayById(object id)
        {
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (Equals(activeTweens[i].id, id))
                    activeTweens[i].Play();
            }
        }

        internal static void KillById(object id, bool complete = false)
        {
            for (int i = activeTweens.Count - 1; i >= 0; i--)
            {
                if (Equals(activeTweens[i].id, id))
                    activeTweens[i].Kill(complete);
            }
        }

        internal static void PauseByTarget(object target)
        {
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (ReferenceEquals(activeTweens[i].target, target))
                    activeTweens[i].Pause();
            }
        }

        internal static void PlayByTarget(object target)
        {
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (ReferenceEquals(activeTweens[i].target, target))
                    activeTweens[i].Play();
            }
        }

        internal static void KillByTarget(object target, bool complete = false)
        {
            for (int i = activeTweens.Count - 1; i >= 0; i--)
            {
                if (ReferenceEquals(activeTweens[i].target, target))
                    activeTweens[i].Kill(complete);
            }
        }

        // === ユーティリティ ===

        internal static int TweensById(object id)
        {
            int count = 0;
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (Equals(activeTweens[i].id, id)) count++;
            }
            return count;
        }

        internal static int TweensByTarget(object target)
        {
            int count = 0;
            for (int i = 0; i < activeTweens.Count; i++)
            {
                if (ReferenceEquals(activeTweens[i].target, target)) count++;
            }
            return count;
        }

        internal static void Clear()
        {
            activeTweens.Clear();
            pendingAdd.Clear();
            pendingRemove.Clear();
        }
    }
}
