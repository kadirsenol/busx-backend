using BusX.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusX.Data.Helpers.EntityConfig
{
    public class JourneyConfig : IEntityTypeConfiguration<Journey>
    {
        public virtual void Configure(EntityTypeBuilder<Journey> builder)
        {
            builder.Property(p => p.From).HasMaxLength(100).IsRequired();
        }
    }
}
