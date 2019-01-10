using System;
using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Handlers
{
    public class CreateListCommandHandler : ICommandHander<CreateListCommand, Uri>
    {
        private readonly IListRepository repo;

        public CreateListCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Option<Uri, CommandFailure>> Execute(CreateListCommand command)
        {
            var list = new List {Id = Guid.NewGuid().ToString(), Name = command.Name};

            var result = await repo.Create(list);

            return result.Match(
                id => Option.Some<Uri, CommandFailure>(
                    new Uri(string.Format(Environment.GetEnvironmentVariable("LIST_RESOURCE_URI"), id))),
                failure => Option.None<Uri, CommandFailure>(CommandFailure.Error)
            );
        }
    }
}