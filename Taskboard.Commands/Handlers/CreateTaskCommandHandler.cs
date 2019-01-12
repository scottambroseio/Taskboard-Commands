using System;
using System.Threading.Tasks;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Handlers
{
    public class CreateTaskCommandHandler : ICommandHander<CreateTaskCommand, string>
    {
        private readonly IListRepository repo;

        public CreateTaskCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<string> Execute(CreateTaskCommand command)
        {
            var list = await repo.GetById(command.ListId);

            var task = new Domain.Task
            {
                Id = Guid.NewGuid().ToString(),
                Description = command.Description,
                Name = command.Name
            };

            list.Tasks.Add(task);

            await repo.Replace(list);

            return task.Id;
        }
    }
}