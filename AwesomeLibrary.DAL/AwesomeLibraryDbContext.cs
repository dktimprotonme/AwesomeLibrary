using AwesomeLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.DAL
{
    public class AwesomeLibraryDbContext : DbContext
    {
        public AwesomeLibraryDbContext(DbContextOptions<AwesomeLibraryDbContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BookTransaction> BookTransactions { get; set; }
    }
}
