using Pihkura.Tween.Core;
using UnityEngine;

namespace Pihkura.Tween.Library
{
    /// <summary>
    /// Tween binding that acts purely as a time-based trigger.
    /// 
    /// Does not modify any properties on the target. Instead, it completes
    /// after the specified duration, allowing callers to use the tween system
    /// as a lightweight delayed callback mechanism.
    /// </summary>
    public sealed class DelayedTriggerTweenBinding : TweenBinding<Behaviour>
    {
        /// <summary>
        /// Called when the tween is bound.
        /// Intentionally empty because this tween only measures time
        /// and does not initialize any target state.
        /// </summary>
        public override void OnBind()
        {
            // Intentionally empty: we only care about time passing
        }

        /// <summary>
        /// Advances the internal timer.
        /// </summary>
        /// <param name="deltaTime">Time step in seconds.</param>
        /// <returns>
        /// True when the accumulated time reaches or exceeds <see cref="Duration"/>.
        /// </returns>
        public override bool Update(float deltaTime)
        {
            this.Timer += deltaTime;
            return this.Timer >= this.Duration;
        }

        /// <summary>
        /// Cancels the delayed trigger.
        /// 
        /// No state needs to be restored because this binding does not
        /// modify the target.
        /// </summary>
        public override void Cancel()
        {
            // Intentionally empty
        }
    }
}
