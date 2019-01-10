using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Handlers
{
    public interface ICommandHander<TCommand> where TCommand : ICommand
    {
        Task<Option<OperationFailure>> Execute(TCommand command);
    }

    public interface ICommandHander<TCommand, TResult> where TCommand : ICommand
    {
        Task<Option<TResult, OperationFailure>> Execute(TCommand command);
    }
}