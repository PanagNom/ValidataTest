using Domain.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.ProductCQRS.Queries.GetProductQuery
{
    public class GetProductQueryHandler
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductQuery query)
        {
            return await _productRepository.GetProductAsync(query.Id);
        }
    }
}
