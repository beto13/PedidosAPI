namespace Application.Filtering.Interfaces
{
    public interface IFilterStrategyFactory<T, TFilterDto>
    {
        List<IFilterStrategy<T>> CreateStrategies(TFilterDto filterDto);
    }
}
