namespace Taskboard.Commands.Commands
{
    public class UpdateTaskCommand : ICommand
    {
        public string ListId { get; set; }
        public string TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}