namespace Grower
{
    public interface IProcess
    {
        string ID { get; }
        void Execute(params object[] args);
    }
}