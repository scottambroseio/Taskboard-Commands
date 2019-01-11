namespace Taskboard.Commands.Commands
{
    public class CreateTaskCommand : ICommand
    {
        public string ListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}