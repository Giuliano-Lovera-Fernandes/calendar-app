using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings
{
    public class EventMapping : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Description)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(255);
            
            builder.Property(x => x.StartDate)
                .IsRequired(true);
            
            builder.Property(x => x.EndDate)
                .IsRequired(true);

            builder.Property(x => x.UserId)
                .IsRequired(true)
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);

            builder.Property(x => x.IsActive)
                .IsRequired(true)
                .HasColumnType("BIT");
        }
    }
}
