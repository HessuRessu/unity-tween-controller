using Pihkura.Tween.Core;
using UnityEngine;

namespace Pihkura.Tween.Library
{
    /// <summary>
    /// Tween binding that interpolates a <see cref="GameObject"/>'s local scale
    /// over time.
    /// 
    /// Supports per-axis influence via <see cref="TweenContext.Influence"/>,
    /// allowing selective scaling on individual axes.
    /// </summary>
    public class ScaleTweenBinding : TweenBinding<GameObject>
    {
        /// <summary>
        /// Advances the tween and applies interpolated local scale to the target.
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
            
            // Apply influence per-axis by weighting the delta from BaseScale to TargetScale
            Vector3 baseScale = this.Context.BaseScale;
            Vector3 targetScale = this.Context.TargetScale;

            // Expected range: 0..1 per axis
            Vector3 influence = this.Context.Influence;

            // Compute target scale after influence masking
            Vector3 influencedTarget = new Vector3(
                baseScale.x + (targetScale.x - baseScale.x) * influence.x,
                baseScale.y + (targetScale.y - baseScale.y) * influence.y,
                baseScale.z + (targetScale.z - baseScale.z) * influence.z
            );

            // Interpolate between base scale and influenced target scale
            this.Target.transform.localScale = Vector3.Lerp(baseScale, influencedTarget, t);

            return this.Timer >= this.Duration;
        }

        /// <summary>
        /// Cancels the tween and immediately applies the target scale.
        /// </summary>
        public override void Cancel()
        {
            this.Target.transform.localScale = this.Context.TargetScale;
        }
    }
}
