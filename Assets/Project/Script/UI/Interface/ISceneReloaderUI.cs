namespace Grower
{
    /// <summary>
    /// Interface for UI elements responsible for scene reloading transitions.
    /// </summary>
    public interface ISceneReloaderUI
    {
        /// <summary>
        /// Duration of the transition when the scene is transitioning in.
        /// </summary>
        float TransitionInDuration { get; }

        /// <summary>
        /// Duration of the transition when the scene is transitioning out.
        /// </summary>
        float TransitionOutDuration { get; }

        /// <summary>
        /// Triggers the transition into the scene.
        /// </summary>
        void TransitionIn();

        /// <summary>
        /// Triggers the transition out of the scene.
        /// </summary>
        void TransitionOut();
    }
}