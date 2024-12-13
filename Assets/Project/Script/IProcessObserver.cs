namespace Grower
{
    /// <summary>
    /// Інтерфейс спостерігачів за процесами.
    /// </summary>
    public interface IProcessObserver
    {
        void OnProcessExecuted(string processID, params object[] args);
    }
}