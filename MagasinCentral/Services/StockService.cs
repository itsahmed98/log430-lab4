using MagasinCentral.Data;
using MagasinCentral.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagasinCentral.Services
{
    public class StockService : IStockService
    {
        private readonly MagasinDbContext _contexte;

        public StockService(MagasinDbContext contexte)
        {
            _contexte = contexte ?? throw new ArgumentNullException(nameof(contexte));
        }

        /// <inheritdoc />
        public async Task<int?> GetStockByMagasinId(int magasinId)
        {
            var magasin = await _contexte.Magasins
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MagasinId == magasinId);

            if (magasin == null)
                return null;

            return await _contexte.MagasinStocksProduits
                .AsNoTracking()
                .Where(msp => msp.MagasinId == magasinId)
                .SumAsync(msp => msp.Quantite);
        }
    }
}