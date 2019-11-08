using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Paye.Models;

namespace Paye.Models
{
    public class PayeDBEntities : IdentityDbContext
    {      
        public PayeDBEntities()
            : base("name=PayeDBEntities")
        {
            //Database.SetInitializer<PayeDBEntities>(null);
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public static PayeDBEntities Create()
        {
            return new PayeDBEntities();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    throw new UnintentionalCodeFirstException();
        //}

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ReportPost> ReportPosts { get; set; }
        public DbSet<FeedbackSuggestion> FeedbackSuggestions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TrustVote> TrustVotes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sms> Sms { get; set; }

    }
}
