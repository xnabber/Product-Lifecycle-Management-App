using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class BomService : IBomService
    {
        private readonly PLMContext _context;
        public BomService(PLMContext context)
        {
            _context = context;
        }
        public async Task<Bom> AddBomAsync(string name)
        {
            var bom = new Bom
            {
                Name = name
            };
            _context.Boms.Add(bom);
            await _context.SaveChangesAsync();

            return bom;
        }

        public async Task<bool> DeleteAsync(int bomId)
        {
            var bom = await _context.Boms.FindAsync(bomId);
            if (bom == null) return false;
            _context.Boms.Remove(bom);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Bom>> GetAllAsync()
        {
            var boms = await _context.Boms
                .Include(x => x.BomMaterials)
                .ThenInclude(y => y.Material)
                .ToListAsync();
            return boms;
        }
        public async Task<Bom?> GetByIdAsync(int bomId)
        {
            return await _context.Boms
                .Include(x => x.BomMaterials)
                .ThenInclude(y => y.Material)
                .FirstOrDefaultAsync(z => z.Id == bomId);
        }
        public async Task<bool> UpdateBomAsync(Bom bom)
        {
            var oldBom = await _context.Boms.FindAsync(bom.Id);
            if (oldBom == null) return false;
            oldBom.Name = bom.Name;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
