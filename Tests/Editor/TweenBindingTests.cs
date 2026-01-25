using NUnit.Framework;
using Pihkura.Tween.Core;
using Pihkura.Tween.Library;
using Pihkura.Tween.Data;

namespace Pihkura.Tween.Tests
{
    /// <summary>
    /// Generic tests that validate common <see cref="TweenBindingBase"/> behavior
    /// independent of concrete binding implementation.
    /// </summary>
    public class TweenBindingTests
    {
        /// <summary>
        /// Verifies that Reset clears common runtime state.
        /// </summary>
        [Test]
        public void Reset_Clears_State()
        {
            // Arrange
            var tween = new DelayedTriggerTweenBinding();
            tween.Bind(null, 1f, new TweenContext());
            tween.onComplete = () => { };

            // Act
            tween.Reset();

            // Assert
            Assert.AreEqual(0f, tween.Timer);
            Assert.IsNull(tween.Target);
            Assert.IsNull(tween.onComplete);
        }

        /// <summary>
        /// Verifies that Complete invokes onComplete when requested.
        /// </summary>
        [Test]
        public void Complete_Invokes_OnComplete_When_Enabled()
        {
            // Arrange
            var tween = new DelayedTriggerTweenBinding();
            bool invoked = false;

            tween.onComplete = () => invoked = true;

            // Act
            tween.Complete(true);

            // Assert
            Assert.IsTrue(invoked);
        }

        /// <summary>
        /// Verifies that Complete does not invoke onComplete when disabled.
        /// </summary>
        [Test]
        public void Complete_Does_Not_Invoke_OnComplete_When_Disabled()
        {
            // Arrange
            var tween = new DelayedTriggerTweenBinding();
            bool invoked = false;

            tween.onComplete = () => invoked = true;

            // Act
            tween.Complete(false);

            // Assert
            Assert.IsFalse(invoked);
        }

        /// <summary>
        /// Verifies that Update eventually reports completion after Duration.
        /// </summary>
        [Test]
        public void Update_Returns_True_After_Duration()
        {
            // Arrange
            var tween = new DelayedTriggerTweenBinding();
            tween.Bind(null, 1f, new TweenContext());

            // Act
            bool finishedFirst = tween.Update(0.5f);
            bool finishedSecond = tween.Update(0.5f);

            // Assert
            Assert.IsFalse(finishedFirst);
            Assert.IsTrue(finishedSecond);
        }
    }
}
