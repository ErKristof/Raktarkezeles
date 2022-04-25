using Microsoft.EntityFrameworkCore;
using Raktarkezeles.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raktarkezeles.API.Data
{
    public class AlkatreszContext : DbContext
    {
        public AlkatreszContext(DbContextOptions<AlkatreszContext> options) : base(options)
        {
        }

        public DbSet<Alkatresz> Alkatreszek { get; set; }
    }
}
