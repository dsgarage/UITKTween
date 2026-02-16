using UnityEngine;

namespace DSGarage.UITKTween.Plugins
{
    internal struct Vector3Plugin : ITweenPlugin<Vector3>
    {
        public static readonly Vector3Plugin Instance = new();

        public Vector3 Evaluate(Vector3 start, Vector3 end, float t) => Vector3.LerpUnclamped(start, end, t);
        public Vector3 Add(Vector3 a, Vector3 b) => a + b;
        public Vector3 Subtract(Vector3 a, Vector3 b) => a - b;
    }
}
