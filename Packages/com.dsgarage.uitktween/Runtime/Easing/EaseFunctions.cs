using UnityEngine;

namespace DSGarage.UITKTween
{
    internal static class EaseFunctions
    {
        private const float PI = Mathf.PI;
        private const float HALF_PI = Mathf.PI * 0.5f;
        private const float BACK_OVERSHOOT = 1.70158f;

        internal static float Evaluate(EaseType ease, float t)
        {
            switch (ease)
            {
                case EaseType.Linear: return t;

                // Quad
                case EaseType.InQuad: return t * t;
                case EaseType.OutQuad: return 1f - (1f - t) * (1f - t);
                case EaseType.InOutQuad: return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2) * 0.5f;

                // Cubic
                case EaseType.InCubic: return t * t * t;
                case EaseType.OutCubic: return 1f - Mathf.Pow(1f - t, 3);
                case EaseType.InOutCubic: return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3) * 0.5f;

                // Quart
                case EaseType.InQuart: return t * t * t * t;
                case EaseType.OutQuart: return 1f - Mathf.Pow(1f - t, 4);
                case EaseType.InOutQuart: return t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4) * 0.5f;

                // Quint
                case EaseType.InQuint: return t * t * t * t * t;
                case EaseType.OutQuint: return 1f - Mathf.Pow(1f - t, 5);
                case EaseType.InOutQuint: return t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5) * 0.5f;

                // Sine
                case EaseType.InSine: return 1f - Mathf.Cos(t * HALF_PI);
                case EaseType.OutSine: return Mathf.Sin(t * HALF_PI);
                case EaseType.InOutSine: return -(Mathf.Cos(PI * t) - 1f) * 0.5f;

                // Expo
                case EaseType.InExpo: return t <= 0f ? 0f : Mathf.Pow(2f, 10f * t - 10f);
                case EaseType.OutExpo: return t >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
                case EaseType.InOutExpo:
                    if (t <= 0f) return 0f;
                    if (t >= 1f) return 1f;
                    return t < 0.5f
                        ? Mathf.Pow(2f, 20f * t - 10f) * 0.5f
                        : (2f - Mathf.Pow(2f, -20f * t + 10f)) * 0.5f;

                // Circ
                case EaseType.InCirc: return 1f - Mathf.Sqrt(1f - t * t);
                case EaseType.OutCirc: return Mathf.Sqrt(1f - (t - 1f) * (t - 1f));
                case EaseType.InOutCirc:
                    return t < 0.5f
                        ? (1f - Mathf.Sqrt(1f - 4f * t * t)) * 0.5f
                        : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2)) + 1f) * 0.5f;

                // Elastic
                case EaseType.InElastic:
                    if (t <= 0f) return 0f;
                    if (t >= 1f) return 1f;
                    return -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * (2f * PI / 3f));
                case EaseType.OutElastic:
                    if (t <= 0f) return 0f;
                    if (t >= 1f) return 1f;
                    return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * (2f * PI / 3f)) + 1f;
                case EaseType.InOutElastic:
                    if (t <= 0f) return 0f;
                    if (t >= 1f) return 1f;
                    return t < 0.5f
                        ? -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * (2f * PI / 4.5f))) * 0.5f
                        : Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * (2f * PI / 4.5f)) * 0.5f + 1f;

                // Back
                case EaseType.InBack: return (BACK_OVERSHOOT + 1f) * t * t * t - BACK_OVERSHOOT * t * t;
                case EaseType.OutBack:
                {
                    float t1 = t - 1f;
                    return 1f + (BACK_OVERSHOOT + 1f) * t1 * t1 * t1 + BACK_OVERSHOOT * t1 * t1;
                }
                case EaseType.InOutBack:
                {
                    float s = BACK_OVERSHOOT * 1.525f;
                    return t < 0.5f
                        ? (Mathf.Pow(2f * t, 2) * ((s + 1f) * 2f * t - s)) * 0.5f
                        : (Mathf.Pow(2f * t - 2f, 2) * ((s + 1f) * (t * 2f - 2f) + s) + 2f) * 0.5f;
                }

                // Bounce
                case EaseType.InBounce: return 1f - BounceOut(1f - t);
                case EaseType.OutBounce: return BounceOut(t);
                case EaseType.InOutBounce:
                    return t < 0.5f
                        ? (1f - BounceOut(1f - 2f * t)) * 0.5f
                        : (1f + BounceOut(2f * t - 1f)) * 0.5f;

                default: return t;
            }
        }

        private static float BounceOut(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (t < 1f / d1)
                return n1 * t * t;
            if (t < 2f / d1)
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            if (t < 2.5f / d1)
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }
}
