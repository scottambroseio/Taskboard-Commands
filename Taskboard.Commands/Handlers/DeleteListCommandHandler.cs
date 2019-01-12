using System;
using System.Threading.Tasks;
using Taskboard.Commands.Commands;
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

        public Task Execute(DeleteListCommand command)
        {
            return repo.Delete(command.Id);
        }
    }
}