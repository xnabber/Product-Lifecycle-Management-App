using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class BomMaterialService : IBomMaterialService
    {
        private readonly PLMContext _context;
        public BomMaterialService(PLMContext context)
        {
            _context = context;
        }
        public async Task<BomMaterial?> AddBomMaterialAsync(BomMaterialDto bomMaterial)
        {
            var bom = await _context.Boms.FindAsync(bomMaterial.BomId);
            if (bom == null) return null;
            var material = await _context.Materials.FindAsync(bomMaterial.MaterialNumber);
            if (material == null) return null;
            var newBomMaterial = new BomMaterial
            {
                Bom = bom,
                Material = material,
                Qty = bomMaterial.Qty,
                UnitMeasureCode = bomMaterial.UnitMeasureCode,
                MaterialNumber = bomMaterial.MaterialNumber
            };
            _context.BomMaterials.Add(newBomMaterial);
            await _context.SaveChangesAsync();
            return newBomMaterial;
        }
        public async Task<bool> DeleteAsync(int bomId, string materialId)
        {
            var bomMaterial = await _context.BomMaterials.FindAsync(bomId, materialId);
            if (bomMaterial == null) return false;
            _context.BomMaterials.Remove(bomMaterial);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<BomMaterial>> GetAllAsync()
        {
            var bomMaterials = await _context.BomMaterials.Include(b => b.Material).Include(b => b.Bom).ToListAsync();
            return bomMaterials;
        }

        public async Task<IEnumerable<BomMaterial>> GetByBom(int bomId)
        {
            var bomMaterials = await _context.BomMaterials.Include(b => b.Material).Include(b => b.Bom).Where(c => c.BomId == bomId).ToListAsync();
            return bomMaterials;
        }

        public async Task<BomMaterial?> GetByIdAsync(int bomId, string materialId)
        {
            return await _context.BomMaterials.Include(b => b.Material).Include(b => b.Bom).FirstOrDefaultAsync(x => x.BomId == bomId && x.MaterialNumber == materialId);
        }
        public async Task<BomMaterial?> UpdateBomMaterialAsync(BomMaterialDto bomMaterial)
        {
            var oldBomMaterial = await _context.BomMaterials.FindAsync(bomMaterial.BomId, bomMaterial.MaterialNumber);
            if (oldBomMaterial == null) return null;
            var bom = await _context.Boms.FindAsync(bomMaterial.BomId);
            if (bom == null) return null;
            var material = await _context.Materials.FindAsync(bomMaterial.MaterialNumber);
            if (material == null) return null;
            oldBomMaterial.Qty = bomMaterial.Qty;
            oldBomMaterial.UnitMeasureCode = bomMaterial.UnitMeasureCode;
            oldBomMaterial.Bom = bom;
            oldBomMaterial.Material = material;
            await _context.SaveChangesAsync();
            return oldBomMaterial;
        }

    }
}
