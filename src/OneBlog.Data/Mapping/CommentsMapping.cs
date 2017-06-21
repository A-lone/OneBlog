using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.Data.Mapping
{

    public class CommentsMapping : EntityMapping<Comments>
    {

        public override void Execute(EntityTypeBuilder<Comments> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.HasOne(x => x.Posts).WithMany(x => x.Comments);
            builder.HasOne(x => x.Author).WithMany(x => x.Comments);
        }
    }


}
