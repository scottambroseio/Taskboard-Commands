namespace Taskboard.Commands.Commands
{
    public class DeleteTaskCommand : ICommand
    {
        public string ListId { get; set; }
        public string TaskId { get; set; }
    }
}