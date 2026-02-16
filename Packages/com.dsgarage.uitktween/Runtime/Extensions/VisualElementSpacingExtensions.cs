using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Spacing 系 Tween 拡張メソッド。
    /// DOMargin, DOPadding（全辺/個別）
    /// </summary>
    public static class VisualElementSpacingExtensions
    {
        // === Margin (全辺) ===

        /// <summary>全辺の margin をアニメーション</summary>
        public static Sequence DOMargin(this VisualElement target, float endValue, float duration)
        {
            return UITKTween.Sequence()
                .Join(target.DOMarginTop(endValue, duration))
                .Join(target.DOMarginRight(endValue, duration))
                .Join(target.DOMarginBottom(endValue, duration))
                .Join(target.DOMarginLeft(endValue, duration))
                .SetTarget(target);
        }

        /// <summary>marginTop をアニメーション</summary>
        public static Tweener<float> DOMarginTop(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.marginTop,
                x => target.style.marginTop = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>marginRight をアニメーション</summary>
        public static Tweener<float> DOMarginRight(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.marginRight,
                x => target.style.marginRight = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>marginBottom をアニメーション</summary>
        public static Tweener<float> DOMarginBottom(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.marginBottom,
                x => target.style.marginBottom = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>marginLeft をアニメーション</summary>
        public static Tweener<float> DOMarginLeft(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.marginLeft,
                x => target.style.marginLeft = x,
                endValue, duration
            ).SetTarget(target);
        }

        // === Padding (全辺) ===

        /// <summary>全辺の padding をアニメーション</summary>
        public static Sequence DOPadding(this VisualElement target, float endValue, float duration)
        {
            return UITKTween.Sequence()
                .Join(target.DOPaddingTop(endValue, duration))
                .Join(target.DOPaddingRight(endValue, duration))
                .Join(target.DOPaddingBottom(endValue, duration))
                .Join(target.DOPaddingLeft(endValue, duration))
                .SetTarget(target);
        }

        /// <summary>paddingTop をアニメーション</summary>
        public static Tweener<float> DOPaddingTop(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.paddingTop,
                x => target.style.paddingTop = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>paddingRight をアニメーション</summary>
        public static Tweener<float> DOPaddingRight(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.paddingRight,
                x => target.style.paddingRight = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>paddingBottom をアニメーション</summary>
        public static Tweener<float> DOPaddingBottom(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.paddingBottom,
                x => target.style.paddingBottom = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>paddingLeft をアニメーション</summary>
        public static Tweener<float> DOPaddingLeft(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.paddingLeft,
                x => target.style.paddingLeft = x,
                endValue, duration
            ).SetTarget(target);
        }
    }
}
