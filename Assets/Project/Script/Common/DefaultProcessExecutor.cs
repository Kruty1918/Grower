using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Default implementation of IProcessExecutor for executing processes by their ID.
    /// </summary>
    public class DefaultProcessExecutor : IProcessExecutor
    {
        private readonly Dictionary<string, ProcessBase> processDictionary;  // Dictionary of processes keyed by their ID

        /// <summary>
        /// Initializes a new instance of the DefaultProcessExecutor class.
        /// </summary>
        /// <param name="processes">A collection of processes to register.</param>
        public DefaultProcessExecutor(IEnumerable<ProcessBase> processes)
        {
            processDictionary = new Dictionary<string, ProcessBase>();
            foreach (var process in processes)
            {
                if (!string.IsNullOrWhiteSpace(process.ID))
                {
                    processDictionary[process.ID] = process;
                }
            }
        }

        /// <summary>
        /// Executes a process by its ID using the registered processes.
        /// </summary>
        /// <param name="processID">The ID of the process to execute.</param>
        /// <param name="args">Arguments to pass to the process.</param>
        public void ExecuteProcess(string processID, params object[] args)
        {
            if (processDictionary.TryGetValue(processID, out var process))
            {
                process.Execute(args);  // Execute the found process
            }
            else
            {
                Debug.LogWarning($"Process with ID '{processID}' not found!");  // Log a warning if the process is not found
            }
        }
    }
}