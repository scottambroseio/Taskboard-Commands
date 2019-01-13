namespace Taskboard.Commands.Commands
{
    public class MoveTaskCommand : ICommand
    {
        public string FromListId { get; set; }
        public string ToListId { get; set; }
        public string TaskId { get; set; }
    }
}