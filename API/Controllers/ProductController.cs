using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.DTO;
using AutoMapper;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IGenericRepository<Product> _productsRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productsRepository = productsRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            var specification = new ProductsWithTypesAndBrandsSpecification();
            
            var products = await _productsRepository.ListAsync(specification);
        
            return _mapper.Map<IReadOnlyList<Product>, List<ProductToReturnDto>>(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){

            var specification = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepository.GetEntityWithSpecification(specification);

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrandRepository.ListAllAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await _productTypeRepository.ListAllAsync();
            return Ok(productTypes);
        }
    }

}