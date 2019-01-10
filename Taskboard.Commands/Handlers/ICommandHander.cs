using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Handlers
{
    public interface ICommandHander<TCommand> where TCommand : ICommand
    {
        Task<Option<CommandFailure>> Execute(TCommand command);
    }

    public interface ICommandHander<TCommand, TResult> where TCommand : ICommand
    {
        Task<Option<TResult, CommandFailure>> Execute(TCommand command);
    }
}