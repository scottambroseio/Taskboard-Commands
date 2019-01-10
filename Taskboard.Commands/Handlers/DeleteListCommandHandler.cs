using System;
using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Handlers
{
    public class DeleteListCommandHandler : ICommandHander<DeleteListCommand>
    {
        private readonly IListRepository repo;

        public DeleteListCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Option<CommandFailure>> Execute(DeleteListCommand command)
        {
            var result = await repo.Delete(command.Id);

            return result.Match(
                failure => Option.Some(CommandFailure.Error),
                () => Option.None<CommandFailure>()
            );
        }
    }
}