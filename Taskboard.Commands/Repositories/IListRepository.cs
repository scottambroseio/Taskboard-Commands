using System.Threading.Tasks;
using Optional;
using Taskboard.Commands.Domain;
using Taskboard.Commands.Enums;

namespace Taskboard.Commands.Repositories
{
    public interface IListRepository
    {
        Task<Option<string, CosmosFailure>> Create(List list);
        Task<Option<CosmosFailure>> Delete(string id);
    }
}