using System;
using UnityEngine;
using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の特殊エフェクト拡張メソッド。
    /// DOPunchPosition/Rotation/Scale, DOShakePosition/Rotation/Scale
    /// </summary>
    public static class VisualElementSpecialExtensions
    {
        // === Punch ===

        /// <summary>
        /// 位置をパンチ（弾性バウンス付きの揺れ）。
        /// punch: 揺れの方向と強さ、vibrato: 振動回数、elasticity: 0-1 の反発弾性
        /// </summary>
        public static Tweener<float> DOPunchPosition(this VisualElement target, Vector2 punch, float duration, int vibrato = 10, float elasticity = 1f)
        {
            PanelLifecycleTracker.Track(target);
            var startPos = StyleValueHelper.GetTranslate(target);

            return UITKTween.To(
                () => 0f,
                t =>
                {
                    Vector2 offset = PunchEvaluate(punch, t, vibrato, elasticity);
                    target.style.translate = new Translate(startPos.x + offset.x, startPos.y + offset.y, 0);
                },
                1f, duration
            ).SetEase(EaseType.Linear).SetTarget(target);
        }

        /// <summary>回転をパンチ</summary>
        public static Tweener<float> DOPunchRotation(this VisualElement target, float punch, float duration, int vibrato = 10, float elasticity = 1f)
        {
            PanelLifecycleTracker.Track(target);
            var startRot = StyleValueHelper.GetRotate(target);

            return UITKTween.To(
                () => 0f,
                t =>
                {
                    float offset = PunchEvaluateFloat(punch, t, vibrato, elasticity);
                    target.style.rotate = new Rotate(startRot + offset);
                },
                1f, duration
            ).SetEase(EaseType.Linear).SetTarget(target);
        }

        /// <summary>スケールをパンチ</summary>
        public static Tweener<float> DOPunchScale(this VisualElement target, Vector2 punch, float duration, int vibrato = 10, float elasticity = 1f)
        {
            PanelLifecycleTracker.Track(target);
            var startScale = StyleValueHelper.GetScale(target);

            return UITKTween.To(
                () => 0f,
                t =>
                {
                    Vector2 offset = PunchEvaluate(punch, t, vibrato, elasticity);
                    target.style.scale = new Scale(new Vector3(startScale.x + offset.x, startScale.y + offset.y, 1f));
                },
                1f, duration
            ).SetEase(EaseType.Linear).SetTarget(target);
        }

        // === Shake ===

        /// <summary>
        /// 位置をシェイク（ランダム揺れ）。
        /// strength: 揺れの強さ、vibrato: 振動回数、fadeOut: フェードアウト
        /// </summary>
        public static Tweener<float> DOShakePosition(this VisualElement target, float duration, Vector2 strength, int vibrato = 10, bool fadeOut = true)
        {
            PanelLifecycleTracker.Track(target);
            var startPos = StyleValueHelper.GetTranslate(target);
            int seed = target.GetHashCode() + Environment.TickCount;

            return UITKTween.To(
                () => 0f,
                t =>
                {
                    Vector2 offset = ShakeEvaluate(strength, t, vibrato, fadeOut, ref seed);
                    target.style.translate = new Translate(startPos.x + offset.x, startPos.y + offset.y, 0);
                },
                1f, duration
            ).SetEase(EaseType.Linear).SetTarget(target);
        }

        /// <summary>位置をシェイク（均一の強さ）</summary>
        public static Tweener<float> DOShakePosition(this VisualElement target, float duration, float strength = 10f, int vibrato = 10, bool fadeOut = true)
        {
            return target.DOShakePosition(duration, new Vector2(strength, strength), vibrato, fadeOut);
        }

        /// <summary>回転をシェイク</summary>
        public static Tweener<float> DOShakeRotation(this VisualElement target, float duration, float strength = 90f, int vibrato = 10, bool fadeOut = true)
        {
            PanelLifecycleTracker.Track(target);
            var startRot = StyleValueHelper.GetRotate(target);
            int seed = target.GetHashCode() + Environment.TickCount;

            return UITKTween.To(
                () => 0f,
                t =>
                {
                    float offset = ShakeEvaluateFloat(strength, t, vibrato, fadeOut, ref seed);
                    target.style.rotate = new Rotate(startRot + offset);
                },
                1f, duration
            ).SetEase(EaseType.Linear).SetTarget(target);
        }

        /// <summary>スケールをシェイク</summary>
        public static Tweener<float> DOShakeScale(this VisualElement target, float duration, Vector2 strength, int vibrato = 10, bool fadeOut = true)
        {
            PanelLifecycleTracker.Track(target);
            var startScale = StyleValueHelper.GetScale(target);
            int seed = target.GetHashCode() + Environment.TickCount;

            return UITKTween.To(
                () => 0f,
                t =>
                {
                    Vector2 offset = ShakeEvaluate(strength, t, vibrato, fadeOut, ref seed);
                    target.style.scale = new Scale(new Vector3(startScale.x + offset.x, startScale.y + offset.y, 1f));
                },
                1f, duration
            ).SetEase(EaseType.Linear).SetTarget(target);
        }

        /// <summary>スケールをシェイク（均一の強さ）</summary>
        public static Tweener<float> DOShakeScale(this VisualElement target, float duration, float strength = 0.2f, int vibrato = 10, bool fadeOut = true)
        {
            return target.DOShakeScale(duration, new Vector2(strength, strength), vibrato, fadeOut);
        }

        // === 内部計算 ===

        /// <summary>減衰サイン波によるパンチ計算（Vector2）</summary>
        private static Vector2 PunchEvaluate(Vector2 punch, float t, int vibrato, float elasticity)
        {
            if (t <= 0f) return Vector2.zero;
            if (t >= 1f) return Vector2.zero;

            float decay = 1f - t;
            float frequency = vibrato * Mathf.PI * t;
            float sin = Mathf.Sin(frequency);
            float dampedSin = sin * decay;

            // elasticity で反発の振れ幅を制御
            float bounceMultiplier = Mathf.Lerp(Mathf.Abs(dampedSin), dampedSin, elasticity);

            return punch * bounceMultiplier;
        }

        /// <summary>減衰サイン波によるパンチ計算（float）</summary>
        private static float PunchEvaluateFloat(float punch, float t, int vibrato, float elasticity)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 0f;

            float decay = 1f - t;
            float frequency = vibrato * Mathf.PI * t;
            float sin = Mathf.Sin(frequency);
            float dampedSin = sin * decay;

            float bounceMultiplier = Mathf.Lerp(Mathf.Abs(dampedSin), dampedSin, elasticity);

            return punch * bounceMultiplier;
        }

        /// <summary>ランダム揺れ計算（Vector2）</summary>
        private static Vector2 ShakeEvaluate(Vector2 strength, float t, int vibrato, bool fadeOut, ref int seed)
        {
            if (t <= 0f) return Vector2.zero;
            if (t >= 1f) return Vector2.zero;

            float decay = fadeOut ? (1f - t) : 1f;
            float frequency = vibrato * Mathf.PI * t;
            float sin = Mathf.Sin(frequency);

            // 決定論的な擬似乱数
            float rx = PseudoRandom(ref seed) * 2f - 1f;
            float ry = PseudoRandom(ref seed) * 2f - 1f;

            return new Vector2(
                strength.x * sin * decay * rx,
                strength.y * sin * decay * ry
            );
        }

        /// <summary>ランダム揺れ計算（float）</summary>
        private static float ShakeEvaluateFloat(float strength, float t, int vibrato, bool fadeOut, ref int seed)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 0f;

            float decay = fadeOut ? (1f - t) : 1f;
            float frequency = vibrato * Mathf.PI * t;
            float sin = Mathf.Sin(frequency);

            float r = PseudoRandom(ref seed) * 2f - 1f;

            return strength * sin * decay * r;
        }

        /// <summary>決定論的擬似乱数（Mulberry32 簡易版）</summary>
        private static float PseudoRandom(ref int seed)
        {
            seed = seed * 1103515245 + 12345;
            return ((seed >> 16) & 0x7FFF) / (float)0x7FFF;
        }
    }
}
