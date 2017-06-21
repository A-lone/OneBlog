using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.Data
{

    public interface IEntityMapping
    {
        void Execute(ModelBuilder builder);
    }

    public abstract class EntityMapping<T> : IEntityMapping where T : class
    {
        public EntityMapping()
        {

        }

        public void Execute(ModelBuilder builder)
        {
            Execute(builder.Entity<T>());
        }

        public abstract void Execute(EntityTypeBuilder<T> builder);
    }
}
