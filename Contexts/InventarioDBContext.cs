using InventarioAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    public class InventarioDBContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoEmpaque> TipoEmpaques { get; set; }

        public InventarioDBContext(DbContextOptions<InventarioDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("Categorias")
                .HasKey(key => key.CodigoCategoria);
            /*  modelBuilder.Entity<Categoria>().ToTable("Categorias")
                  .Property("Descripcion")
                  .IsRequired();*/
            modelBuilder.Entity<TipoEmpaque>().ToTable("TipoEmpaque")
                .HasKey(key => key.CodigoEmpaque);
            base.OnModelCreating(modelBuilder);
        }
    }
}