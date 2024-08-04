using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistance.Configurations
{
    public sealed class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)   // bu metot IEntityTypeConfiguration'dan implemente edilir.
        {
            builder.ToTable("Cars");    // veritabanında ki tablonun adını konfigüre edebiliriz.
            builder.HasKey(p => p.Id);  // o tablodaki Id'nin primary key oldugunu soylüyoruz.
        }        
    }
}
