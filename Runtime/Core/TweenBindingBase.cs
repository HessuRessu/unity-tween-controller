using UnityEngine;

namespace Pihkura.Tween.Core
{
    /// <summary>
    /// Non-generic base class for all tween bindings.
    /// 
    /// Defines the minimal contract required by the tween system
    /// so that bindings can be updated, completed, cancelled,
    /// and pooled without knowing their concrete type.
    /// </summary>
    public abstract class TweenBindingBase
    {
        /// <summary>
        /// Advances the tween simulation.
        /// </summary>
        /// <param name="deltaTime">Time step in seconds.</param>
        /// <returns>
        /// True if the tween has completed during this update; otherwise false.
        /// </returns>
        public abstract bool Update(float deltaTime);

        /// <summary>
        /// Resets all internal state so this binding can be reused
        /// from a pool.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Marks the tween as completed.
        /// </summary>
        /// <param name="invokeOnComplete">
        /// If true, completion callbacks should be invoked.
        /// </param>
        public abstract void Complete(bool invokeOnComplete);

        /// <summary>
        /// Cancels the tween without treating it as a normal completion.
        /// Implementations typically stop further updates and cleanup state.
        /// </summary>
        public abstract void Cancel();

        /// <summary>
        /// Untyped access to the tween target.
        /// 
        /// Used by systems that operate on bindings generically
        /// (e.g., cancelling all tweens targeting a specific object).
        /// </summary>
        public abstract Object UntypedTarget { get; }
    }
}
