using System;
using UnityEngine;

namespace DSGarage.UITKTween
{
    public abstract class Tween
    {
        // 状態
        internal bool isActive;
        internal bool isPlaying;
        internal bool isPaused;
        internal bool isComplete;
        internal bool isBackwards;
        internal bool autoKill = true;

        // パラメータ
        internal float duration;
        internal float delay;
        internal float elapsedTime;
        internal float elapsedDelay;
        internal bool delayComplete;
        internal EaseType easeType = EaseType.OutQuad;
        internal AnimationCurve customEaseCurve;
        internal int loops = 1;
        internal LoopType loopType = LoopType.Restart;
        internal bool isRelative;
        internal float timeScale = 1f;
        internal UpdateType updateType = UpdateType.Normal;
        internal object id;
        internal object target;

        // ループ追跡
        internal int completedLoops;
        internal bool hasStarted;

        // コールバック
        internal Action onStart;
        internal Action onPlay;
        internal Action<float> onUpdate;
        internal Action onComplete;
        internal Action onKill;
        internal Action onStepComplete;
        internal Action onPause;
        internal Action onRewind;

        // プロパティ
        public float Duration => duration;
        public float ElapsedTime => elapsedTime;
        public float ElapsedPercentage => duration > 0f ? Mathf.Clamp01(elapsedTime / duration) : 1f;
        public bool IsPlaying => isPlaying && !isPaused;
        public bool IsPaused => isPaused;
        public bool IsComplete => isComplete;
        public bool IsActive => isActive;
        public bool IsBackwards => isBackwards;
        public int Loops => loops;
        public int CompletedLoops => completedLoops;

        // === Fluent Settings ===

        public Tween SetEase(EaseType ease)
        {
            easeType = ease;
            customEaseCurve = null;
            return this;
        }

        public Tween SetEase(AnimationCurve curve)
        {
            easeType = EaseType.Custom;
            customEaseCurve = curve;
            return this;
        }

        public Tween SetLoops(int loops, LoopType loopType = LoopType.Restart)
        {
            this.loops = loops;
            this.loopType = loopType;
            return this;
        }

        public Tween SetDelay(float delay)
        {
            this.delay = Mathf.Max(0f, delay);
            return this;
        }

        public Tween SetRelative(bool isRelative = true)
        {
            this.isRelative = isRelative;
            return this;
        }

        public Tween SetAutoKill(bool autoKill = true)
        {
            this.autoKill = autoKill;
            return this;
        }

        public Tween SetId(object id)
        {
            this.id = id;
            return this;
        }

        public Tween SetTarget(object target)
        {
            this.target = target;
            return this;
        }

        public Tween SetUpdate(UpdateType updateType)
        {
            this.updateType = updateType;
            return this;
        }

        public Tween SetTimeScale(float timeScale)
        {
            this.timeScale = timeScale;
            return this;
        }

        // === Callbacks ===

        public Tween OnStart(Action callback) { onStart = callback; return this; }
        public Tween OnPlay(Action callback) { onPlay = callback; return this; }
        public Tween OnUpdate(Action<float> callback) { onUpdate = callback; return this; }
        public Tween OnComplete(Action callback) { onComplete = callback; return this; }
        public Tween OnKill(Action callback) { onKill = callback; return this; }
        public Tween OnStepComplete(Action callback) { onStepComplete = callback; return this; }
        public Tween OnPause(Action callback) { onPause = callback; return this; }
        public Tween OnRewind(Action callback) { onRewind = callback; return this; }

        // === Control ===

        public void Play()
        {
            if (!isActive) return;
            if (isPaused)
            {
                isPaused = false;
                onPlay?.Invoke();
            }
            isPlaying = true;
        }

        public void Pause()
        {
            if (!isActive || !isPlaying || isPaused) return;
            isPaused = true;
            onPause?.Invoke();
        }

        public void Kill(bool complete = false)
        {
            if (!isActive) return;
            if (complete) Complete();
            isActive = false;
            isPlaying = false;
            onKill?.Invoke();
            TweenManager.MarkForRemoval(this);
        }

        public void Restart(bool includeDelay = true)
        {
            if (!isActive) return;
            elapsedTime = 0f;
            completedLoops = 0;
            isComplete = false;
            isBackwards = false;
            hasStarted = false;
            if (includeDelay)
            {
                elapsedDelay = 0f;
                delayComplete = delay <= 0f;
            }
            isPlaying = true;
            isPaused = false;
        }

        public void Rewind(bool includeDelay = true)
        {
            if (!isActive) return;
            elapsedTime = 0f;
            completedLoops = 0;
            isComplete = false;
            isBackwards = false;
            hasStarted = false;
            if (includeDelay)
            {
                elapsedDelay = 0f;
                delayComplete = delay <= 0f;
            }
            isPlaying = false;
            ApplyValue(0f);
            onRewind?.Invoke();
        }

