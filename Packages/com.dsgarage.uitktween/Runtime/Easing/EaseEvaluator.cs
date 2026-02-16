using UnityEngine;

namespace DSGarage.UITKTween
{
    internal static class EaseEvaluator
    {
        internal static float Evaluate(EaseType ease, float t, AnimationCurve customCurve = null)
        {
            if (ease == EaseType.Custom && customCurve != null)
                return customCurve.Evaluate(t);
            return EaseFunctions.Evaluate(ease, t);
        }
    }
}
