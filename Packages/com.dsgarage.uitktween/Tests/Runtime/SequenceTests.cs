using NUnit.Framework;

namespace DSGarage.UITKTween.Tests
{
    [TestFixture]
    public class SequenceTests
    {
        [SetUp]
        public void SetUp()
        {
            TweenManager.Clear();
        }

        [Test]
        public void Sequence_Append_RunsSequentially()
        {
            float val1 = 0f, val2 = 0f;

            UITKTween.Sequence()
                .Append(UITKTween.To(() => val1, x => val1 = x, 10f, 0.5f).SetEase(EaseType.Linear))
                .Append(UITKTween.To(() => val2, x => val2 = x, 20f, 0.5f).SetEase(EaseType.Linear))
                .SetAutoKill(false);

            // 0.5秒: 最初の Tween 完了
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(10f, val1, 1f, "First tween should be at end");
            Assert.AreEqual(0f, val2, 1f, "Second tween should not have started yet");

            // 1.0秒: 2番目の Tween 完了
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(20f, val2, 1f, "Second tween should be at end");
        }

        [Test]
        public void Sequence_Join_RunsParallel()
        {
            float val1 = 0f, val2 = 0f;

            UITKTween.Sequence()
                .Append(UITKTween.To(() => val1, x => val1 = x, 10f, 1f).SetEase(EaseType.Linear))
                .Join(UITKTween.To(() => val2, x => val2 = x, 20f, 1f).SetEase(EaseType.Linear))
                .SetAutoKill(false);

            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.AreEqual(5f, val1, 1f, "First tween midpoint");
            Assert.AreEqual(10f, val2, 1f, "Second tween midpoint (parallel)");
        }

        [Test]
        public void Sequence_AppendInterval_AddsGap()
        {
            float value = 0f;

            UITKTween.Sequence()
                .AppendInterval(0.5f)
                .Append(UITKTween.To(() => value, x => value = x, 10f, 0.5f).SetEase(EaseType.Linear))
                .SetAutoKill(false);

            // 0.3秒: まだインターバル中
            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.AreEqual(0f, value, 0.1f, "Should be 0 during interval");

            // 0.8秒: Tween が 0.3秒進んでいるはず
            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.Greater(value, 0f, "Should have started after interval");
        }

        [Test]
        public void Sequence_AppendCallback_FiresAtCorrectTime()
        {
            bool callbackFired = false;
            float value = 0f;

            UITKTween.Sequence()
                .Append(UITKTween.To(() => value, x => value = x, 10f, 0.5f))
                .AppendCallback(() => callbackFired = true)
                .SetAutoKill(false);

            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.IsFalse(callbackFired, "Callback should not fire before tween completes");

            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.IsTrue(callbackFired, "Callback should fire after tween completes");
        }

        [Test]
        public void Sequence_OnComplete_Fires()
        {
            bool completed = false;

            UITKTween.Sequence()
                .Append(UITKTween.To(() => 0f, x => { }, 1f, 0.5f))
                .OnComplete(() => completed = true);

            TweenManager.Update(0.6f, UpdateType.Normal);
            Assert.IsTrue(completed, "Sequence OnComplete should fire");
        }

        [Test]
        public void Sequence_SetDelay_WorksCorrectly()
        {
            float value = 0f;

            UITKTween.Sequence()
                .Append(UITKTween.To(() => value, x => value = x, 10f, 0.5f).SetEase(EaseType.Linear))
                .SetDelay(0.3f)
                .SetAutoKill(false);

            TweenManager.Update(0.2f, UpdateType.Normal);
            Assert.AreEqual(0f, value, 0.1f, "Should be delayed");

            TweenManager.Update(0.5f, UpdateType.Normal);
            Assert.Greater(value, 0f, "Should start after delay");
        }

        [Test]
        public void Sequence_Insert_AtSpecificTime()
        {
            float val1 = 0f, val2 = 0f;

            UITKTween.Sequence()
                .Append(UITKTween.To(() => val1, x => val1 = x, 10f, 1f).SetEase(EaseType.Linear))
                .Insert(0.5f, UITKTween.To(() => val2, x => val2 = x, 20f, 0.5f).SetEase(EaseType.Linear))
                .SetAutoKill(false);

            // 0.3秒: val2 はまだ開始前
            TweenManager.Update(0.3f, UpdateType.Normal);
            Assert.AreEqual(0f, val2, 0.1f, "Inserted tween should not start before 0.5s");

            // 0.7秒: val2 は 0.2秒進んでいるはず
            TweenManager.Update(0.4f, UpdateType.Normal);
            Assert.Greater(val2, 0f, "Inserted tween should have started");
        }

        [Test]
        public void EmptySequence_CompletesImmediately()
        {
            bool completed = false;

            UITKTween.Sequence()
                .OnComplete(() => completed = true);

            TweenManager.Update(0.01f, UpdateType.Normal);
            Assert.IsTrue(completed, "Empty sequence should complete immediately");
        }
    }
}
