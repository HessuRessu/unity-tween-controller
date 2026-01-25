using Pihkura.Tween.Data;
using UnityEngine;

namespace Pihkura.Tween.Core
{
    /// <summary>
    /// Generic base class for strongly-typed tween bindings.
    /// 
    /// A tween binding represents a single running tween instance that
    /// drives some property of a specific target object over time.
    /// 
    /// This class provides common state such as timing, target reference,
    /// and completion handling, while concrete subclasses implement
    /// the actual interpolation logic.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the target object that this tween operates on.
    /// Must derive from <see cref="UnityEngine.Object"/>.
    /// </typeparam>
    public abstract class TweenBinding<T> : TweenBindingBase where T : Object
    {
        /// <summary>
        /// Total duration of the tween in seconds.
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Current elapsed time of the tween in seconds.
        /// </summary>
        public float Timer { get; set; }

        /// <summary>
        /// Strongly-typed target object affected by this tween.
        /// </summary>
        public T Target { get; set; }

        /// <summary>
        /// Context data describing easing, looping, and other tween parameters.
        /// </summary>
        public TweenContext Context { get; set; }

        /// <summary>
        /// Untyped access to the target object.
        /// Used by systems that operate on tween bindings generically
        /// (e.g., cancellation by target).
        /// </summary>
        public override Object UntypedTarget => this.Target;

        /// <summary>
        /// Optional callback invoked when the tween completes.
        /// </summary>
        public System.Action onComplete;
 
        /// <summary>
        /// Updates the tween state.
        /// 
        /// Implementations should advance <see cref="Timer"/>,
        /// apply interpolation to the target, and return true when
        /// the tween has finished.
        /// </summary>
        /// <param name="deltaTime">Time step in seconds.</param>
        /// <returns>
        /// True if the tween has completed during this update; otherwise false.
        /// </returns>
        public override bool Update(float deltaTime)
        {
            return true;
        }

        /// <summary>
        /// Resets this binding to a clean state so it can be reused
        /// from the pool.
        /// </summary>
        public override void Reset()
        {
            this.Timer = 0;
            this.Target = null;
            this.onComplete = null;
        }

        /// <summary>
        /// Completes the tween and optionally invokes the completion callback.
        /// </summary>
        /// <param name="invokeOnComplete">
        /// If true, <see cref="onComplete"/> is invoked.
        /// </param>
        public override void Complete(bool invokeOnComplete)
        {
            if (invokeOnComplete)
                this.onComplete?.Invoke();
        }

        /// <summary>
        /// Binds this tween to a target and initializes its runtime state.
        /// </summary>
        /// <param name="target">Target object to animate.</param>
        /// <param name="duration">Tween duration in seconds.</param>
        /// <param name="context">Tween configuration context.</param>
        public virtual void Bind(T target, float duration, in TweenContext context)
        {
            this.Target = target;
            this.Duration = duration;
            this.Context = context;
            this.OnBind();
        }

        /// <summary>
        /// Called after <see cref="Bind"/>.
        /// Subclasses can perform additional initialization here.
        /// </summary>
        public virtual void OnBind() { }
    }
}
