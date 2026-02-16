using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Text 系 Tween 拡張メソッド。
    /// DOFontSize, DOLetterSpacing, DOWordSpacing
    /// </summary>
    public static class VisualElementTextExtensions
    {
        /// <summary>style.fontSize をアニメーション</summary>
        public static Tweener<float> DOFontSize(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.fontSize,
                x => target.style.fontSize = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.letterSpacing をアニメーション</summary>
        public static Tweener<float> DOLetterSpacing(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.letterSpacing,
                x => target.style.letterSpacing = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.wordSpacing をアニメーション</summary>
        public static Tweener<float> DOWordSpacing(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => target.resolvedStyle.wordSpacing,
                x => target.style.wordSpacing = x,
                endValue, duration
            ).SetTarget(target);
        }
    }
}
