using UnityEngine;

namespace Grower
{
    /// <summary>
    /// A strategy for handling input using swipe gestures on mobile platforms.
    /// </summary>
    [CreateAssetMenu(fileName = "SwipeInputStrategy", menuName = "Grower/Input Strategy/Swipe")]
    public class SwipeInputStrategy : InputStrategy
    {
        #region Fields

        /// <summary>
        /// The minimum distance required for a valid swipe.
        /// </summary>
        [SerializeField] private float minDist = 0.2f;

        /// <summary>
        /// The maximum time allowed for a swipe to be valid.
        /// </summary>
        [SerializeField] private float maxTime = 1f;

        /// <summary>
        /// The direction threshold to detect significant swipe directions.
        /// </summary>
        [SerializeField] private float dirThreshold = 0.9f;

        #endregion

        #region Methods

        /// <summary>
        /// Returns a swipe input processor configured with the defined swipe parameters.
        /// </summary>
        /// <returns>An instance of <see cref="SwipeInputProcessor"/> for processing swipe inputs.</returns>
        public override IInputProcessor GetInputProcessor()
        {
            return new SwipeInputProcessor(minDist, maxTime, dirThreshold);
        }

        #endregion
    }
}