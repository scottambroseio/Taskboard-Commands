using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Handlers
{
    public class DeleteListCommandHandler : ICommandHander<DeleteListCommand>
    {
        public Task<Option<OperationFailure>> Execute(DeleteListCommand command)
        {
            return Task.FromResult(Option.None<OperationFailure>());
        }
    }
}