using Microsoft.EntityFrameworkCore;
using PLMApp.Data;
using PLMApp.Models;

namespace PLMProject.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly PLMContext _context;
        public MaterialService(PLMContext context)
        {
            _context = context;
        }
        public async Task<Material> AddMaterialAsync(MaterialDto material)
        {
            var newMaterial = new Material
            {
                MaterialNumber = material.MaterialNumber,
                MaterialDescription = material.MaterialDescription,
                Height = material.Height,
                Weight = material.Weight,
                Width = material.Width
            };
            _context.Materials.Add(newMaterial);
            await _context.SaveChangesAsync();
            return newMaterial;
        }
        public async Task<bool> DeleteAsync(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId);
            if (material == null) return false;
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Material>> GetAllAsync()
        {
            var materials = await _context.Materials.ToListAsync();
            return materials;
        }
        public async Task<Material?> GetByIdAsync(int materialId)
        {
            return await _context.Materials.FindAsync(materialId);
        }
        public async Task<Material?> UpdateMaterialAsync(MaterialDto material)
        {
            var oldMaterial = await _context.Materials.FindAsync(material.MaterialNumber);
            if (oldMaterial == null) return null;

            oldMaterial.MaterialDescription = material.MaterialDescription;
            oldMaterial.Weight = material.Weight;
            oldMaterial.Width = material.Width;
            oldMaterial.Height = material.Height;
            await _context.SaveChangesAsync();
            return oldMaterial;
        }
    }
}
