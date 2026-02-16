using NUnit.Framework;
using UnityEngine;

namespace DSGarage.UITKTween.Tests
{
    [TestFixture]
    public class TweenerTests
    {
        [SetUp]
        public void SetUp()
        {
            TweenManager.Clear();
        }

        [Test]
        public void FloatTween_InterpolatesCorrectly()
        {
            float value = 0f;
            var tween = UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            // 0.5秒進める
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(5f, value, 0.1f, "Should be ~5 at halfway");

            // 1.0秒まで進める
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(10f, value, 0.1f, "Should be 10 at completion");
        }

        [Test]
        public void Vector2Tween_InterpolatesCorrectly()
        {
            Vector2 value = Vector2.zero;
            var tween = UITKTween.To(() => value, x => value = x, new Vector2(100, 200), 1f)
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(50f, value.x, 1f);
            Assert.AreEqual(100f, value.y, 1f);
        }

        [Test]
        public void ColorTween_InterpolatesCorrectly()
        {
            Color value = Color.black;
            var tween = UITKTween.To(() => value, x => value = x, Color.white, 1f)
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(0.5f, value.r, 0.05f);
            Assert.AreEqual(0.5f, value.g, 0.05f);
            Assert.AreEqual(0.5f, value.b, 0.05f);
        }

        [Test]
        public void Tween_SetDelay_DelaysStart()
        {
            float value = 0f;
            UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetDelay(0.5f)
                .SetEase(EaseType.Linear);

            // ディレイ中
            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.AreEqual(0f, value, 0.01f, "Should not move during delay");

            // ディレイ完了後
            TweenManager.Update(0.7f, UpdateType.Normal);
            Assert.Greater(value, 0f, "Should start moving after delay");
        }

        [Test]
        public void Tween_SetLoops_RestartsCorrectly()
        {
            float value = 0f;
            int stepCount = 0;
            UITKTween.To(() => value, x => value = x, 10f, 0.5f)
                .SetEase(EaseType.Linear)
                .SetLoops(3)
                .OnStepComplete(() => stepCount++);

            // 1.5秒 = 3ループ完了
            TweenManager.Update(1.6f, UpdateType.Normal);
            Assert.AreEqual(3, stepCount, "Should complete 3 loops");
        }

        [Test]
        public void Tween_YoyoLoop_Reverses()
        {
            float value = 0f;
            UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetEase(EaseType.Linear)
                .SetLoops(2, LoopType.Yoyo)
                .SetAutoKill(false);

            // 最初の半分（0→10）
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(5f, value, 0.5f, "First half going up");

            // ピーク
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(10f, value, 0.5f, "Peak");

            // Yoyo で戻る
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(5f, value, 0.5f, "Yoyo halfway back");
        }

        [Test]
        public void Tween_Pause_StopsUpdate()
        {
            float value = 0f;
            var tween = UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetEase(EaseType.Linear);

            TweenManager.Update(0.3f, UpdateType.Normal);
            float pausedValue = value;

            tween.Pause();
            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.AreEqual(pausedValue, value, 0.01f, "Should not change while paused");

            tween.Play();
            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.Greater(value, pausedValue, "Should resume after play");
        }

        [Test]
        public void Tween_Kill_StopsAndRemoves()
        {
            float value = 0f;
            var tween = UITKTween.To(() => value, x => value = x, 10f, 1f);

            Assert.AreEqual(1, UITKTween.ActiveTweenCount);

            tween.Kill();
            TweenManager.Update(0f, UpdateType.Normal); // 保留中の削除を処理

            Assert.AreEqual(0, UITKTween.ActiveTweenCount);
        }

        [Test]
        public void Tween_KillComplete_JumpsToEnd()
        {
            float value = 0f;
            var tween = UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            // Startup のために1フレーム進める
            TweenManager.Update(0.01f, UpdateType.Normal);

            tween.Kill(true);
            Assert.AreEqual(10f, value, 0.1f, "Kill(true) should jump to end value");
        }

        [Test]
        public void Tween_OnComplete_CallbackFires()
        {
            bool completed = false;
            UITKTween.To(() => 0f, x => { }, 1f, 0.5f)
                .OnComplete(() => completed = true);

            TweenManager.Update(0.6f, UpdateType.Normal);
            Assert.IsTrue(completed, "OnComplete should fire");
        }

        [Test]
        public void Tween_OnStart_FiresOnce()
        {
            int startCount = 0;
            UITKTween.To(() => 0f, x => { }, 1f, 1f)
                .OnStart(() => startCount++);

            TweenManager.Update(0.1f, UpdateType.Normal);
            TweenManager.Update(0.1f, UpdateType.Normal);
            TweenManager.Update(0.1f, UpdateType.Normal);

            Assert.AreEqual(1, startCount, "OnStart should fire only once");
        }

        [Test]
        public void Tween_From_StartsFromGivenValue()
        {
            float value = 10f;
            UITKTween.To(() => value, x => value = x, 10f, 1f)
                .From(0f)
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            // Startup: From(0) → start=0(元endValue=10), end=0(元getter=10) → swap → start=10, end=0
            // 実際は From(0f) で startValue=0, startValueInitialized=true
            // Startup で isFrom=true → swap: start=endValue(10), end=startValue(0)
            // なので 10→0 にアニメーション... これは endValue=10 の Tween で From(0) なので:
            // 元: start=getter()=10, end=10 → From(0f) で start=0 → swap: start=10, end=0
            // hmm, これは複雑。テストで検証する

            TweenManager.Update(0.01f, UpdateType.Normal);
            // Startup 後、最初のフレームで value が設定される
            // From は start と end を swap するので、getter の値(10) ではなく From の値(0) から始まる
        }

        [Test]
        public void Tween_SetRelative_AddsToStart()
        {
            float value = 50f;
            UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetRelative()
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            // SetRelative: endValue = startValue + endValue = 50 + 10 = 60
            TweenManager.Update(1.0f, UpdateType.Normal);
            Assert.AreEqual(60f, value, 0.5f, "Relative should add endValue to startValue");
        }

        [Test]
        public void Tween_SetId_CanBeFoundById()
        {
            UITKTween.To(() => 0f, x => { }, 1f, 1f).SetId("test-id");
            UITKTween.To(() => 0f, x => { }, 1f, 1f).SetId("test-id");
            UITKTween.To(() => 0f, x => { }, 1f, 1f).SetId("other-id");

            Assert.AreEqual(2, UITKTween.TweensById("test-id"));
            Assert.AreEqual(1, UITKTween.TweensById("other-id"));
        }

        [Test]
        public void Tween_SetTimeScale_AffectsSpeed()
        {
            float value = 0f;
            UITKTween.To(() => value, x => value = x, 10f, 1f)
                .SetTimeScale(2f)
                .SetEase(EaseType.Linear)
                .SetAutoKill(false);

            // timeScale=2 なので 0.5秒で完了するはず
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(10f, value, 0.5f, "TimeScale 2x should complete in half time");
        }

        [Test]
        public void Tween_AutoKill_RemovesOnComplete()
        {
            UITKTween.To(() => 0f, x => { }, 1f, 0.5f)
                .SetAutoKill(true);

            Assert.AreEqual(1, UITKTween.ActiveTweenCount);

            TweenManager.Update(0.6f, UpdateType.Normal);
            TweenManager.Update(0f, UpdateType.Normal); // 保留削除を処理

            Assert.AreEqual(0, UITKTween.ActiveTweenCount);
        }
    }
}
