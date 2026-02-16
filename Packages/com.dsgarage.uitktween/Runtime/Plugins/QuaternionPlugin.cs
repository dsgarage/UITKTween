using UnityEngine;

namespace DSGarage.UITKTween.Plugins
{
    /// <summary>
    /// Quaternion 用プラグイン。Slerp で補間する。
    /// </summary>
    internal struct QuaternionPlugin : ITweenPlugin<Quaternion>
    {
        public static readonly QuaternionPlugin Instance = new();

        public Quaternion Evaluate(Quaternion start, Quaternion end, float t) => Quaternion.SlerpUnclamped(start, end, t);

        public Quaternion Add(Quaternion a, Quaternion b) => a * b;

        public Quaternion Subtract(Quaternion a, Quaternion b) => Quaternion.Inverse(b) * a;
    }
}
