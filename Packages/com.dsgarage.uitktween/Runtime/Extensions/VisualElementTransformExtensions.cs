using UnityEngine;
using UnityEngine.UIElements;
using DSGarage.UITKTween.Internal;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// VisualElement の Transform 系 Tween 拡張メソッド。
    /// DOMove, DOMoveX/Y, DORotate, DOScale
    /// </summary>
    public static class VisualElementTransformExtensions
    {
        // === Move (translate) ===

        /// <summary>translate を指定位置にアニメーション</summary>
        public static Tweener<Vector2> DOMove(this VisualElement target, Vector2 endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetTranslate(target),
                x => target.style.translate = new Translate(x.x, x.y, 0),
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>translate の X 軸のみアニメーション</summary>
        public static Tweener<float> DOMoveX(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetTranslate(target).x,
                x =>
                {
                    var current = StyleValueHelper.GetTranslate(target);
                    target.style.translate = new Translate(x, current.y, 0);
                },
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>translate の Y 軸のみアニメーション</summary>
        public static Tweener<float> DOMoveY(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetTranslate(target).y,
                y =>
                {
                    var current = StyleValueHelper.GetTranslate(target);
                    target.style.translate = new Translate(current.x, y, 0);
                },
                endValue, duration
            ).SetTarget(target);
        }

        // === Rotate ===

        /// <summary>rotate をアニメーション（度数）</summary>
        public static Tweener<float> DORotate(this VisualElement target, float endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetRotate(target),
                x => target.style.rotate = new Rotate(x),
                endValue, duration
            ).SetTarget(target);
        }

        // === Scale ===

        /// <summary>scale を Vector2 でアニメーション</summary>
        public static Tweener<Vector2> DOScale(this VisualElement target, Vector2 endValue, float duration)
        {
            PanelLifecycleTracker.Track(target);
            return UITKTween.To(
                () => StyleValueHelper.GetScale(target),
                x => target.style.scale = new Scale(new Vector3(x.x, x.y, 1f)),
                endValue, duration
            ).SetTarget(target);
        }

        /// <summary>scale を均一にアニメーション</summary>
        public static Tweener<Vector2> DOScale(this VisualElement target, float endValue, float duration)
        {
            return target.DOScale(new Vector2(endValue, endValue), duration);
        }
    }
}
