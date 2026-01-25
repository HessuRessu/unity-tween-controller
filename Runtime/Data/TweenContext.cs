using UnityEngine;

namespace Pihkura.Tween.Data
{
    /// <summary>
    /// Container describing the initial and target values for a tween operation.
    /// 
    /// This struct is passed to tween bindings to define what values should be
    /// interpolated and how the interpolation is influenced.
    /// </summary>
    public struct TweenContext
    {
        /// <summary>
        /// String based key for facilitating tween bindings.
        /// </summary>
        public string TweenKey;

        /// <summary>
        /// Starting scale value for vector-based tweeners.
        /// </summary>
        public Vector3 BaseScale;

        /// <summary>
        /// Target scale value for vector-based tweeners.
        /// </summary>
        public Vector3 TargetScale;

        /// <summary>
        /// Starting color value for color-based tweeners.
        /// </summary>
        public Color BaseColor;

        /// <summary>
        /// Target color value for color-based tweeners.
        /// </summary>
        public Color TargetColor;

        /// <summary>
        /// Starting scalar value for float-based tweeners.
        /// </summary>
        public float BaseValue;

        /// <summary>
        /// Target scalar value for float-based tweeners.
        /// </summary>
        public float TargetValue;

        /// <summary>
        /// Optional influence vector used to weight or mask interpolation
        /// (e.g., axis-specific scaling or directional influence).
        /// </summary>
        public Vector3 Influence;
    }
}
