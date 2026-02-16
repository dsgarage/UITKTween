using UnityEngine;

namespace DSGarage.UITKTween.Plugins
{
    /// <summary>
    /// StyleLength 用プラグイン。
    /// StyleLength は内部的に float で補間し、Length 型への変換は拡張メソッド側で行う。
    /// FloatPlugin と同一の実装だが、意味的に分離。
    /// </summary>
    internal struct LengthPlugin : ITweenPlugin<float>
    {
        public static readonly LengthPlugin Instance = new();

        public float Evaluate(float start, float end, float t) => Mathf.LerpUnclamped(start, end, t);
        public float Add(float a, float b) => a + b;
        public float Subtract(float a, float b) => a - b;
    }
}
