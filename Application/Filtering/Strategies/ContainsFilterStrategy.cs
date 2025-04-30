using Application.Filtering.Interfaces;
using System.Linq.Expressions;

namespace Application.Filtering.Strategies
{
    public class ContainsFilterStrategy<T> : IFilterStrategy<T>
    {
        private readonly Expression<Func<T, string>> _propertySelector;
        private readonly string _value;

        public ContainsFilterStrategy(Expression<Func<T, string>> propertySelector, string value)
        {
            _propertySelector = propertySelector;
            _value = value;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var parameter = _propertySelector.Parameters[0];
            var body = Expression.Call(_propertySelector.Body,
                nameof(string.Contains), null, Expression.Constant(_value));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
