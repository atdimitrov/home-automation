using System.Threading.Tasks;

namespace HomeAutomation.Server.Interfaces
{
    public interface IHeatingLogicService
    {
        Task<Result<uint>> GetTargetNumberOfHeaters();
    }
}
