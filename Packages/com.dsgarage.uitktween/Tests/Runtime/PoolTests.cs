using NUnit.Framework;
using UnityEngine;

namespace DSGarage.UITKTween.Tests
{
    [TestFixture]
    public class PoolTests
    {
        [SetUp]
        public void SetUp()
        {
            TweenPool.ClearAll();
            TweenManager.Clear();
        }

        [Test]
        public void Pool_Get_CreatesNewInstance()
        {
            var tweener = TweenPool.Get<Tweener<float>>();
            Assert.IsNotNull(tweener);
        }

        [Test]
        public void Pool_Return_IncreasesPoolCount()
        {
            var tweener = TweenPool.Get<Tweener<float>>();
            Assert.AreEqual(0, TweenPool.PooledCount<Tweener<float>>());

            TweenPool.Return(tweener);
            Assert.AreEqual(1, TweenPool.PooledCount<Tweener<float>>());
        }

        [Test]
        public void Pool_GetAfterReturn_ReusesInstance()
        {
            var tweener1 = TweenPool.Get<Tweener<float>>();
            TweenPool.Return(tweener1);

            var tweener2 = TweenPool.Get<Tweener<float>>();
            Assert.AreSame(tweener1, tweener2, "Should reuse pooled instance");
        }

        [Test]
        public void Pool_Warmup_PreAllocates()
        {
            TweenPool.Warmup<Tweener<float>>(10);
            Assert.AreEqual(10, TweenPool.PooledCount<Tweener<float>>());
        }

        [Test]
        public void Pool_ClearAll_EmptiesPools()
        {
            TweenPool.Warmup<Tweener<float>>(5);
            TweenPool.Warmup<Sequence>(3);

            TweenPool.ClearAll();

            Assert.AreEqual(0, TweenPool.PooledCount<Tweener<float>>());
            Assert.AreEqual(0, TweenPool.PooledCount<Sequence>());
        }

        [Test]
        public void Pool_ReturnResetsState()
        {
            var tweener = TweenPool.Get<Tweener<float>>();
            tweener.isActive = true;
            tweener.isPlaying = true;
            tweener.duration = 5f;

            TweenPool.Return(tweener);
            var reused = TweenPool.Get<Tweener<float>>();

            Assert.IsFalse(reused.isActive, "Should be reset");
            Assert.IsFalse(reused.isPlaying, "Should be reset");
            Assert.AreEqual(0f, reused.duration, "Should be reset");
        }

        [Test]
        public void SetCapacity_PreAllocatesMultipleTypes()
        {
            UITKTween.SetCapacity(5, 3);

            Assert.AreEqual(5, TweenPool.PooledCount<Tweener<float>>());
            Assert.AreEqual(5, TweenPool.PooledCount<Tweener<Vector2>>());
            Assert.AreEqual(5, TweenPool.PooledCount<Tweener<Vector3>>());
            Assert.AreEqual(5, TweenPool.PooledCount<Tweener<Color>>());
            Assert.AreEqual(3, TweenPool.PooledCount<Sequence>());
        }
    }
}
