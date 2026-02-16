using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Position 系 Tween 拡張メソッド。
    /// DOLeft, DOTop, DORight, DOBottom
    /// </summary>
    public static class VisualElementPositionExtensions
    {
        /// <summary>style.left をアニメーション</summary>
        public static Tweener<float> DOLeft(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetLeft(target),
                x => target.style.left = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.top をアニメーション</summary>
        public static Tweener<float> DOTop(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetTop(target),
                x => target.style.top = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.right をアニメーション</summary>
        public static Tweener<float> DORight(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetRight(target),
                x => target.style.right = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.bottom をアニメーション</summary>
        public static Tweener<float> DOBottom(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetBottom(target),
                x => target.style.bottom = x,
                endValue, duration
            ).SetTarget(target);
        }
    }
}
