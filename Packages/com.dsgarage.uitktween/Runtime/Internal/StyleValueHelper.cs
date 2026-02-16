using UnityEngine;
using UnityEngine.UIElements;

namespace DSGarage.UITKTween.Internal
{
    /// <summary>
    /// UI Toolkit の StyleLength/StyleFloat/StyleColor の安全な読み書きヘルパー。
    /// style は Auto/Initial キーワードを返す場合があるため、resolvedStyle からフォールバック取得する。
    /// </summary>
    internal static class StyleValueHelper
    {
        // === Float 値の取得 ===

        internal static float GetStyleFloat(StyleFloat styleValue, float resolvedValue)
        {
            if (styleValue.keyword == StyleKeyword.Undefined)
                return styleValue.value;
            return resolvedValue;
        }

        // === Length 値の取得 ===

        internal static float GetStyleLength(StyleLength styleLength, float resolvedValue)
        {
            if (styleLength.keyword == StyleKeyword.Undefined)
                return styleLength.value.value;
            return resolvedValue;
        }

        // === Translate の取得 ===

        internal static Vector2 GetTranslate(VisualElement element)
        {
            var t = element.resolvedStyle.translate;
            return new Vector2(t.x.value, t.y.value);
        }

        // === Scale の取得 ===

        internal static Vector2 GetScale(VisualElement element)
        {
            var s = element.resolvedStyle.scale;
            return new Vector2(s.value.x, s.value.y);
        }

        // === Rotate の取得 ===

        internal static float GetRotate(VisualElement element)
        {
            return element.resolvedStyle.rotate.angle.value;
        }

        // === Opacity の取得 ===

        internal static float GetOpacity(VisualElement element)
        {
            return element.resolvedStyle.opacity;
        }

        // === BackgroundColor の取得 ===

        internal static Color GetBackgroundColor(VisualElement element)
        {
            return element.resolvedStyle.backgroundColor;
        }

        // === TextColor の取得 ===

        internal static Color GetTextColor(VisualElement element)
        {
            return element.resolvedStyle.color;
        }

        // === Position 値の取得 ===

        internal static float GetLeft(VisualElement element)
        {
            return GetStyleLength(element.style.left, element.resolvedStyle.left);
        }

        internal static float GetTop(VisualElement element)
        {
            return GetStyleLength(element.style.top, element.resolvedStyle.top);
        }

        internal static float GetRight(VisualElement element)
        {
            return GetStyleLength(element.style.right, element.resolvedStyle.right);
        }

        internal static float GetBottom(VisualElement element)
        {
            return GetStyleLength(element.style.bottom, element.resolvedStyle.bottom);
        }

        // === Size 値の取得 ===

        internal static float GetWidth(VisualElement element)
        {
            return GetStyleLength(element.style.width, element.resolvedStyle.width);
        }

        internal static float GetHeight(VisualElement element)
        {
            return GetStyleLength(element.style.height, element.resolvedStyle.height);
        }

        // === Border ===

        internal static float GetBorderWidth(VisualElement element, BorderSide side)
        {
            switch (side)
            {
                case BorderSide.Top: return element.resolvedStyle.borderTopWidth;
                case BorderSide.Right: return element.resolvedStyle.borderRightWidth;
                case BorderSide.Bottom: return element.resolvedStyle.borderBottomWidth;
                case BorderSide.Left: return element.resolvedStyle.borderLeftWidth;
                default: return 0f;
            }
        }

        internal static Color GetBorderColor(VisualElement element, BorderSide side)
        {
            switch (side)
            {
                case BorderSide.Top: return element.resolvedStyle.borderTopColor;
                case BorderSide.Right: return element.resolvedStyle.borderRightColor;
                case BorderSide.Bottom: return element.resolvedStyle.borderBottomColor;
                case BorderSide.Left: return element.resolvedStyle.borderLeftColor;
                default: return Color.clear;
            }
        }

        // === Margin/Padding ===

        internal static float GetMargin(VisualElement element, BorderSide side)
        {
            switch (side)
            {
                case BorderSide.Top: return element.resolvedStyle.marginTop;
                case BorderSide.Right: return element.resolvedStyle.marginRight;
                case BorderSide.Bottom: return element.resolvedStyle.marginBottom;
                case BorderSide.Left: return element.resolvedStyle.marginLeft;
                default: return 0f;
            }
        }

        internal static float GetPadding(VisualElement element, BorderSide side)
        {
            switch (side)
            {
                case BorderSide.Top: return element.resolvedStyle.paddingTop;
                case BorderSide.Right: return element.resolvedStyle.paddingRight;
                case BorderSide.Bottom: return element.resolvedStyle.paddingBottom;
                case BorderSide.Left: return element.resolvedStyle.paddingLeft;
                default: return 0f;
            }
        }
    }

    internal enum BorderSide
    {
        Top,
        Right,
        Bottom,
        Left
    }
}
