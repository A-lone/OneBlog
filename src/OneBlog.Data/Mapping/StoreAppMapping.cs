using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OneBlog.Data.Mapping
{

    public class StoreAppMapping : EntityMapping<StoreApp>
    {

        public override void Execute(EntityTypeBuilder<StoreApp> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.HasOne(x => x.Categories).WithMany(x => x.StoreApp);
        }
    }


}
