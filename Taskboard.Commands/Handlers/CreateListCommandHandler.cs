using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Handlers
{
    public class CreateListCommandHandler : ICommandHander<CreateListCommand, string>
    {
        public Task<Option<string, OperationFailure>> Execute(CreateListCommand command)
        {
            return Task.FromResult(Option.Some<string, OperationFailure>("location"));
        }
    }
}