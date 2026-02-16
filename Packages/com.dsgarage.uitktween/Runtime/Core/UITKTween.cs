using System;
using UnityEngine;
using DSGarage.UITKTween.Plugins;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// UITKTween の公開静的ファサード。
    /// DOTween ライクな API で UI Toolkit の VisualElement をアニメーションする。
    /// </summary>
    public static class UITKTween
    {
        // === デフォルト設定 ===

        public static EaseType defaultEase = EaseType.OutQuad;
        public static bool defaultAutoKill = true;

        // === ジェネリック To ===

        /// <summary>
        /// 汎用 Tween を作成する。プラグインを明示指定。
        /// </summary>
        public static Tweener<T> To<T, TPlugin>(Func<T> getter, Action<T> setter, T endValue, float duration)
            where T : struct
            where TPlugin : struct, ITweenPlugin<T>
        {
            TweenUpdateRunner.EnsureInitialized();
            var tweener = new Tweener<T>();
            tweener.Setup(getter, setter, endValue, duration, default(TPlugin));
            tweener.easeType = defaultEase;
            tweener.autoKill = defaultAutoKill;
            TweenManager.Add(tweener);
            return tweener;
        }

        // === 型別ショートカット ===

        /// <summary>float の Tween</summary>
        public static Tweener<float> To(Func<float> getter, Action<float> setter, float endValue, float duration)
        {
            return To<float, FloatPlugin>(getter, setter, endValue, duration);
        }

        /// <summary>Vector2 の Tween</summary>
        public static Tweener<Vector2> To(Func<Vector2> getter, Action<Vector2> setter, Vector2 endValue, float duration)
        {
            return To<Vector2, Vector2Plugin>(getter, setter, endValue, duration);
        }

        /// <summary>Vector3 の Tween</summary>
        public static Tweener<Vector3> To(Func<Vector3> getter, Action<Vector3> setter, Vector3 endValue, float duration)
        {
            return To<Vector3, Vector3Plugin>(getter, setter, endValue, duration);
        }

        /// <summary>Color の Tween</summary>
        public static Tweener<Color> To(Func<Color> getter, Action<Color> setter, Color endValue, float duration)
        {
            return To<Color, ColorPlugin>(getter, setter, endValue, duration);
        }

        // === Sequence ===

        /// <summary>新しい Sequence を作成</summary>
        public static Sequence Sequence()
        {
            TweenUpdateRunner.EnsureInitialized();
            var seq = new Sequence();
            seq.isActive = true;
            seq.isPlaying = true;
            seq.delayComplete = true;
            seq.easeType = EaseType.Linear; // Sequence はデフォルト Linear
            seq.autoKill = defaultAutoKill;
            TweenManager.Add(seq);
            return seq;
        }

        // === グローバル制御 ===

        /// <summary>全 Tween を一時停止</summary>
        public static void PauseAll() => TweenManager.PauseAll();

        /// <summary>全 Tween を再生</summary>
        public static void PlayAll() => TweenManager.PlayAll();

        /// <summary>全 Tween を Kill</summary>
        public static void KillAll(bool complete = false) => TweenManager.KillAll(complete);

        // === ID/Target による制御 ===

        /// <summary>指定 ID の Tween を一時停止</summary>
        public static void Pause(object id) => TweenManager.PauseById(id);

        /// <summary>指定 ID の Tween を再生</summary>
        public static void Play(object id) => TweenManager.PlayById(id);

        /// <summary>指定 ID の Tween を Kill</summary>
        public static void Kill(object id, bool complete = false) => TweenManager.KillById(id, complete);

        /// <summary>指定 Target に紐づく Tween を一時停止</summary>
        public static void PauseTarget(object target) => TweenManager.PauseByTarget(target);

        /// <summary>指定 Target に紐づく Tween を再生</summary>
        public static void PlayTarget(object target) => TweenManager.PlayByTarget(target);

        /// <summary>指定 Target に紐づく Tween を Kill</summary>
        public static void KillTarget(object target, bool complete = false) => TweenManager.KillByTarget(target, complete);

        // === Manual Update ===

        /// <summary>UpdateType.Manual の Tween を手動更新</summary>
        public static void ManualUpdate(float deltaTime) => TweenManager.Update(deltaTime, UpdateType.Manual);

        // === ユーティリティ ===

        /// <summary>アクティブな Tween の数を返す</summary>
        public static int ActiveTweenCount => TweenManager.ActiveCount;

        /// <summary>指定 ID の Tween 数を返す</summary>
        public static int TweensById(object id) => TweenManager.TweensById(id);

        /// <summary>指定 Target の Tween 数を返す</summary>
        public static int TweensByTarget(object target) => TweenManager.TweensByTarget(target);

        // === プール管理 ===

        /// <summary>プールを事前確保してGCを削減</summary>
        public static void SetCapacity(int tweenerCapacity, int sequenceCapacity)
        {
            TweenPool.Warmup<Tweener<float>>(tweenerCapacity);
            TweenPool.Warmup<Tweener<Vector2>>(tweenerCapacity);
            TweenPool.Warmup<Tweener<Vector3>>(tweenerCapacity);
            TweenPool.Warmup<Tweener<Color>>(tweenerCapacity);
            TweenPool.Warmup<Sequence>(sequenceCapacity);
        }

        /// <summary>全プールをクリア</summary>
        public static void ClearPools() => TweenPool.ClearAll();
    }
}
