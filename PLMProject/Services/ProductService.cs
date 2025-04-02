using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class ProductService : IProductService
    {
        private readonly PLMContext _context;
        public ProductService(PLMContext context)
        {
            _context = context;
        }
        public async Task<Product?> AddProductAsync(ProductRequestDto product)
        {
            var bom = await _context.Boms.FindAsync(product.BomId);
            if (bom == null) return null;

            var newProduct = new Product
            {
                Bom = bom,
                Name = product.Name,
                Description = product.Description,
                EstimatedHeight = product.EstimatedHeight,
                EstimatedWeight = product.EstimatedWeight,
                EstimatedWidth = product.EstimatedWidth
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Bom)
                .Include(p => p.Bom.BomMaterials)
                    .ThenInclude(bm => bm.Material)
                .Include(p => p.ProductStageHistory
                    .OrderBy(psh => psh.StartOfStage))
                    .ThenInclude(psh => psh.Stage)
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.User)
                .ToListAsync();

            return products;
        }



        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Bom)
                .Include(p => p.Bom.BomMaterials)
                .Include(p => p.ProductStageHistory)
                .ThenInclude(psh => psh.Stage)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }


        public async Task<Product?> UpdateProductAsync(ProductRequestDto product)
        {
            var oldProduct = await _context.Products.FindAsync(product.Id);
            var newBom = await _context.Boms.FindAsync(product.BomId);
            if (oldProduct == null || newBom == null) return null;

            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.EstimatedHeight = product.EstimatedHeight;
            oldProduct.EstimatedWeight = product.EstimatedWeight;
            oldProduct.EstimatedWidth = product.EstimatedWidth;
            oldProduct.Bom = newBom;

            await _context.SaveChangesAsync();
            return oldProduct;
        }

        public async Task<IDictionary<string, int>> GetProductsGroupedByCurrentStageAsync()
        {
            var today = DateTime.Today;

            // Get all stages from the database
            var allStages = await _context.Stages.ToListAsync();

            // Get all products along with their stage history
            var products = await _context.Products
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.Stage)
                .ToListAsync();

            // Dictionary to store the count of products per stage
            var stageCounts = new Dictionary<string, int>();

            // Loop through each product to determine its current stage
            foreach (var product in products)
            {
                // Find the active stage for the product (the one that includes today's date)
                var currentStage = product.ProductStageHistory
                    .Where(psh => psh.StartOfStage <= today)
                    .OrderByDescending(psh => psh.StartOfStage)
                    .FirstOrDefault();

                if (currentStage != null)
                {
                    var stageName = currentStage.Stage.Name;

                    // Increment the count for the stage in the dictionary
                    if (stageCounts.ContainsKey(stageName))
                    {
                        stageCounts[stageName]++;
                    }
                    else
                    {
                        stageCounts[stageName] = 1;
                    }
                }
            }

            // Ensure all stages are included in the dictionary, even if their count is 0
            foreach (var stage in allStages)
            {
                if (!stageCounts.ContainsKey(stage.Name))
                {
                    stageCounts[stage.Name] = 0;
                }
            }

            return stageCounts;
        }

        public async Task<List<StageDurationDto>> GetProductStageDurationsAsync(int productId)
        {
            var today = DateTime.Today;

            // Find the product and include its stage history
            var product = await _context.Products
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.Stage)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return null;
            }

            var stageDurations = new List<StageDurationDto>();

            // Sort the stages by their start date
            var orderedStages = product.ProductStageHistory
                .OrderBy(psh => psh.StartOfStage)
                .ToList();

            for (int i = 0; i < orderedStages.Count; i++)
            {
                var stageHistory = orderedStages[i];
                var stageName = stageHistory.Stage.Name;
                DateTime endDate = i < orderedStages.Count - 1 ? orderedStages[i + 1].StartOfStage : today;

                var duration = (endDate - stageHistory.StartOfStage).Days;
                if (duration < 0)
                {
                    duration = 0;
                }

                stageDurations.Add(new StageDurationDto
                {
                    StageName = stageName,
                    DaysInStage = duration
                });
            }

            return stageDurations;
        }

        public async Task<List<ProjectStatistics>> GetDetailedProductStatisticsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Bom)
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.Stage)
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.User)
                .Include(p => p.Bom.BomMaterials)
                    .ThenInclude(bm => bm.Material)
                .ToListAsync();

            var projectStatisticsList = new List<ProjectStatistics>();

            foreach (var product in products)
            {
                var productStats = new ProjectStatistics
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    EstimatedHeight = product.EstimatedHeight,
                    EstimatedWeight = product.EstimatedWeight,
                    EstimatedWidth = product.EstimatedWidth,
                    BOMName = product.Bom.Name,
                    CurrentStage = GetCurrentStage(product),
                    TotalMaterialQuantity = GetTotalMaterialQuantity(product),
                    BOMMaterials = product.Bom.BomMaterials.Select(bm => new BOMMaterialStatistics
                    {
                        MaterialNumber = bm.MaterialNumber,
                        MaterialDescription = bm.Material.MaterialDescription,
                        Qty = bm.Qty,
                        UnitMeasureCode = bm.UnitMeasureCode
                    }).ToList(),
                    StageHistory = product.ProductStageHistory.Select(psh => new StageHistoryStatistics
                    {
                        StageName = psh.Stage.Name,
                        StartOfStage = psh.StartOfStage,
                        UserName = psh.User.Name,
                        Duration = GetStageDurationInDays(psh)
                    }).ToList()
                };

                projectStatisticsList.Add(productStats);
            }

            return projectStatisticsList;
        }

        public async Task<ProjectStatistics?> GetDetailedProductStatisticsByProjectIdAsync(int projectId)
        {
            // Example: Fetch the project data from the database
            var product = await _context.Products
                .Include(p => p.Bom)
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.Stage)
                .Include(p => p.ProductStageHistory)
                    .ThenInclude(psh => psh.User)
                .Include(p => p.Bom.BomMaterials)
                    .ThenInclude(bm => bm.Material)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (product == null)
            {
                return null; // Or throw an exception if needed
            }

            // Map the data to the ProjectStatistics model
            var productStatistics = new ProjectStatistics
            {
                ProductName = product.Name,
                ProductDescription = product.Description,
                EstimatedHeight = product.EstimatedHeight,
                EstimatedWeight = product.EstimatedWeight,
                EstimatedWidth = product.EstimatedWidth,
                BOMName = product.Bom.Name,
                BOMMaterials = product.Bom.BomMaterials.Select(bm => new BOMMaterialStatistics
                {
                    MaterialNumber = bm.MaterialNumber,
                    MaterialDescription = bm.Material.MaterialDescription,
                    Qty = bm.Qty,
                    UnitMeasureCode = bm.UnitMeasureCode
                }).ToList(),
                StageHistory = product.ProductStageHistory.Select(psh => new StageHistoryStatistics
                {
                    StageName = psh.Stage.Name,
                    StartOfStage = psh.StartOfStage,
                    UserName = psh.User.Name,
                    Duration = GetStageDurationInDays(psh)
                }).ToList(),
                CurrentStage = GetCurrentStage(product),
                TotalMaterialQuantity = GetTotalMaterialQuantity(product),
            };

            return productStatistics;
        }


        // Helper Method to Get Current Stage
        private string GetCurrentStage(Product product)
        {
            var today = DateTime.Today;

            var currentStage = product.ProductStageHistory
                .Where(psh => psh.StartOfStage <= today)
                .OrderByDescending(psh => psh.StartOfStage)
                .FirstOrDefault();

            return currentStage?.Stage.Name ?? "No stage found";
        }

        // Helper Method to Get Total Material Quantity
        private int GetTotalMaterialQuantity(Product product)
        {
            return product.Bom.BomMaterials.Sum(bm => (int)bm.Qty);
        }

        // Helper Method to Get Stage Duration in Days
        private int GetStageDurationInDays(ProductStageHistory stageHistory)
        {
            var today = DateTime.Today;
            var endDate = stageHistory.StartOfStage;

            // Check if the stage is in the future, if so, set duration to 0
            if (endDate > today)
            {
                return 0;
            }

            return (today - endDate).Days;
        }
    }
}
