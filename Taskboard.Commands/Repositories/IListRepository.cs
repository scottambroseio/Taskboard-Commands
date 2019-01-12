using System.Threading.Tasks;
using Taskboard.Commands.Domain;
using Task = System.Threading.Tasks.Task;

namespace Taskboard.Commands.Repositories
{
    public interface IListRepository
    {
        Task<List> GetById(string id);
        Task<string> Create(List list);
        Task Replace(List list);
        Task Delete(string id);
    }
}