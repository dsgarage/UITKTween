using UnityEngine;

namespace DSGarage.UITKTween.Plugins
{
    internal struct ColorPlugin : ITweenPlugin<Color>
    {
        public static readonly ColorPlugin Instance = new();

        public Color Evaluate(Color start, Color end, float t) => Color.LerpUnclamped(start, end, t);
        public Color Add(Color a, Color b) => a + b;
        public Color Subtract(Color a, Color b) => a - b;
    }
}
