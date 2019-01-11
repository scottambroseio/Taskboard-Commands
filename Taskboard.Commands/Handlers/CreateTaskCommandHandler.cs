using System;
using System.Threading.Tasks;
using Optional;
using Optional.Unsafe;
using Taskboard.Commands.Commands;
using Taskboard.Commands.Enums;
using Taskboard.Commands.Extensions;
using Taskboard.Commands.Repositories;
using Task = Taskboard.Commands.Domain.Task;

namespace Taskboard.Commands.Handlers
{
    public class CreateTaskCommandHandler : ICommandHander<CreateTaskCommand>
    {
        private readonly IListRepository repo;

        public CreateTaskCommandHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Option<CommandFailure>> Execute(CreateTaskCommand command)
        {
            var getResult = await repo.GetById(command.ListId);

            if (!getResult.HasValue)
            {
                return Option.Some(getResult.ExceptionOrFailure().MapToCommandFailure());
            }

            var list = getResult.ValueOrFailure();

            var task = new Task
            {
                Id = Guid.NewGuid().ToString(),
                Description = command.Description,
                Name = command.Name
            };

            list.Tasks.Add(task);

            var replaceResult = await repo.Replace(list);

            return replaceResult.Match(
                error => Option.Some(error.MapToCommandFailure()),
                () => Option.None<CommandFailure>()
            );
        }
    }
}