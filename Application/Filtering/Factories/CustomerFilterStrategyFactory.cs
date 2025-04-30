using Application.Dtos;
using Application.Filtering.Interfaces;
using Application.Filtering.Strategies;
using Domain.Entities;

namespace Application.Filtering.Factories
{
    public class CustomerFilterStrategyFactory : IFilterStrategyFactory<Customer, CustomerFilterDto>
    {
        public List<IFilterStrategy<Customer>> CreateStrategies(CustomerFilterDto filterDto)
        {
            var filters = new List<IFilterStrategy<Customer>>();
            var actions = new List<(Func<bool> condition, Func<IFilterStrategy<Customer>> strategy)>
            {
                (() => !string.IsNullOrEmpty(filterDto.Name),() => new ContainsFilterStrategy < Customer >(p => p.Name, filterDto.Name !)),
                (() => !string.IsNullOrEmpty(filterDto.Email),() => new ContainsFilterStrategy < Customer >(p => p.Email, filterDto.Email !)),
            };

            foreach (var (condition, strategy) in actions)
            {
                if (condition()) filters.Add(strategy());
            }

            return filters;
        }
    }
}
