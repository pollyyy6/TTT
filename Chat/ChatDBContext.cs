using Chat.DataClasses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chat
{
    public class ChatDBContext : DbContext
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<FMessage> ForwardedMessages { get; set; }
        public DbSet<GroupChat> Chats { get; set; }
        public DbSet<Chats_Participants> Chats_Participants { get; set; } 
        public ChatDBContext
            (DbContextOptions<ChatDBContext> options)
            : base(options)
        {
			
        }

        public ChatDBContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>().HasMany<FMessage>().WithOne().OnDelete(DeleteBehavior.NoAction);
        }

		public String ConnectionString = "";
		public IConfigurationRoot Configuration { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			String ConnStr = GlobalStrings.GetConnectionStringName();
			if (Configuration == null)
			{
				Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
			}
			else
			{
				ConnStr = "TTT_Test";
			}

			String connectionString = Configuration.GetConnectionString(ConnStr);
			optionsBuilder.UseSqlServer(connectionString);
		}

		public ChatDBContext CreateTestContext()
		{
			DirectoryInfo info = new DirectoryInfo(Directory.GetCurrentDirectory());
			DirectoryInfo temp = info.Parent.Parent.Parent.Parent;
			String CurDir = Path.Combine(temp.ToString(), "TicTacToe");
			String ConnStr = "TTT_Test";
			Configuration = new ConfigurationBuilder().SetBasePath(CurDir).AddJsonFile("appsettings.json").Build();
			DbContextOptionsBuilder builder = new DbContextOptionsBuilder<ChatDBContext>();
			String connectionString = Configuration.GetConnectionString(ConnStr);
			this.ConnectionString = connectionString;
			builder.UseSqlServer(connectionString);
			return this;
		}
	}
}
