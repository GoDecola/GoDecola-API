using System.Linq.Expressions;

namespace GoDecola.API.Utils
{
    public static class DynamicFilterHelper
    {
        public static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, object filter)
        {
            var filterProperties = filter.GetType().GetProperties();

            foreach (var prop in filterProperties)
            {
                var value = prop.GetValue(filter);
                if (value == null) continue;

                var parameter = Expression.Parameter(typeof(T), "x");

                // Suporte a propriedades aninhadas: "AccommodationDetails.Address.City"
                var propertyNames = prop.Name.Split('.');
                Expression propertyAccess = parameter;

                foreach (var name in propertyNames)
                {
                    propertyAccess = Expression.PropertyOrField(propertyAccess, name);
                }

                var constant = Expression.Constant(value);

                Expression valueExpression = constant;

                if (propertyAccess.Type != constant.Type)
                {
                    valueExpression = Expression.Convert(constant, propertyAccess.Type);
                }

                Expression comparison;

                if (propertyAccess.Type == typeof(string))
                {
                    comparison = Expression.Call(propertyAccess, nameof(string.Contains), Type.EmptyTypes, valueExpression);
                }
                else
                {
                    comparison = Expression.Equal(propertyAccess, valueExpression);
                }

                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                query = query.Where(lambda);
            }

            return query;
        }
    }
}
