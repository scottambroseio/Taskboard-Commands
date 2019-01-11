using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Extensions
{
    public static class CosmosFailureExtensions
    {
        public static CommandFailure MapToCommandFailure(this CosmosFailure failure)
        {
            switch (failure)
            {
                case CosmosFailure.NotFound: return CommandFailure.NotFound;
                default: return CommandFailure.Error;
            }
        }
    }
}