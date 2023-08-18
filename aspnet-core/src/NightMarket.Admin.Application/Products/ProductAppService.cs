﻿using NightMarket.Admin.Commons;
using NightMarket.ProductCategories;
using NightMarket.Products;
using NightMarket.Products.DomainServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace NightMarket.Admin.Products
{
    public class ProductAppService :
        CrudAppService<
            Product,
            ProductDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductDto>,
        IProductAppService
    {

        private readonly ProductManager _productManager;
        private readonly IRepository<ProductCategory,Guid> _categoryRepository;
        public ProductAppService(IRepository<Product, Guid> repository,
            ProductManager productManager,
			IRepository<ProductCategory, Guid> categoryRepository) : base(repository)
        {
            _productManager = productManager;
            _categoryRepository = categoryRepository;
        }

		public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
		{
            var product = await _productManager.CreateAsync(input.ManufacturerId, input.Name, input.Code,
                input.Slug, input.ProductType, input.SKU, input.SortOrder, input.Visibility, input.IsActive, input.CategoryId,
                input.SeoMetaDescription, input.Description, input.ThumbnailPicture, input.SellPrice
                );

            var result = await Repository.InsertAsync(product );
            return ObjectMapper.Map<Product, ProductDto>( result );
		}

		public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
		{
			var product = await Repository.GetAsync( id );
            if ( product == null )
			{
                throw new UserFriendlyException($"There product is not exist with this id: { id }",NightMarketDomainErrorCodes.ProductIsNotExists).WithData("id",id);
            }
            product.ManufacturerId = input.ManufacturerId;
            if ( product.Name != input.Name)
            {
                await _productManager.ChangeNameAsync(product,input.Name);
            }
            product.Code = input.Code;
            product.Slug = input.Slug;
            product.ProductType = input.ProductType;
            product.SKU = input.SKU;
            product.SortOrder = input.SortOrder;
            product.Visibility = input.Visibility;
            product.IsActive = input.IsActive;
            if(product.CategoryId != input.CategoryId)  
            {
				product.CategoryId = input.CategoryId;
                var category = await _categoryRepository.GetAsync( input.CategoryId );
                product.CategoryName = category.Name;
                product.CategorySlug = category.Slug;
			}
            product.SeoMetaDescription = input.SeoMetaDescription;
            product.Description = input.Description;
            product.ThumbnailPicture = input.ThumbnailPicture;
            product.SellPrice = input.SellPrice;

            await Repository.UpdateAsync( product );
			return ObjectMapper.Map<Product, ProductDto>(product);
		}



		public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        public async Task<List<ProductInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();

            query = query.Where(x => x.IsActive == true);

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data);
        }

        public async Task<PagedResultDto<ProductInListDto>> GetListWithFilterAsync(ProductListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), x => x.Name.Contains(input.KeyWord));
            query = query.WhereIf(input.CategoryId.HasValue, x => x.CategoryId == input.CategoryId);


            var totalCount = await AsyncExecuter.LongCountAsync(query);

            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductInListDto>()
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data)
            };
        }
    }
}
