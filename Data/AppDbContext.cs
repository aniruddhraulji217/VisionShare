using Microsoft.EntityFrameworkCore;
using VisionShare.Models;

namespace VisionShare.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<VisionShare.Models.Idea> Ideas { get; set; }
        public DbSet<VisionShare.Models.Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Idea>().HasData(
                new Idea
                {
                    IdeaId = 1,
                    Title = "First Idea",
                    Description = "This is the description of the first idea.",
                    Author = "John Doe",
                    UserId = "user1",
                    FeatureImagePath = "/images/idea1.png",
                    DatePosted = new DateTime(2025, 11, 9)

                },
                new Idea
                {
                    IdeaId = 2,
                    Title = "Second Idea",
                    Description = "This is the description of the second idea.",
                    Author = "Jane Smith",
                    UserId = "user2",
                    FeatureImagePath = "/images/idea2.png",
                    DatePosted = new DateTime(2025, 11, 10)
                }


                );
        }
    }
}
