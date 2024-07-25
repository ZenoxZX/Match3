using Cysharp.Threading.Tasks;

namespace CommandSystem
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        UniTask WaitForCompletion();
    }
}