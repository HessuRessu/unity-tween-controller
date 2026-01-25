using NUnit.Framework;
using UnityEngine;
using Pihkura.Tween.Library;
using Pihkura.Tween.Data;

namespace Pihkura.Tween.Tests
{
    /// <summary>
    /// Unit tests for <see cref="CanvasGroupAlphaTweenBinding"/>.
    /// </summary>
    public class CanvasGroupAlphaTweenBindingTests
    {
        /// <summary>
        /// Verifies that alpha starts at BaseValue when bound.
        /// </summary>
        [Test]
        public void Alpha_Starts_At_BaseValue()
        {
            // Arrange
            var go = new GameObject("CanvasGroup");
            var canvasGroup = go.AddComponent<CanvasGroup>();

            var tween = new CanvasGroupAlphaTweenBinding();

            // Act
            tween.Bind(
                canvasGroup,
                1f,
                new TweenContext
                {
                    BaseValue = 0f,
                    TargetValue = 1f
                });

            // Assert
            Assert.AreEqual(0f, canvasGroup.alpha, 0.0001f);

            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that alpha interpolates over time.
        /// </summary>
        [Test]
        public void Alpha_Interpolates_Towards_Target()
        {
            // Arrange
            var go = new GameObject("CanvasGroup");
            var canvasGroup = go.AddComponent<CanvasGroup>();

            var tween = new CanvasGroupAlphaTweenBinding();

            tween.Bind(
                canvasGroup,
                1f,
                new TweenContext
                {
                    BaseValue = 0f,
                    TargetValue = 1f
                });

            // Act
            tween.Update(0.5f);

            // Assert
            Assert.Greater(canvasGroup.alpha, 0f);
            Assert.Less(canvasGroup.alpha, 1f);

            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that cancel applies the target value.
        /// </summary>
        [Test]
        public void Cancel_Applies_TargetValue()
        {
            // Arrange
            var go = new GameObject("CanvasGroup");
            var canvasGroup = go.AddComponent<CanvasGroup>();

            var tween = new CanvasGroupAlphaTweenBinding();

            tween.Bind(
                canvasGroup,
                1f,
                new TweenContext
                {
                    BaseValue = 0f,
                    TargetValue = 1f
                });

            // Act
            tween.Cancel();

            // Assert
            Assert.AreEqual(1f, canvasGroup.alpha, 0.0001f);

            Object.DestroyImmediate(go);
        }
    }
}
