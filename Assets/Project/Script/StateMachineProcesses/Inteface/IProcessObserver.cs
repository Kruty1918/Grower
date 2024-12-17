namespace Grower
{
    /// <summary>
    /// Interface for observers of processes, which react to the execution of a process.
    /// </summary>
    public interface IProcessObserver
    {
        /// <summary>
        /// Called after a process has been executed.
        /// </summary>
        /// <param name="processID">The identifier of the executed process.</param>
        /// <param name="args">Arguments passed during the execution of the process.</param>
        void OnProcessExecuted(string processID, params object[] args);
    }
}