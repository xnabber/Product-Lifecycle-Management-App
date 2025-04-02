using PLMApp.Models;

namespace PLMProject.Services
{
    public interface IStageService
    {
        Task<IEnumerable<Stage>> GetAllAsync();
        Task<Stage?> GetByIdAsync(int stagesId);
        Task<bool> AddStageAsync(Stage stage);
        Task<bool> UpdateStageAsync(Stage stage);
        Task<bool> DeleteAsync(int stagesId);
    }
}
