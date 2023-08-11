using System;
using System.Collections.Generic;
using System.Text;
using BDO.Examen.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BDO.Examen.Datos.Mapping
{
    public class FirmadigitalMaps : IEntityTypeConfiguration<Firmadigital>
    {
        public void Configure(EntityTypeBuilder<Firmadigital> entity)
        {
            entity.HasKey(e => e.IdFirma);

            entity.ToTable("firmadigital");

            entity.Property(e => e.IdFirma).HasColumnName("id_firma");

            entity.Property(e => e.CertificadoDigital)
                .HasColumnType("text")
                .HasColumnName("certificado_digital");

            entity.Property(e => e.EmpresaAcreditadora)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("empresa_acreditadora");

            entity.Property(e => e.FechaEmision)
                .HasColumnType("datetime")
                .HasColumnName("fecha_emision");

            entity.Property(e => e.FechaVencimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_vencimiento");

            entity.Property(e => e.RazonSocial)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("razon_social");

            entity.Property(e => e.RepresentanteLegal)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("representante_legal");

            entity.Property(e => e.RutaRubrica)
                .HasColumnType("text")
                .HasColumnName("ruta_rubrica");

            entity.Property(e => e.TipoFirma)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("tipo_firma")
                .IsFixedLength(true);

            entity.ToTable("firmadigital");
        }
    }
}
