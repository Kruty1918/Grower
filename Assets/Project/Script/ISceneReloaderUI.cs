namespace Grower
{
    public interface ISceneReloaderUI
    {
        float TransitionInDuration { get; }
        float TransitionOutDuration { get; }

        void TransitionIn();
        void TransitionOut();
    }
}