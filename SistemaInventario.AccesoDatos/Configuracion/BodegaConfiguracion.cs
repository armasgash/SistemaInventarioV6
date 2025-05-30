﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class BodegaConfiguracion : IEntityTypeConfiguration<Bodega>
    {
        public void Configure(EntityTypeBuilder<Bodega> builder)
        {
            builder.ToTable("Bodegas");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Estado).IsRequired();
        }
    }
}
