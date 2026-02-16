using System.Collections.Generic;

namespace DSGarage.UITKTween.Internal
{
    /// <summary>
    /// ID による Tween 検索を最適化するマネージャー。
    /// 将来的にハッシュマップベースの高速検索に拡張可能。
    /// 現在は TweenManager のリニアサーチに委譲。
    /// </summary>
    internal static class TweenIdManager
    {
        // 現時点では TweenManager のリニアサーチで十分なパフォーマンス。
        // 1000+ 同時 Tween で ID 検索が頻発する場合、
        // Dictionary<object, List<Tween>> への最適化を検討する。

        internal static void Pause(object id) => TweenManager.PauseById(id);
        internal static void Play(object id) => TweenManager.PlayById(id);
        internal static void Kill(object id, bool complete = false) => TweenManager.KillById(id, complete);
        internal static int Count(object id) => TweenManager.TweensById(id);
    }
}
