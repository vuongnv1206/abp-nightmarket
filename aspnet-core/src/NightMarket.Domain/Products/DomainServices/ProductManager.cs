using JetBrains.Annotations;
using NightMarket.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace NightMarket.Products.DomainServices
{
	public class ProductManager : DomainService
	{
        private readonly IRepository<Product,Guid> _productRepository;
        private readonly IRepository<ProductCategory,Guid> _productCategoryRepository;
        public ProductManager(IRepository<Product,Guid> productRepository,
			IRepository<ProductCategory, Guid> productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Guid manufacturerId,
			string name, string code, string slug,
			ProductType productType, string sKU, int? sortOrder,
			bool visibility, bool isActive, Guid categoryId,
			string? seoMetaDescription, string? description,
			string? thumbnailPicture, double sellPrice)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

			if (await _productRepository.AnyAsync(x => x.Name == name))
            {
                throw new UserFriendlyException($"There is already an product with the same name: {name}", NightMarketDomainErrorCodes.ProductNameAlreadyExists).WithData("name", name);
			}

			if (await _productRepository.AnyAsync(x => x.Code == code))
			{
				throw new UserFriendlyException($"There is already an product with the same code: {code}",NightMarketDomainErrorCodes.ProductCodeAlreadyExists).WithData("code", code);
			}

			if (await _productRepository.AnyAsync(x => x.SKU == sKU))
			{
				throw new UserFriendlyException($"There is already an product with the same sku: {sKU}",NightMarketDomainErrorCodes.ProductSKUAlreadyExists).WithData("sku", sKU);
			}

			var category = await _productCategoryRepository.GetAsync(categoryId);

            return new Product(
                GuidGenerator.Create(),
                manufacturerId,
                name,
                code,
                slug,
                productType,
                sKU,
                sortOrder,
                visibility,
                isActive,
                categoryId,
                seoMetaDescription,
                description,
                thumbnailPicture,
                sellPrice,
                category.Name,
                category.Slug
                );
           
        }

		public async Task ChangeNameAsync([NotNull] Product product, [NotNull] string newName)
		{
			Check.NotNull(product, nameof(product));
			Check.NotNullOrWhiteSpace(newName, nameof(newName));

			if (await _productRepository.AnyAsync(x => x.Name == newName))
			{
				throw new UserFriendlyException($"There is already an product with the same name: {newName}",NightMarketDomainErrorCodes.ProductNameAlreadyExists).WithData("name", newName);
			}

			product.ChangeName(newName);
		}
	}
}
