using Pihkura.Tween.Core;
using UnityEngine;

namespace Pihkura.Tween.Library
{
    /// <summary>
    /// Tween binding that interpolates the alpha channel of a LineRenderer's
    /// color gradient over time.
    /// 
    /// This binding is typically used for fade-in / fade-out effects on
    /// line-based visuals such as paths, territory borders, selection outlines,
    /// or other procedural line graphics.
    /// </summary>
    public class LineRendererAlphaTweenBinding : TweenBinding<LineRenderer>
    {
        /// <summary>
        /// Called when the binding is first attached to a target.
        /// Initializes the LineRenderer alpha to the start (base) value
        /// defined in the tween context.
        /// </summary>
        public override void OnBind()
        {
            this.SetAlphaKeys(0f);
        }

        /// <summary>
        /// Advances the tween by the given time step.
        /// Updates the LineRenderer's alpha based on the normalized tween
        /// progress.
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update (seconds).</param>
        /// <returns>
        /// True when the tween has reached its duration and is complete.
        /// </returns>
        public override bool Update(float deltaTime)
        {
            this.Timer += deltaTime;

            // Normalized interpolation factor
            float t = Mathf.Clamp01(this.Timer / this.Duration);

            // Interpolate alpha
            this.SetAlphaKeys(t);

            return this.Timer >= this.Duration;
        }

        /// <summary>
        /// Cancels the tween and immediately applies the final (target)
        /// alpha value.
        /// </summary>
        public override void Cancel()
        {
            this.SetAlphaKeys(1f);
        }

        /// <summary>
        /// Updates all alpha keys in the LineRenderer's color gradient
        /// by interpolating between the base and target values.
        /// </summary>
        /// <param name="time">
        /// Normalized interpolation value (0 = base, 1 = target).
        /// </param>
        private void SetAlphaKeys(float time)
        {
            Gradient gradient = this.Target.colorGradient;

            var keys = gradient.alphaKeys;

            // Update each alpha key with the interpolated value
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].alpha = Mathf.Lerp(this.Context.BaseValue, this.Context.TargetValue, time);
            }

            gradient.alphaKeys = keys;
            this.Target.colorGradient = gradient;
        }
    }
}
