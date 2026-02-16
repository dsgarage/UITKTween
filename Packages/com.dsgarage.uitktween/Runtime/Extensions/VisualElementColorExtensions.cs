using UnityEngine;
using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Color 系 Tween 拡張メソッド。
    /// DOFade, DOColor, DOTextColor
    /// </summary>
    public static class VisualElementColorExtensions
    {
        /// <summary>style.opacity をアニメーション</summary>
        public static Tweener<float> DOFade(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetOpacity(target),
                x => target.style.opacity = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.backgroundColor をアニメーション</summary>
        public static Tweener<Color> DOColor(this VisualElement target, Color endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetBackgroundColor(target),
                x => target.style.backgroundColor = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.color（テキスト色）をアニメーション</summary>
        public static Tweener<Color> DOTextColor(this VisualElement target, Color endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetTextColor(target),
                x => target.style.color = x,
                endValue, duration
            ).SetTarget(target);
        }
    }
}
