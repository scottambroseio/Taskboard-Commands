using System;
using System.Threading.Tasks;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Handlers
{
    public class CreateListCommandHandler : ICommandHander<CreateListCommand, string>
    {
        private readonly IListRepository repo;

        public CreateListCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<string> Execute(CreateListCommand command)
        {
            var list = new List {Id = Guid.NewGuid().ToString(), Name = command.Name};

            return repo.Create(list);
        }
    }
}