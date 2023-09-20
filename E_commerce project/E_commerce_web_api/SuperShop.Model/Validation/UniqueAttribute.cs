using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.Validation
{
    public class UniqueAttribute : ValidationAttribute
    {
        private readonly Type _dbContextType;
        private readonly string _propertyName;

        public UniqueAttribute(Type dbContextType, string propertyName)
        {
            _dbContextType = dbContextType;
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (DbContext)validationContext.GetService(_dbContextType);
            var propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);

            var currentValue = propertyInfo.GetValue(validationContext.ObjectInstance);
            var entityType = validationContext.ObjectType;

            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Array.Empty<Type>());
            var genericSetMethod = setMethod?.MakeGenericMethod(entityType);
            var dbSet = genericSetMethod?.Invoke(dbContext, null);

            var findMethod = dbSet?.GetType().GetMethod(nameof(Enumerable.FirstOrDefault), new[] { typeof(Func<,>).MakeGenericType(entityType, typeof(bool)) });
            var entity = findMethod?.Invoke(dbSet, new object[] { CreatePredicate(entityType, propertyInfo, currentValue) });

            if (entity != null)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private static object CreatePredicate(Type entityType, PropertyInfo propertyInfo, object value)
        {
            var parameter = Expression.Parameter(entityType, "x");
            var property = Expression.Property(parameter, propertyInfo);
            var equal = Expression.Equal(property, Expression.Constant(value, propertyInfo.PropertyType));
            var lambda = Expression.Lambda(equal, parameter);
            return lambda.Compile();
        }
    }
}
