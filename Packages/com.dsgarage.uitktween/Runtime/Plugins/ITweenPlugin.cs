namespace DSGarage.UITKTween.Plugins
{
    internal interface ITweenPlugin<T> where T : struct
    {
        T Evaluate(T start, T end, float t);
        T Add(T a, T b);
        T Subtract(T a, T b);
    }
}
