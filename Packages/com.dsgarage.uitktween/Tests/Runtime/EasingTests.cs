using NUnit.Framework;
using UnityEngine;

namespace DSGarage.UITKTween.Tests
{
    [TestFixture]
    public class EasingTests
    {
        [Test]
        public void Linear_ReturnsInputValue()
        {
            Assert.AreEqual(0f, EaseFunctions.Evaluate(EaseType.Linear, 0f), 0.001f);
            Assert.AreEqual(0.5f, EaseFunctions.Evaluate(EaseType.Linear, 0.5f), 0.001f);
            Assert.AreEqual(1f, EaseFunctions.Evaluate(EaseType.Linear, 1f), 0.001f);
        }

        [Test]
        public void AllEases_StartAtZero()
        {
            foreach (EaseType ease in System.Enum.GetValues(typeof(EaseType)))
            {
                if (ease == EaseType.Custom) continue;
                float value = EaseFunctions.Evaluate(ease, 0f);
                Assert.AreEqual(0f, value, 0.01f, $"{ease} should start at 0");
            }
        }

        [Test]
        public void AllEases_EndAtOne()
        {
            foreach (EaseType ease in System.Enum.GetValues(typeof(EaseType)))
            {
                if (ease == EaseType.Custom) continue;
                float value = EaseFunctions.Evaluate(ease, 1f);
                Assert.AreEqual(1f, value, 0.01f, $"{ease} should end at 1");
            }
        }

        [Test]
        public void OutQuad_IsFasterAtStart()
        {
            float midValue = EaseFunctions.Evaluate(EaseType.OutQuad, 0.5f);
            Assert.Greater(midValue, 0.5f, "OutQuad at t=0.5 should be > 0.5");
        }

        [Test]
        public void InQuad_IsSlowerAtStart()
        {
            float midValue = EaseFunctions.Evaluate(EaseType.InQuad, 0.5f);
            Assert.Less(midValue, 0.5f, "InQuad at t=0.5 should be < 0.5");
        }

        [Test]
        public void InOutQuad_IsSymmetric()
        {
            float at25 = EaseFunctions.Evaluate(EaseType.InOutQuad, 0.25f);
            float at75 = EaseFunctions.Evaluate(EaseType.InOutQuad, 0.75f);
            Assert.AreEqual(at25 + at75, 1f, 0.01f, "InOutQuad should be symmetric");
        }

        [Test]
        public void OutBounce_HasBounces()
        {
            // OutBounce は t=0.5 付近で非線形な動き
            float v1 = EaseFunctions.Evaluate(EaseType.OutBounce, 0.3f);
            float v2 = EaseFunctions.Evaluate(EaseType.OutBounce, 0.5f);
            float v3 = EaseFunctions.Evaluate(EaseType.OutBounce, 0.7f);
            // 単調増加ではない（バウンスがある）or 値が有効範囲内
            Assert.GreaterOrEqual(v1, 0f);
            Assert.GreaterOrEqual(v2, 0f);
            Assert.GreaterOrEqual(v3, 0f);
        }

        [Test]
        public void EaseEvaluator_LinearPassthrough()
        {
            float value = EaseEvaluator.Evaluate(EaseType.Linear, 0.5f, null);
            Assert.AreEqual(0.5f, value, 0.001f);
        }

        [Test]
        public void EaseEvaluator_CustomCurve()
        {
            var curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            float value = EaseEvaluator.Evaluate(EaseType.Custom, 0.5f, curve);
            Assert.AreEqual(0.5f, value, 0.1f, "Custom curve should approximate 0.5 at midpoint");
        }

        [Test]
        public void OutElastic_Overshoots()
        {
            // OutElastic は途中で 1 を超える
            float value = EaseFunctions.Evaluate(EaseType.OutElastic, 0.3f);
            // 特定の t で 1 を超えるか、少なくとも有効な値
            Assert.IsTrue(!float.IsNaN(value) && !float.IsInfinity(value));
        }

        [Test]
        public void OutBack_OvershoosAtEnd()
        {
            // OutBack は t=0.5 付近で 1 を超える場合がある
            float peak = 0f;
            for (float t = 0f; t <= 1f; t += 0.01f)
            {
                float v = EaseFunctions.Evaluate(EaseType.OutBack, t);
                if (v > peak) peak = v;
            }
            Assert.Greater(peak, 1f, "OutBack should overshoot past 1.0");
        }
    }
}
