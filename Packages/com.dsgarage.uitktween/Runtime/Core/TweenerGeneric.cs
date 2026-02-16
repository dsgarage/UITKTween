using System;
using DSGarage.UITKTween.Plugins;

namespace DSGarage.UITKTween
{
    public class Tweener<T> : Tween where T : struct
    {
        internal Func<T> getter;
        internal Action<T> setter;
        internal T startValue;
        internal T endValue;
        internal T changeValue;
        internal bool isFrom;
        internal bool startValueInitialized;

        // プラグインをデリゲートとして保持（struct boxing回避）
        internal Func<T, T, float, T> evaluateFunc;
        internal Func<T, T, T> addFunc;
        internal Func<T, T, T> subtractFunc;

        public T StartValue => startValue;
        public T EndValue => endValue;

        // === From ===

        public Tweener<T> From()
        {
            isFrom = true;
            return this;
        }

        public Tweener<T> From(T fromValue)
        {
            isFrom = true;
            startValue = fromValue;
            startValueInitialized = true;
            return this;
        }

        // === Fluent overrides ===

        public new Tweener<T> SetEase(EaseType ease) { base.SetEase(ease); return this; }
        public new Tweener<T> SetEase(UnityEngine.AnimationCurve curve) { base.SetEase(curve); return this; }
        public new Tweener<T> SetLoops(int loops, LoopType loopType = LoopType.Restart) { base.SetLoops(loops, loopType); return this; }
        public new Tweener<T> SetDelay(float delay) { base.SetDelay(delay); return this; }
        public new Tweener<T> SetRelative(bool isRelative = true) { base.SetRelative(isRelative); return this; }
        public new Tweener<T> SetAutoKill(bool autoKill = true) { base.SetAutoKill(autoKill); return this; }
        public new Tweener<T> SetId(object id) { base.SetId(id); return this; }
        public new Tweener<T> SetTarget(object target) { base.SetTarget(target); return this; }
        public new Tweener<T> SetUpdate(UpdateType updateType) { base.SetUpdate(updateType); return this; }
        public new Tweener<T> SetTimeScale(float timeScale) { base.SetTimeScale(timeScale); return this; }
        public new Tweener<T> OnStart(Action callback) { base.OnStart(callback); return this; }
        public new Tweener<T> OnPlay(Action callback) { base.OnPlay(callback); return this; }
        public new Tweener<T> OnUpdate(Action<float> callback) { base.OnUpdate(callback); return this; }
        public new Tweener<T> OnComplete(Action callback) { base.OnComplete(callback); return this; }
        public new Tweener<T> OnKill(Action callback) { base.OnKill(callback); return this; }
        public new Tweener<T> OnStepComplete(Action callback) { base.OnStepComplete(callback); return this; }

        // === Internal ===

        internal override void Startup()
        {
            if (!startValueInitialized && getter != null)
            {
                startValue = getter();
                startValueInitialized = true;
            }

            if (isFrom)
            {
                // From: start→end を逆転。即座にstartValueへジャンプ
                T temp = startValue;
                startValue = endValue;
                endValue = temp;
            }

            if (isRelative && subtractFunc != null)
            {
                endValue = addFunc(startValue, endValue);
            }
        }

        protected override void ApplyValue(float easedTime)
        {
            if (evaluateFunc == null || setter == null) return;
            T value = evaluateFunc(startValue, endValue, easedTime);
            setter(value);
        }

        protected override void OnIncrementalLoop()
        {
            if (addFunc != null && subtractFunc != null)
            {
                T diff = subtractFunc(endValue, startValue);
                startValue = endValue;
                endValue = addFunc(endValue, diff);
            }
        }

        internal override void Reset()
        {
            base.Reset();
            getter = null;
            setter = null;
            startValue = default;
            endValue = default;
            changeValue = default;
            isFrom = false;
            startValueInitialized = false;
            evaluateFunc = null;
            addFunc = null;
            subtractFunc = null;
        }

        internal void Setup<TPlugin>(Func<T> getter, Action<T> setter, T endValue, float duration, TPlugin plugin)
            where TPlugin : struct, ITweenPlugin<T>
        {
            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;
            this.isActive = true;
            this.isPlaying = true;
            this.delayComplete = true;

            // プラグインのメソッドをデリゲートにキャプチャ
            evaluateFunc = plugin.Evaluate;
            addFunc = plugin.Add;
            subtractFunc = plugin.Subtract;
        }
    }
}
