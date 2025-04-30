using System.Linq.Expressions;

namespace Application.Filtering.Interfaces
{
    public interface IFilterStrategy<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
