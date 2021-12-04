using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vraze.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Facilitator> Facilitators { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Hint> Hints { get; set; }
        public DbSet<ChallengeHistory> ChallengeHistories { get; set; }

    }
}
