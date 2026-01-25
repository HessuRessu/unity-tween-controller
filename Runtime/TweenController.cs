using System.Collections.Generic;
using Pihkura.Tween.Core;
using UnityEngine;

namespace Pihkura.Tween
{
    /// <summary>
    /// Central controller responsible for managing, updating, and pooling all active tweens.
    /// 
    /// Owns the lifetime of <see cref="TweenBindingBase"/> instances, updates them every frame,
    /// and recycles completed bindings to avoid runtime allocations.
    /// 
    /// Implemented as a scene-level singleton.
    /// </summary>
    public class TweenController : MonoBehaviour
    {
        /// <summary>
        /// List of currently active tween bindings.
        /// These are updated every frame by <see cref="Update"/>.
        /// </summary>
        private List<TweenBindingBase> _active;

        /// <summary>
        /// Pool of inactive tween bindings that can be reused.
        /// Used to reduce allocations and GC pressure.
        /// </summary>
        private List<TweenBindingBase> _reserve;

        #region SingletonInstance

        /// <summary>
        /// Singleton instance of the <see cref="TweenController"/>.
        /// There must be exactly one instance present in the scene.
        /// </summary>
        public static TweenController Instance { get; private set; }

        /// <summary>
        /// Initializes the singleton instance and internal collections.
        /// If another instance already exists, this GameObject is destroyed.
        /// </summary>
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            this._active = new List<TweenBindingBase>();
            this._reserve = new List<TweenBindingBase>();
        }
        #endregion


        #region UnityCallbacks

        /// <summary>
        /// Updates all active tweens.
        /// Completed tweens are returned to the reserve pool.
        /// </summary>
        void Update()
        {
            if (this._active.Count == 0)
                return;

            // Iterate backwards so elements can be safely removed while iterating
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                if (this._active[i].Update(Time.deltaTime))
                {
                    // Tween reports completion
                    this._active[i].Complete(true);
                    this._reserve.Add(this._active[i]);
                    this._active.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Clears the singleton reference when this controller is destroyed.
        /// </summary>
        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
        #endregion

        /// <summary>
        /// Retrieves a tween binding of the given type.
        /// 
        /// Reuses an existing binding from the reserve pool when possible,
        /// otherwise creates a new instance.
        /// The returned binding is automatically added to the active list.
        /// </summary>
        /// <typeparam name="TBinding">Concrete tween binding type.</typeparam>
        /// <returns>Initialized tween binding ready for configuration.</returns>
        public TBinding Get<TBinding>() where TBinding : TweenBindingBase, new()
        {
            // Try to reuse a binding from the reserve pool
            for (int i = this._reserve.Count - 1; i >= 0; i--)
            {
                if (this._reserve[i] is TBinding typed)
                {
                    this._reserve.RemoveAt(i);
                    typed.Reset();
                    this._active.Add(typed);
                    return typed;
                }
            }

            // Create a new binding if none were available
            TBinding created = new TBinding();
            this._active.Add(created);
            return created;
        }

        /// <summary>
        /// Cancels all active tweens that target the given object.
        /// </summary>
        /// <param name="target">Target object whose tweens should be cancelled.</param>
        /// <param name="invokeOnComplete">
        /// If true, completion callbacks are invoked when cancelling.
        /// </param>
        public void Cancel(Object target, bool invokeOnComplete = true)
        {
            // Iterate backwards so elements can be safely removed while iterating
            for (int i = this._active.Count - 1; i >= 0; i--)
            {
                if (this._active[i].UntypedTarget == target)
                {
                    this._active[i].Complete(invokeOnComplete);
                    this._reserve.Add(this._active[i]);
                    this._active.RemoveAt(i);
                }
            }
        }
    }
}
