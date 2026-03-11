using Pihkura.Tween.Core;
using UnityEngine;

namespace Pihkura.Tween.Library
{
    /// <summary>
    /// Tween binding that interpolates a float parameter on a material instance
    /// over time.
    /// 
    /// Typically used for shader-driven effects such as fades, highlights,
    /// dissolve amounts, or other custom material properties.
    /// </summary>
    public class MaterialTweenBinding : TweenBinding<MeshRenderer>
    {

        private MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        /// <summary>
        /// Initializes the material property to the base (starting) value
        /// defined in the tween context.
        /// </summary>
        public override void OnBind()
        {
            this.Target.GetPropertyBlock(mpb);
            mpb.SetFloat(this.Context.TweenKey, this.Context.BaseValue);
            this.Target.SetPropertyBlock(mpb);
        }

        /// <summary>
        /// Advances the tween and applies the interpolated value
        /// to the target material property.
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

            // Interpolate material float property
            this.Target.GetPropertyBlock(mpb);
            mpb.SetFloat(this.Context.TweenKey, Mathf.Lerp(this.Context.BaseValue, this.Context.TargetValue, t));
            this.Target.SetPropertyBlock(mpb);

            return this.Timer >= this.Duration;
        }

        /// <summary>
        /// Cancels the tween and immediately applies the target value
        /// to the material property.
        /// </summary>
        public override void Cancel()
        {
            this.Target.GetPropertyBlock(mpb);
            mpb.SetFloat(this.Context.TweenKey, this.Context.TargetValue);
            this.Target.SetPropertyBlock(mpb);
        }
    }
}
