using UnityEngine;
using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Size 系 Tween 拡張メソッド。
    /// DOWidth, DOHeight, DOSize
    /// </summary>
    public static class VisualElementSizeExtensions
    {
        /// <summary>style.width をアニメーション</summary>
        public static Tweener<float> DOWidth(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetWidth(target),
                x => target.style.width = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>style.height をアニメーション</summary>
        public static Tweener<float> DOHeight(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetHeight(target),
                x => target.style.height = x,
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>width と height を同時にアニメーション（Sequence で並行実行）</summary>
        public static Sequence DOSize(this VisualElement target, Vector2 endValue, float duration)
        {
            return UITKTween.Sequence()
                .Join(target.DOWidth(endValue.x, duration))
                .Join(target.DOHeight(endValue.y, duration))
                .SetTarget(target);
        }
    }
}
