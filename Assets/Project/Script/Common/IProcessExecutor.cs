namespace Grower
{
    /// <summary>
    /// Defines a contract for objects that can execute a process identified by a process ID.
    /// </summary>
    public interface IProcessExecutor
    {
        /// <summary>
        /// Executes a process based on the given process ID and additional arguments.
        /// </summary>
        /// <param name="processID">The unique identifier of the process to be executed.</param>
        /// <param name="args">Additional arguments that may be required for the execution of the process.</param>
        void ExecuteProcess(string processID, params object[] args);
    }
}