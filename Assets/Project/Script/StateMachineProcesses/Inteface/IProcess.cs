namespace Grower
{
    /// <summary>
    /// Interface representing a process that can be executed.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Gets the unique identifier for the process.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// Executes the process with the provided arguments.
        /// </summary>
        /// <param name="args">The arguments to be passed during the execution of the process.</param>
        void Execute(params object[] args);
    }
}