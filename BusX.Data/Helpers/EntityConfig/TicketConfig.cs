using BusX.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusX.Data.Helpers.EntityConfig
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public virtual void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(p => p.Pnr).HasMaxLength(50).IsRequired();
        }
    }
}
