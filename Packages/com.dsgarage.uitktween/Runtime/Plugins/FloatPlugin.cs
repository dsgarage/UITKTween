using UnityEngine;

namespace DSGarage.UITKTween.Plugins
{
    internal struct FloatPlugin : ITweenPlugin<float>
    {
        public static readonly FloatPlugin Instance = new();

        public float Evaluate(float start, float end, float t) => Mathf.LerpUnclamped(start, end, t);
        public float Add(float a, float b) => a + b;
        public float Subtract(float a, float b) => a - b;
    }
}
