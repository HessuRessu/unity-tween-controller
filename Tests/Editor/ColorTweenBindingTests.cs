using NUnit.Framework;
using UnityEngine;
using Pihkura.Tween.Library;
using Pihkura.Tween.Data;
using UnityEngine.UI;

namespace Pihkura.Tween.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ColorTweenBinding"/>.
    /// </summary>
    public class ColorTweenBindingTests
    {
        /// <summary>
        /// Verifies that color starts at BaseColor when bound.
        /// </summary>
        [Test]
        public void Color_Starts_At_BaseColor()
        {
            // Arrange
            var go = new GameObject("Graphic");
            var image = go.AddComponent<Image>();

            var tween = new ColorTweenBinding();

            // Act
            tween.Bind(
                image,
                1f,
                new TweenContext
                {
                    BaseColor = Color.black,
                    TargetColor = Color.white
                });

            // Assert
            Assert.AreEqual(Color.black, image.color);

            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that color interpolates over time.
        /// </summary>
        [Test]
        public void Color_Interpolates_Towards_Target()
        {
            // Arrange
            var go = new GameObject("Graphic");
            var image = go.AddComponent<Image>();

            var tween = new ColorTweenBinding();

            tween.Bind(
                image,
                1f,
                new TweenContext
                {
                    BaseColor = Color.black,
                    TargetColor = Color.white
                });

            // Act
            tween.Update(0.5f);

            // Assert
            Assert.AreNotEqual(Color.black, image.color);
            Assert.AreNotEqual(Color.white, image.color);

            Object.DestroyImmediate(go);
        }

        /// <summary>
        /// Verifies that cancel applies the target color.
        /// </summary>
        [Test]
        public void Cancel_Applies_TargetColor()
        {
            // Arrange
            var go = new GameObject("Graphic");
            var image = go.AddComponent<Image>();

            var tween = new ColorTweenBinding();

            tween.Bind(
                image,
                1f,
                new TweenContext
                {
                    BaseColor = Color.black,
                    TargetColor = Color.white
                });

            // Act
            tween.Cancel();

            // Assert
            Assert.AreEqual(Color.white, image.color);

            Object.DestroyImmediate(go);
        }
    }
}
