using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;
using WikiDK.Repositories;

namespace WikiDK.Services
{
    public class RankService
    {
        private readonly AppDbContext _context;

        public RankService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rank>> GetAllAsync()
        {
            return await _context.Ranks.ToListAsync();
        }

        public async Task<Rank?> GetByIdAsync(int id)
        {
            return await _context.Ranks.FindAsync(id);
        }

        public async Task<Rank> CreateAsync(Rank rank)
        {
            _context.Ranks.Add(rank);
            await _context.SaveChangesAsync();
            return rank;
        }

        public async Task<Rank?> UpdateAsync(int id, Rank updatedRank)
        {
            var existing = await _context.Ranks.FindAsync(id);
            if (existing == null) return null;

            existing.Name = updatedRank.Name;
            existing.Description = updatedRank.Description;
            existing.Icon = updatedRank.Icon;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rank = await _context.Ranks.FindAsync(id);
            if (rank == null) return false;

            _context.Ranks.Remove(rank);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RankExistsAsync(int id)
        {
            return await _context.Ranks.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> AddRankToUser(int userId, int rankId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            var rankExists = await _context.Ranks.AnyAsync(r => r.Id == rankId);
            if (!rankExists) return false;

            if (!user.Ranks.Contains(rankId))
            {
                user.Ranks = user.Ranks.Append(rankId).ToArray();
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
