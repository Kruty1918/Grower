namespace Grower
{
    public interface IProcessExecutor
    {
        void ExecuteProcess(string processID, params object[] args);
    }
}