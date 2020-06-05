using System;
using System.Collections.Generic;
using System.Text;
using LMSLexicon20.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMSLexicon20.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
    }
}
