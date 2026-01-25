using Pihkura.Tween.Core;
using UnityEngine;

namespace Pihkura.Tween.Library
{
    /// <summary>
    /// Tween binding that interpolates the <see cref="CanvasGroup.alpha"/> value
    /// over time.
    /// 
    /// Commonly used for UI fade-in and fade-out effects.
    /// </summary>
    public class CanvasGroupAlphaTweenBinding : TweenBinding<CanvasGroup>
    {
        /// <summary>
        /// Initializes the target state when the tween is bound.
        /// Sets the alpha to the base (starting) value defined in the context.
        /// </summary>
        public override void OnBind()
        {
            this.Target.alpha = this.Context.BaseValue;
        }

        /// <summary>
        /// Advances the tween and applies interpolated alpha to the target.
        /// </summary>
        /// <param name="deltaTime">Time step in seconds.</param>
        /// <returns>
        /// True when the tween has reached its duration and is complete.
        /// </returns>
        public override bool Update(float deltaTime)
        {
            this.Timer += deltaTime;

            // Normalized interpolation factor
            float t = Mathf.Clamp01(this.Timer / this.Duration);

            // Interpolate alpha
            this.Target.alpha = Mathf.Lerp(this.Context.BaseValue, this.Context.TargetValue, t);

            return this.Timer >= this.Duration;
        }

        /// <summary>
        /// Cancels the tween and immediately applies the target value.
        /// </summary>
        public override void Cancel()
        {
            this.Target.alpha = this.Context.TargetValue;
        }
    }
}
