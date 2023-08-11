using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BDO.Examen.Datos.Mapping;
using BDO.Examen.Entidades;

namespace BDO.Examen.Datos
{
    public class DbContextExamen : DbContext
    {
        public DbSet<Firmadigital> Firmadigital { get; set; }


        public DbContextExamen(DbContextOptions<DbContextExamen> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new FirmadigitalMaps());
        }
    }
}
