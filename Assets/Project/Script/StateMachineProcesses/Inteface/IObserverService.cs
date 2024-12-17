namespace Grower
{
    /// <summary>
    /// Interface for observer services to manage and retrieve observers.
    /// Provides methods for getting observers by their ID and cleaning up observers.
    /// </summary>
    public interface IObserverService
    {
        /// <summary>
        /// Retrieves an observer by its ID.
        /// </summary>
        /// <param name="observerID">The ID of the observer to retrieve.</param>
        /// <returns>The observer associated with the provided ID, or null if not found.</returns>
        IProcessObserver GetObserverById(string observerID);

        /// <summary>
        /// Cleans up observers that are no longer needed.
        /// Removes observers that are deletable or invalid.
        /// </summary>
        void CleanupObservers();
    }
}