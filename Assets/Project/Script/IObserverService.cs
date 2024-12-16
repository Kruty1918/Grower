namespace Grower
{
    /// <summary>
    /// Interface for observer services to manage and retrieve observers.
    /// </summary>
    public interface IObserverService
    {
        IProcessObserver GetObserverById(string observerID);
        void CleanupObservers();
    }
}