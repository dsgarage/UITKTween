using System;
using System.Collections.Generic;
using UnityEngine;

namespace DSGarage.UITKTween
{
    /// <summary>
    /// 複数の Tween をタイムラインで管理するシーケンス。
    /// DOTween の Sequence と同等の API を提供する。
    /// </summary>
    public class Sequence : Tween
    {
        private readonly List<SequenceItem> items = new(8);
        private float totalDuration;
        private float lastPosition;
        private bool isSetup;

        // === ビルドメソッド ===

        /// <summary>シーケンス末尾に Tween を追加</summary>
        public Sequence Append(Tween tween)
        {
            if (tween == null) return this;
            ValidateNotStarted();
            RemoveFromManager(tween);

            float insertTime = totalDuration;
            float tweenDuration = tween.duration + tween.delay;
            items.Add(new SequenceItem(tween, insertTime, SequenceItemType.Tween));
            totalDuration = insertTime + tweenDuration;
            UpdateDuration();
            return this;
        }

        /// <summary>シーケンス先頭に Tween を追加（他の全アイテムを後方にシフト）</summary>
        public Sequence Prepend(Tween tween)
        {
            if (tween == null) return this;
            ValidateNotStarted();
            RemoveFromManager(tween);

            float tweenDuration = tween.duration + tween.delay;
            // 既存アイテムを後方にシフト
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.startTime += tweenDuration;
                items[i] = item;
            }
            items.Add(new SequenceItem(tween, 0f, SequenceItemType.Tween));
            totalDuration += tweenDuration;
            UpdateDuration();
            return this;
        }

        /// <summary>直前に追加した Tween と同時開始で並行実行</summary>
        public Sequence Join(Tween tween)
        {
            if (tween == null) return this;
            ValidateNotStarted();
            RemoveFromManager(tween);

            // 直前の Tween の開始時間を取得
            float insertTime = GetLastTweenInsertTime();
            float tweenDuration = tween.duration + tween.delay;
            items.Add(new SequenceItem(tween, insertTime, SequenceItemType.Tween));
            float endTime = insertTime + tweenDuration;
            if (endTime > totalDuration) totalDuration = endTime;
            UpdateDuration();
            return this;
        }

        /// <summary>指定時間にTweenを挿入</summary>
        public Sequence Insert(float atPosition, Tween tween)
        {
            if (tween == null) return this;
            ValidateNotStarted();
            RemoveFromManager(tween);

            float tweenDuration = tween.duration + tween.delay;
            items.Add(new SequenceItem(tween, atPosition, SequenceItemType.Tween));
            float endTime = atPosition + tweenDuration;
            if (endTime > totalDuration) totalDuration = endTime;
            UpdateDuration();
            return this;
        }

        /// <summary>シーケンス末尾にインターバル（待機時間）を追加</summary>
        public Sequence AppendInterval(float interval)
        {
            ValidateNotStarted();
            totalDuration += interval;
            UpdateDuration();
            return this;
        }

