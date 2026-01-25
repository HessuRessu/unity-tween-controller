using Pihkura.Tween.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Pihkura.Tween.Library
{
    /// <summary>
    /// Tween binding that interpolates the <see cref="Graphic.color"/> value
    /// over time.
    /// 
    /// Commonly used for fading UI graphics or smoothly transitioning
    /// between colors.
    /// </summary>
    public class ColorTweenBinding : TweenBinding<Graphic>
    {
        
        /// <summary>
        /// Initializes the target state when the tween is bound.
        /// Sets the color to the base (starting) value defined in the context.
        /// </summary>
        public override void OnBind()
        {
            this.Target.color = this.Context.BaseColor;
        }

        /// <summary>
        /// Advances the tween and applies interpolated color to the target.
        /// </summary>
        /// <param name="deltaTime">Time step in seconds.</param>
        /// <returns>
        /// True when the tween has reached its duration and is complete.
        /// </returns>
        public override bool Update(float deltaTime)
        {
            this.Timer += deltaTime;

            // Normalized interpolation factor
            float t = Mathf.Clamp01(Timer / Duration);

            // Interpolate color
            this.Target.color = Color.Lerp(this.Context.BaseColor, this.Context.TargetColor, t);

            return this.Timer >= this.Duration;
        }

        /// <summary>
        /// Cancels the tween and immediately applies the target color.
        /// </summary>
        public override void Cancel()
        {
            this.Target.color = this.Context.TargetColor;
        }
    }
}
