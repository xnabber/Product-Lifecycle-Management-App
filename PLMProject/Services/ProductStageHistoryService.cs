using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PLMApp.Services
{
    public class ProductStageHistoryService : IProductStageHistoryService
    {
        private readonly PLMContext _context;

        public ProductStageHistoryService(PLMContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductStageHistory>> GetAllAsync()
        {
            return await _context.ProductStageHistories
                .Include(psh => psh.Product)
                .Include(psh => psh.Stage)
                .Include(psh => psh.User)
                .ToListAsync();
        }

        public async Task<ProductStageHistory?> GetByIdAsync(int productId, int stageId, DateTime startOfStage)
        {
            return await _context.ProductStageHistories
                .Include(psh => psh.Product)
                .Include(psh => psh.Stage)
                .Include(psh => psh.User)
                .FirstOrDefaultAsync(psh => psh.ProductId == productId && psh.StageId == stageId && psh.StartOfStage == startOfStage);
        }

        public async Task<ProductStageHistory?> AddStageTransitionAsync(ProductStageHistoryDto productStageHistoryDto)
        {
            // Get product
            var product = await _context.Products.FindAsync(productStageHistoryDto.ProductId);
            if (product == null) return null;

            var stage = await _context.Stages.FindAsync(productStageHistoryDto.StageId);
            if (stage == null) return null;

            // Get user and validate role
            var user = await _context.Users.FindAsync(productStageHistoryDto.UserId);
            if (user == null) return null;

            // Get the last stage
            var lastStage = await _context.ProductStageHistories
                .Where(psh => psh.ProductId == productStageHistoryDto.ProductId)
                .OrderByDescending(psh => psh.StartOfStage)
                .FirstOrDefaultAsync();

            DateTime dateTime = DateTime.Parse(productStageHistoryDto.StartOfStage);

            // Add new stage record
            var newStageHistory = new ProductStageHistory
            {
                ProductId = productStageHistoryDto.ProductId,
                StageId = productStageHistoryDto.StageId,
                StartOfStage = dateTime,
                User = user,
                Product = product,
                Stage = stage,
            };

            _context.ProductStageHistories.Add(newStageHistory);
            await _context.SaveChangesAsync();
            return newStageHistory;
        }

        private bool IsValidStageTransition(int currentStageId, int newStageId)
        {
            // Example: Define allowed transitions
            var validTransitions = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 2 } }, // Concept -> Feasibility
                { 2, new List<int> { 3 } }, // Feasibility -> Design
                { 3, new List<int> { 4 } }, // Design -> Production
                { 4, new List<int> { 5, 6 } }, // Production -> Withdrawal or Standby
                { 6, new List<int> { 4 } }, // Standby -> Production
            };

            return validTransitions.ContainsKey(currentStageId) &&
                   validTransitions[currentStageId].Contains(newStageId);
        }

        public async Task<bool> DeleteAsync(int productId, int stageId, DateTime startOfStage)
        {
            var history = await _context.ProductStageHistories
                .FindAsync(productId, stageId, startOfStage);

            if (history == null)
            {
                return false;
            }

            _context.ProductStageHistories.Remove(history);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
