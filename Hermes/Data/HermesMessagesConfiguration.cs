using Hermes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hermes.Data
{
    public class HermesMessagesConfiguration : IEntityTypeConfiguration<HermesMessage>
    {

        public HermesMessagesConfiguration()
        {
        }
        public void Configure(EntityTypeBuilder<HermesMessage> builder)
        {
            builder.ToTable("HermesMessages");

        }
    }
}
