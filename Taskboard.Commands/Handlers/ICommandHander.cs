using System.Threading.Tasks;
using Taskboard.Commands.Commands;

namespace Taskboard.Commands.Handlers
{
    public interface ICommandHander<TCommand> where TCommand : ICommand
    {
        Task Execute(TCommand command);
    }

    public interface ICommandHander<TCommand, TResult> where TCommand : ICommand
    {
        Task<TResult> Execute(TCommand command);
    }
}