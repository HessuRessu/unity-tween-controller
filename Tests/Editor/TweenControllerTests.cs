using NUnit.Framework;
using UnityEngine;
using Pihkura.Tween;
using Pihkura.Tween.Library;
using Pihkura.Tween.Data;
using System.Reflection;

namespace Pihkura.Tween.Tests
{
    /// <summary>
    /// Unit tests for <see cref="TweenController"/>.
    /// 
    /// These tests focus on verifying pooling behavior, basic lifecycle,
    /// and deterministic completion using manual time stepping.
    /// </summary>
    public class TweenControllerTests
    {
        /// <summary>
        /// Manually invokes Awake() on the controller.
        /// 
        /// In EditMode tests Unity does not always call MonoBehaviour lifecycle
        /// methods automatically, so we explicitly trigger initialization.
        /// </summary>
        private static void Initialize(TweenController controller)
        {
            typeof(TweenController)
                .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(controller, null);
        }

        /// <summary>
        /// Verifies that requesting a tween returns a valid binding instance.
        /// </summary>
        [Test]
        public void Get_Returns_Binding_Instance()
        {
            // Arrange
            var go = new GameObject("TweenController");
            var controller = go.AddComponent<TweenController>();
            Initialize(controller);

            // Act
            var tween = controller.Get<DelayedTriggerTweenBinding>();

            // Assert
            Assert.IsNotNull(tween);

            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that a delayed trigger tween completes after its duration.
        /// </summary>
        [Test]
        public void DelayedTween_Completes_After_Duration()
        {
            // Arrange
            var go = new GameObject("TweenController");
            var controller = go.AddComponent<TweenController>();
            Initialize(controller);

            var targetGO = new GameObject("Target");
            var target = targetGO.AddComponent<CanvasGroup>();

            var tween = controller.Get<DelayedTriggerTweenBinding>();
            tween.Bind(target, 1f, new TweenContext());

            // Act
            bool finishedFirst = tween.Update(0.5f);
            bool finishedSecond = tween.Update(0.5f);

            // Assert
            Assert.IsFalse(finishedFirst);
            Assert.IsTrue(finishedSecond);

            Object.DestroyImmediate(targetGO);
            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that completed tweens are recycled from the pool.
        /// </summary>
        [Test]
        public void Completed_Tween_Is_Reused_From_Pool()
        {
            // Arrange
            var go = new GameObject("TweenController");
            var controller = go.AddComponent<TweenController>();
            Initialize(controller);

            // Use a real target so Cancel(target) only affects this tween
            var targetGO = new GameObject("Target");
            var target = targetGO.AddComponent<CanvasGroup>();

            var tweenA = controller.Get<DelayedTriggerTweenBinding>();
            tweenA.Bind(target, 0.1f, new TweenContext());

            // Simulate completion
            tweenA.Update(0.1f);
            controller.Cancel(tweenA.UntypedTarget);

            // Act
            var tweenB = controller.Get<DelayedTriggerTweenBinding>();

            // Assert
            Assert.AreSame(tweenA, tweenB);

            Object.DestroyImmediate(targetGO);
            Object.DestroyImmediate(go);
        }
    }
}
