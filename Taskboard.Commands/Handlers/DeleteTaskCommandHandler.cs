using System;
using System.Linq;
using System.Threading.Tasks;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Exceptions;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Handlers
{
    public class DeleteTaskCommandHandler : ICommandHander<DeleteTaskCommand>
    {
        private readonly IListRepository repo;

        public DeleteTaskCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task Execute(DeleteTaskCommand command)
        {
            var list = await repo.GetById(command.ListId);

            var task = list.Tasks.FirstOrDefault(t => t.Id == command.TaskId);

            if (task == null)
            {
                throw ResourceNotFoundException.FromResourceId(command.TaskId);
            }

            list.Tasks.Remove(task);

            await repo.Replace(list);
        }
    }
}