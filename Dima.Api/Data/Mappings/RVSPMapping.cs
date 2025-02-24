using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings
{
    public class RVSPMapping : IEntityTypeConfiguration<RVSP>
    {
        public void Configure(EntityTypeBuilder<RVSP> builder)
        {
            builder.ToTable("RVSP");
            builder.HasKey(x => x.Id); 
            
            builder.Property(x => x.UserId)
                .IsRequired(true)
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);

            builder.Property(x => x.EventResponseStatus)
                .IsRequired(true)
                .HasColumnType("SMALLINT");            

            builder.Property(x => x.EventResponseDate)
                .IsRequired(true);            

        }
    }
}