        public void PlayForward()
        {
            if (!isActive) return;
            isBackwards = false;
            Play();
        }

        public void PlayBackwards()
        {
            if (!isActive) return;
            isBackwards = true;
            Play();
        }

        public void Goto(float time, bool andPlay = false)
        {
            if (!isActive) return;
            elapsedTime = Mathf.Clamp(time, 0f, duration);
            float normalizedTime = duration > 0f ? elapsedTime / duration : 1f;
            ApplyValue(EaseEvaluator.Evaluate(easeType, normalizedTime, customEaseCurve));
            if (andPlay) Play();
        }

        public void Complete()
        {
            if (!isActive || isComplete) return;
            elapsedTime = duration;
            ApplyValue(isBackwards ? 0f : 1f);
            isComplete = true;
            isPlaying = false;
            onComplete?.Invoke();
        }

        // === Internal ===

        internal bool UpdateInternal(float deltaTime)
        {
            if (!isActive || !isPlaying || isPaused) return false;

            float scaledDelta = deltaTime * timeScale;

            // ディレイ処理
            if (!delayComplete)
            {
                elapsedDelay += scaledDelta;
                if (elapsedDelay < delay) return false;
                delayComplete = true;
                scaledDelta = elapsedDelay - delay;
            }

            // 初回起動コールバック
            if (!hasStarted)
            {
                hasStarted = true;
                Startup();
                onStart?.Invoke();
            }

            // 時間更新
            if (isBackwards)
                elapsedTime -= scaledDelta;
            else
                elapsedTime += scaledDelta;

            // ループ処理
            if (elapsedTime >= duration)
            {
                completedLoops++;
                onStepComplete?.Invoke();

                if (loops > 0 && completedLoops >= loops)
                {
                    // 完了
                    elapsedTime = duration;
                    float easedT = EaseEvaluator.Evaluate(easeType, 1f, customEaseCurve);
                    ApplyValue(easedT);
                    onUpdate?.Invoke(1f);
                    isComplete = true;
                    isPlaying = false;
                    onComplete?.Invoke();
                    if (autoKill) Kill();
                    return true;
                }

                // ループ継続
                switch (loopType)
                {
                    case LoopType.Restart:
                        elapsedTime -= duration;
                        break;
                    case LoopType.Yoyo:
                        isBackwards = !isBackwards;
                        elapsedTime = duration - (elapsedTime - duration);
                        break;
                    case LoopType.Incremental:
                        elapsedTime -= duration;
                        OnIncrementalLoop();
                        break;
                }
            }
            else if (elapsedTime < 0f)
            {
                // 逆再生でのループ
                completedLoops++;
                onStepComplete?.Invoke();

                if (loops > 0 && completedLoops >= loops)
                {
                    elapsedTime = 0f;
                    ApplyValue(0f);
                    onUpdate?.Invoke(0f);
                    isComplete = true;
                    isPlaying = false;
                    onComplete?.Invoke();
                    if (autoKill) Kill();
                    return true;
                }

                switch (loopType)
                {
                    case LoopType.Restart:
                        elapsedTime += duration;
                        break;
                    case LoopType.Yoyo:
                        isBackwards = !isBackwards;
                        elapsedTime = -elapsedTime;
                        break;
                    case LoopType.Incremental:
                        elapsedTime += duration;
                        break;
                }
            }

            // 値適用
            float normalizedTime = duration > 0f ? Mathf.Clamp01(elapsedTime / duration) : 1f;
            float easedValue = EaseEvaluator.Evaluate(easeType, normalizedTime, customEaseCurve);
            ApplyValue(easedValue);
            onUpdate?.Invoke(normalizedTime);

            return false;
        }

        internal virtual void Startup() { }
        protected abstract void ApplyValue(float easedTime);
        protected virtual void OnIncrementalLoop() { }

        internal virtual void Reset()
        {
            isActive = false;
            isPlaying = false;
            isPaused = false;
            isComplete = false;
            isBackwards = false;
            autoKill = true;
            duration = 0f;
            delay = 0f;
            elapsedTime = 0f;
            elapsedDelay = 0f;
            delayComplete = false;
            easeType = EaseType.OutQuad;
            customEaseCurve = null;
            loops = 1;
            loopType = LoopType.Restart;
            isRelative = false;
            timeScale = 1f;
            updateType = UpdateType.Normal;
            id = null;
            target = null;
            completedLoops = 0;
            hasStarted = false;
            onStart = null;
            onPlay = null;
            onUpdate = null;
            onComplete = null;
            onKill = null;
            onStepComplete = null;
            onPause = null;
            onRewind = null;
        }
    }
}