        /// <summary>シーケンス先頭にインターバルを追加（他の全アイテムを後方にシフト）</summary>
        public Sequence PrependInterval(float interval)
        {
            ValidateNotStarted();
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.startTime += interval;
                items[i] = item;
            }
            totalDuration += interval;
            UpdateDuration();
            return this;
        }

        /// <summary>シーケンス末尾にコールバックを追加</summary>
        public Sequence AppendCallback(Action callback)
        {
            if (callback == null) return this;
            ValidateNotStarted();
            items.Add(new SequenceItem(callback, totalDuration));
            return this;
        }

        /// <summary>シーケンス先頭にコールバックを追加</summary>
        public Sequence PrependCallback(Action callback)
        {
            if (callback == null) return this;
            ValidateNotStarted();
            // 既存アイテムはシフトしない（コールバックは0秒なのでシフト不要）
            items.Add(new SequenceItem(callback, 0f));
            return this;
        }

        /// <summary>指定時間にコールバックを挿入</summary>
        public Sequence InsertCallback(float atPosition, Action callback)
        {
            if (callback == null) return this;
            ValidateNotStarted();
            items.Add(new SequenceItem(callback, atPosition));
            return this;
        }

        // === Fluent overrides ===

        public new Sequence SetEase(EaseType ease) { base.SetEase(ease); return this; }
        public new Sequence SetEase(AnimationCurve curve) { base.SetEase(curve); return this; }
        public new Sequence SetLoops(int loops, LoopType loopType = LoopType.Restart) { base.SetLoops(loops, loopType); return this; }
        public new Sequence SetDelay(float delay) { base.SetDelay(delay); return this; }
        public new Sequence SetAutoKill(bool autoKill = true) { base.SetAutoKill(autoKill); return this; }
        public new Sequence SetId(object id) { base.SetId(id); return this; }
        public new Sequence SetTarget(object target) { base.SetTarget(target); return this; }
        public new Sequence SetUpdate(UpdateType updateType) { base.SetUpdate(updateType); return this; }
        public new Sequence SetTimeScale(float timeScale) { base.SetTimeScale(timeScale); return this; }
        public new Sequence OnStart(Action callback) { base.OnStart(callback); return this; }
        public new Sequence OnPlay(Action callback) { base.OnPlay(callback); return this; }
        public new Sequence OnUpdate(Action<float> callback) { base.OnUpdate(callback); return this; }
        public new Sequence OnComplete(Action callback) { base.OnComplete(callback); return this; }
        public new Sequence OnKill(Action callback) { base.OnKill(callback); return this; }
        public new Sequence OnStepComplete(Action callback) { base.OnStepComplete(callback); return this; }

        // === Internal ===

        internal override void Startup()
        {
            if (isSetup) return;
            isSetup = true;

            // 子 Tween の Startup を呼ぶ
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].type == SequenceItemType.Tween && items[i].tween != null)
                {
                    items[i].tween.hasStarted = true;
                    items[i].tween.Startup();
                }
            }
        }

        protected override void ApplyValue(float easedTime)
        {
            // Sequence は totalDuration に基づいてシーケンス全体の位置を計算
            float position = easedTime * totalDuration;
            ApplyAtPosition(position);
        }

        private void ApplyAtPosition(float position)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.type == SequenceItemType.Callback)
                {
                    // コールバック: 位置を通過した時に呼び出す
                    if (lastPosition < item.startTime && position >= item.startTime)
                        item.callback?.Invoke();
                    else if (lastPosition > item.startTime && position <= item.startTime)
                        item.callback?.Invoke(); // 逆再生時
                    continue;
                }

                if (item.tween == null || !item.tween.isActive) continue;

                float tweenDuration = item.tween.duration;
                if (tweenDuration <= 0f) continue;

                float tweenStart = item.startTime;
                float tweenEnd = tweenStart + item.tween.duration + item.tween.delay;

                if (position >= tweenStart && position <= tweenEnd)
                {
                    // この Tween の範囲内
                    float localTime = position - tweenStart - item.tween.delay;
                    localTime = Mathf.Clamp(localTime, 0f, item.tween.duration);
                    float normalizedTime = item.tween.duration > 0f ? localTime / item.tween.duration : 1f;
                    float easedValue = EaseEvaluator.Evaluate(item.tween.easeType, normalizedTime, item.tween.customEaseCurve);
                    item.tween.ApplyValue(easedValue);
                }
                else if (position >= tweenEnd)
                {
                    // Tween 完了位置
                    float easedValue = EaseEvaluator.Evaluate(item.tween.easeType, 1f, item.tween.customEaseCurve);
                    item.tween.ApplyValue(easedValue);
                }
                else if (position <= tweenStart)
                {
                    // Tween 開始前
                    float easedValue = EaseEvaluator.Evaluate(item.tween.easeType, 0f, item.tween.customEaseCurve);
                    item.tween.ApplyValue(easedValue);
                }
            }

            lastPosition = position;
        }

        internal override void Reset()
        {
            base.Reset();
            items.Clear();
            totalDuration = 0f;
            lastPosition = 0f;
            isSetup = false;
        }

        // === Private ヘルパー ===

        private void UpdateDuration()
        {
            duration = totalDuration;
        }

        private float GetLastTweenInsertTime()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].type == SequenceItemType.Tween)
                    return items[i].startTime;
            }
            return 0f;
        }

        private void ValidateNotStarted()
        {
            if (hasStarted)
                UnityEngine.Debug.LogWarning("[UITKTween] Sequence は開始後に変更できません。");
        }

        private static void RemoveFromManager(Tween tween)
        {
            // Sequence に追加された Tween は TweenManager から外す（Sequence が管理する）
            tween.isPlaying = false;
            TweenManager.MarkForRemoval(tween);
        }

        // === 内部構造体 ===

        private struct SequenceItem
        {
            internal Tween tween;
            internal Action callback;
            internal float startTime;
            internal SequenceItemType type;

            internal SequenceItem(Tween tween, float startTime, SequenceItemType type)
            {
                this.tween = tween;
                this.callback = null;
                this.startTime = startTime;
                this.type = type;
            }

            internal SequenceItem(Action callback, float startTime)
            {
                this.tween = null;
                this.callback = callback;
                this.startTime = startTime;
                this.type = SequenceItemType.Callback;
            }
        }

        private enum SequenceItemType
        {
            Tween,
            Callback
        }
    }
}
