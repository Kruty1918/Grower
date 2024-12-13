using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    public class DefaultProcessExecutor : IProcessExecutor
    {
        private readonly Dictionary<string, ProcessBase> processDictionary;

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

        public void ExecuteProcess(string processID, params object[] args)
        {
            if (processDictionary.TryGetValue(processID, out var process))
            {
                process.Execute(args);
            }
            else
            {
                Debug.LogWarning($"Process with ID '{processID}' not found!");
            }
        }
    }
}