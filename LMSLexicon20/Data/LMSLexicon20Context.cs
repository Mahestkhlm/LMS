using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LMSLexicon20.Models;

namespace LMSLexicon20.Data
{
    public class LMSLexicon20Context : DbContext
    {
        public LMSLexicon20Context (DbContextOptions<LMSLexicon20Context> options)
            : base(options)
        {
        }

        public DbSet<Course> Course { get; set; }
    }
}
