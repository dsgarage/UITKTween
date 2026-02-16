namespace DSGarage.UITKTween
{
    public enum EaseType
    {
        Linear,
        InQuad, OutQuad, InOutQuad,
        InCubic, OutCubic, InOutCubic,
        InQuart, OutQuart, InOutQuart,
        InQuint, OutQuint, InOutQuint,
        InSine, OutSine, InOutSine,
        InExpo, OutExpo, InOutExpo,
        InCirc, OutCirc, InOutCirc,
        InElastic, OutElastic, InOutElastic,
        InBack, OutBack, InOutBack,
        InBounce, OutBounce, InOutBounce,
        Custom
    }

    public enum LoopType
    {
        Restart,
        Yoyo,
        Incremental
    }

    public enum UpdateType
    {
        Normal,
        Late,
        Manual
    }
}
