using MiniSteam.Entities.MicrosoftIdentity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CostAndProductConsolidator.DataAccess.EntityConfigurations.MicrosoftIdentity
{
    public class UserClamConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable(nameof(UserClaim));
        }
    }
}
