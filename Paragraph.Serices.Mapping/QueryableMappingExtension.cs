using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;

namespace Paragraph.Services.Mapping
{
    public static class QueryableMappingExtension
    {
        public static IQueryable<TDestination> To<TDestination>(
           this IQueryable source,
           params Expression<Func<TDestination, object>>[] membersToExpand)
        {

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));

            }



            return source.ProjectTo(membersToExpand);

        }

        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            object parameters)
        {

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));

            }



            return source.ProjectTo<TDestination>(parameters);
        }
    }
}
