using System;
using System.Linq;
using System.Threading.Tasks;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Exceptions;
using Taskboard.Commands.Repositories;

namespace Taskboard.Commands.Handlers
{
    public class MoveTaskCommandHandler : ICommandHander<MoveTaskCommand>
    {
        private readonly IListRepository repo;

        public MoveTaskCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task Execute(MoveTaskCommand command)
        {
            // todo: use a custom cosmos stored proc in order to provide a transaction

            var from = await repo.GetById(command.FromListId);

            var task = from.Tasks.FirstOrDefault(t => t.Id == command.TaskId);

            if (task == null)
            {
                throw ResourceNotFoundException.FromResourceId(command.TaskId);
            }

            var to = await repo.GetById(command.ToListId);

            from.Tasks.Remove(task);
            to.Tasks.Add(task);

            // no transaction so might as well save concurrently
            await Task.WhenAll(repo.Replace(from), repo.Replace(to));
        }
    }
}