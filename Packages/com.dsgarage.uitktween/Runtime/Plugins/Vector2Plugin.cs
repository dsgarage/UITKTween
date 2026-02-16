using UnityEngine;

namespace DSGarage.UITKTween.Plugins
{
    internal struct Vector2Plugin : ITweenPlugin<Vector2>
    {
        public static readonly Vector2Plugin Instance = new();

        public Vector2 Evaluate(Vector2 start, Vector2 end, float t) => Vector2.LerpUnclamped(start, end, t);
        public Vector2 Add(Vector2 a, Vector2 b) => a + b;
        public Vector2 Subtract(Vector2 a, Vector2 b) => a - b;
    }
}
