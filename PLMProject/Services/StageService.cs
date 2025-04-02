using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class StageService : IStageService
    {
        private readonly PLMContext _context;
        public StageService(PLMContext context)
        {
            _context = context;
        }
        public async Task<bool> AddStageAsync(Stage stage)
        {
            _context.Stages.Add(stage);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int stageId)
        {
            var stage = await _context.Stages.FindAsync(stageId);
            if (stage == null) return false;
            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Stage>> GetAllAsync()
        {
            var stages = await _context.Stages.ToListAsync();
            return stages;
        }
        public async Task<Stage?> GetByIdAsync(int stageId)
        {
            return await _context.Stages.FindAsync(stageId);
        }
        public async Task<bool> UpdateStageAsync(Stage stage)
        {
            var oldStage = await _context.Stages.FindAsync(stage.Id);
            if (oldStage == null) return false;
            oldStage.Name = stage.Name;
            oldStage.Description = stage.Description;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
