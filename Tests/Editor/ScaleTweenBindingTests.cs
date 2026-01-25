using NUnit.Framework;
using UnityEngine;
using Pihkura.Tween.Library;
using Pihkura.Tween.Data;

namespace Pihkura.Tween.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ScaleTweenBinding"/>.
    /// </summary>
    public class ScaleTweenBindingTests
    {
        /// <summary>
        /// Verifies that scale interpolates towards the target scale.
        /// </summary>
        [Test]
        public void Scale_Interpolates_Towards_Target()
        {
            // Arrange
            var go = new GameObject("ScaledObject");
            go.transform.localScale = Vector3.one;

            var tween = new ScaleTweenBinding();

            tween.Bind(
                go,
                1f,
                new TweenContext
                {
                    BaseScale = Vector3.one,
                    TargetScale = Vector3.one * 2f,
                    Influence = Vector3.one
                });

            // Act
            tween.Update(0.5f);

            // Assert
            Assert.Greater(go.transform.localScale.x, 1f);
            Assert.Less(go.transform.localScale.x, 2f);

            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that cancel applies the target scale.
        /// </summary>
        [Test]
        public void Cancel_Applies_TargetScale()
        {
            // Arrange
            var go = new GameObject("ScaledObject");
            go.transform.localScale = Vector3.one;

            var tween = new ScaleTweenBinding();

            tween.Bind(
                go,
                1f,
                new TweenContext
                {
                    BaseScale = Vector3.one,
                    TargetScale = Vector3.one * 2f,
                    Influence = Vector3.one
                });

            // Act
            tween.Cancel();

            // Assert
            Assert.AreEqual(Vector3.one * 2f, go.transform.localScale);

            Object.DestroyImmediate(go);
        }
    }
}