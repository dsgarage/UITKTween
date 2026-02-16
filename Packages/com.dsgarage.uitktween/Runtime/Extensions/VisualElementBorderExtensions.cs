using UnityEngine;
using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Border 系 Tween 拡張メソッド。
    /// DOBorderColor, DOBorderWidth, DOBorderRadius（全辺/個別）
    /// </summary>
    public static class VisualElementBorderExtensions
    {
        // === Border Color (全辺) ===

        /// <summary>全辺の borderColor をアニメーション</summary>
        public static Sequence DOBorderColor(this VisualElement target, Color endValue, float duration)
        {
            return UITKTween.Sequence()
                .Join(target.DOBorderTopColor(endValue, duration))
                .Join(target.DOBorderRightColor(endValue, duration))
                .Join(target.DOBorderBottomColor(endValue, duration))
                .Join(target.DOBorderLeftColor(endValue, duration))
                .SetTarget(target);
        }

        /// <summary>borderTopColor をアニメーション</summary>
        public static Tweener<Color> DOBorderTopColor(this VisualElement target, Color endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderTopColor,
                x => target.style.borderTopColor = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderRightColor をアニメーション</summary>
        public static Tweener<Color> DOBorderRightColor(this VisualElement target, Color endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderRightColor,
                x => target.style.borderRightColor = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderBottomColor をアニメーション</summary>
        public static Tweener<Color> DOBorderBottomColor(this VisualElement target, Color endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderBottomColor,
                x => target.style.borderBottomColor = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderLeftColor をアニメーション</summary>
        public static Tweener<Color> DOBorderLeftColor(this VisualElement target, Color endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderLeftColor,
                x => target.style.borderLeftColor = x,
                endValue, duration
            ).SetTarget(target);
        }

        // === Border Width (全辺) ===

        /// <summary>全辺の borderWidth をアニメーション</summary>
        public static Sequence DOBorderWidth(this VisualElement target, float endValue, float duration)
        {
            return UITKTween.Sequence()
                .Join(target.DOBorderTopWidth(endValue, duration))
                .Join(target.DOBorderRightWidth(endValue, duration))
                .Join(target.DOBorderBottomWidth(endValue, duration))
                .Join(target.DOBorderLeftWidth(endValue, duration))
                .SetTarget(target);
        }

        /// <summary>borderTopWidth をアニメーション</summary>
        public static Tweener<float> DOBorderTopWidth(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderTopWidth,
                x => target.style.borderTopWidth = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderRightWidth をアニメーション</summary>
        public static Tweener<float> DOBorderRightWidth(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderRightWidth,
                x => target.style.borderRightWidth = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderBottomWidth をアニメーション</summary>
        public static Tweener<float> DOBorderBottomWidth(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderBottomWidth,
                x => target.style.borderBottomWidth = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderLeftWidth をアニメーション</summary>
        public static Tweener<float> DOBorderLeftWidth(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderLeftWidth,
                x => target.style.borderLeftWidth = x,
                endValue, duration
            ).SetTarget(target);
        }

        // === Border Radius (全角) ===

        /// <summary>全角の borderRadius をアニメーション</summary>
        public static Sequence DOBorderRadius(this VisualElement target, float endValue, float duration)
        {
            return UITKTween.Sequence()
                .Join(target.DOBorderTopLeftRadius(endValue, duration))
                .Join(target.DOBorderTopRightRadius(endValue, duration))
                .Join(target.DOBorderBottomRightRadius(endValue, duration))
                .Join(target.DOBorderBottomLeftRadius(endValue, duration))
                .SetTarget(target);
        }

        /// <summary>borderTopLeftRadius をアニメーション</summary>
        public static Tweener<float> DOBorderTopLeftRadius(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderTopLeftRadius,
                x => target.style.borderTopLeftRadius = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderTopRightRadius をアニメーション</summary>
        public static Tweener<float> DOBorderTopRightRadius(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderTopRightRadius,
                x => target.style.borderTopRightRadius = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderBottomRightRadius をアニメーション</summary>
        public static Tweener<float> DOBorderBottomRightRadius(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderBottomRightRadius,
                x => target.style.borderBottomRightRadius = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>borderBottomLeftRadius をアニメーション</summary>
        public static Tweener<float> DOBorderBottomLeftRadius(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.borderBottomLeftRadius,
                x => target.style.borderBottomLeftRadius = x,
                endValue, duration
            ).SetTarget(target);
        }
    }
}
