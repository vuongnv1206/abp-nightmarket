using Microsoft.AspNetCore.Authorization;
using NightMarket.Admin.Commons;
using NightMarket.Admin.Products.Attributes;
using NightMarket.ProductAttributes;
using NightMarket.ProductCategories;
using NightMarket.Products;
using NightMarket.Products.Containers;
using NightMarket.Products.DomainServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace NightMarket.Admin.Products
{
	
	public class ProductAppService :
		CrudAppService<
			Product,
			ProductDto,
			Guid,
			PagedResultRequestDto,
			CreateUpdateProductDto>,
		IProductAppService

	{

		private readonly ProductManager _productManager;
		private readonly IRepository<ProductCategory, Guid> _categoryRepository;
		private readonly IRepository<ProductAttribute, Guid> _attributeRepository;
		private readonly IRepository<ProductAttributeDateTime> _attributeDateTimeRepository;
		private readonly IRepository<ProductAttributeInt> _attributeIntRepository;
		private readonly IRepository<ProductAttributeDecimal> _attributeDecimalRepository;
		private readonly IRepository<ProductAttributeVarchar> _attributeVarcharRepository;
		private readonly IRepository<ProductAttributeText> _attributeTextRepository;
		//Blob storage
		private readonly IBlobContainer<ProductThumbnailPictureContainer> _blobContainer;

		private readonly ProductCodeGenerator _productCodeGenerator;



		public ProductAppService(IRepository<Product, Guid> repository,
			ProductManager productManager,
			IRepository<ProductCategory, Guid> categoryRepository,
			IRepository<ProductAttribute, Guid> attributeRepository,
			IBlobContainer<ProductThumbnailPictureContainer> blobContainer,
			ProductCodeGenerator productCodeGenerator,
			 IRepository<ProductAttributeDateTime> attributeDateTimeRepository,
			  IRepository<ProductAttributeInt> attributeIntRepository,
			  IRepository<ProductAttributeDecimal> attributeDecimalRepository,
			  IRepository<ProductAttributeVarchar> attributeVarcharRepository,
			  IRepository<ProductAttributeText> attributeTextRepository) : base(repository)
		{
			_productManager = productManager;
			_categoryRepository = categoryRepository;
			_attributeRepository = attributeRepository;
			_blobContainer = blobContainer;
			_productCodeGenerator = productCodeGenerator;
			_attributeDateTimeRepository = attributeDateTimeRepository;
			_attributeIntRepository = attributeIntRepository;
			_attributeDecimalRepository = attributeDecimalRepository;
			_attributeVarcharRepository = attributeVarcharRepository;
			_attributeTextRepository = attributeTextRepository;
		}

		public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
		{
			var product = await _productManager.CreateAsync(input.ManufacturerId,
				input.Name, input.Code,
				input.Slug, input.ProductType,
				input.SKU, input.SortOrder,
				input.Visibility, input.IsActive,
				input.CategoryId,
				input.SeoMetaDescription,
				input.Description, input.SellPrice
				);
			if (input.ThumbnailPictureContent != null && input.ThumbnailPictureContent.Length > 0)
			{
				await SaveImagesAsync(input.ThumbnailPictureName, input.ThumbnailPictureContent);
				product.ThumbnailPicture = input.ThumbnailPictureName;
			}

			var result = await Repository.InsertAsync(product);
			return ObjectMapper.Map<Product, ProductDto>(result);
		}

		public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
		{
			var product = await Repository.GetAsync(id);
			if (product == null)
			{
				throw new UserFriendlyException($"There product is not exist with this id: {id}", NightMarketDomainErrorCodes.ProductIsNotExists).WithData("id", id);
			}
			product.ManufacturerId = input.ManufacturerId;
			if (product.Name != input.Name)
			{
				await _productManager.ChangeNameAsync(product, input.Name);
			}
			product.Code = input.Code;
			product.Slug = input.Slug;
			product.ProductType = input.ProductType;
			product.SKU = input.SKU;
			product.SortOrder = input.SortOrder;
			product.Visibility = input.Visibility;
			product.IsActive = input.IsActive;
			if (product.CategoryId != input.CategoryId)
			{
				product.CategoryId = input.CategoryId;
				var category = await _categoryRepository.GetAsync(input.CategoryId);  
				product.CategoryName = category.Name;
				product.CategorySlug = category.Slug;
			}
			product.SeoMetaDescription = input.SeoMetaDescription;
			product.Description = input.Description;
			if (input.ThumbnailPictureContent != null && input.ThumbnailPictureContent.Length > 0)
			{
				await SaveImagesAsync(input.ThumbnailPictureName, input.ThumbnailPictureContent);
				product.ThumbnailPicture = input.ThumbnailPictureName;
			}
			product.SellPrice = input.SellPrice;

			await Repository.UpdateAsync(product);
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

			var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime)
				.Skip(input.SkipCount)
				.Take(input.MaxResultCount));

			foreach (var productDto in data)
			{
				productDto.ThumbnailPicture = await GetThumbnailImageAsync(productDto.ThumbnailPicture);
			}

			return new PagedResultDto<ProductInListDto>()
			{
				TotalCount = totalCount,
				Items = ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data)
			};
		}

		private async Task SaveImagesAsync(string fileName, string base64)
		{
			Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
			base64 = regex.Replace(base64, string.Empty);
			byte[] bytes = Convert.FromBase64String(base64);

			await _blobContainer.SaveAsync(fileName, bytes, overrideExisting: true);
		}

		public async Task<string> GetThumbnailImageAsync(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return null;
			}
			var thumbnailContent = await _blobContainer.GetAllBytesOrNullAsync(fileName);
			if (thumbnailContent is null)
			{
				return null;
			}
			var result = Convert.ToBase64String(thumbnailContent);
			return result;
		}

		public async Task<string> GetSuggestNewCodeAsync()
		{
			return await _productCodeGenerator.GenerateAsync();
		}


		public async Task<ProductAttributeValueDto> AddProductAttributeAsync(AddUpdateProductAttributeDto input)
		{
			var product = await Repository.GetAsync(input.ProductId);
			if (product is null)
			{
				throw new UserFriendlyException($"There product is not exist with this id: {input.ProductId}", NightMarketDomainErrorCodes.ProductIsNotExists).WithData("id", input.ProductId);
			}
			var attribute = await _attributeRepository.GetAsync(input.AttributeId);
			if (attribute is null)
			{
				throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
			}
			var newAttributeId = GuidGenerator.Create();
			switch (attribute.DataType)
			{
				case AttributeType.Date:
					if (input.DateTimeValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);
					}
					var productAttributeDateTime = new ProductAttributeDateTime(newAttributeId, input.AttributeId, input.ProductId, input.DateTimeValue);
					await _attributeDateTimeRepository.InsertAsync(productAttributeDateTime);
					break;
				case AttributeType.Int:
					if (input.IntValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeInt = new ProductAttributeInt(newAttributeId, input.AttributeId, input.ProductId, input.IntValue.Value);
					await _attributeIntRepository.InsertAsync(productAttributeInt);
					break;
				case AttributeType.Decimal:
					if (input.DecimalValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeDecimal = new ProductAttributeDecimal(newAttributeId, input.AttributeId, input.ProductId, input.DecimalValue.Value);
					await _attributeDecimalRepository.InsertAsync(productAttributeDecimal);
					break;
				case AttributeType.Varchar:
					if (input.VarcharValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeVarchar = new ProductAttributeVarchar(newAttributeId, input.AttributeId, input.ProductId, input.VarcharValue);
					await _attributeVarcharRepository.InsertAsync(productAttributeVarchar);
					break;
				case AttributeType.Text:
					if (input.TextValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeText = new ProductAttributeText(newAttributeId, input.AttributeId, input.ProductId, input.TextValue);
					await _attributeTextRepository.InsertAsync(productAttributeText);
					break;
			}
			await UnitOfWorkManager.Current.SaveChangesAsync();
			return new ProductAttributeValueDto()
			{
				AttributeId = input.AttributeId,
				Code = attribute.Code,
				DataType = attribute.DataType,
				DateTimeValue = input.DateTimeValue,
				DecimalValue = input.DecimalValue,
				Id = newAttributeId,
				IntValue = input.IntValue,
				Label = attribute.Label,
				Note = attribute.Note,
				ProductId = input.ProductId,
				TextValue = input.TextValue
			};
		}



		public async Task<ProductAttributeValueDto> UpdateProductAttributeAsync(Guid id, AddUpdateProductAttributeDto input)
		{
			var product = await Repository.GetAsync(input.ProductId);
			if (product is null)
			{
				throw new UserFriendlyException($"There product is not exist with this id: {input.ProductId}", NightMarketDomainErrorCodes.ProductIsNotExists).WithData("id", input.ProductId);
			}
			var attribute = await _attributeRepository.GetAsync(input.AttributeId);
			if (attribute is null)
			{
				throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
			}

			switch (attribute.DataType)
			{
				case AttributeType.Date:
					if (input.DateTimeValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);
					}
					var productAttributeDateTime = await _attributeDateTimeRepository.GetAsync(x => x.Id == id);
					if (productAttributeDateTime == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
					}
					productAttributeDateTime.Value = input.DateTimeValue.Value;
					await _attributeDateTimeRepository.UpdateAsync(productAttributeDateTime);
					break;
				case AttributeType.Int:
					if (input.IntValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeInt = await _attributeIntRepository.GetAsync(x => x.Id == id);
					if (productAttributeInt == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
					}
					productAttributeInt.Value = input.IntValue.Value;
					await _attributeIntRepository.UpdateAsync(productAttributeInt);
					break;
				case AttributeType.Decimal:
					if (input.DecimalValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeDecimal = await _attributeDecimalRepository.GetAsync(x => x.Id == id);
					if (productAttributeDecimal == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
					}
					productAttributeDecimal.Value = input.DecimalValue.Value;
					await _attributeDecimalRepository.UpdateAsync(productAttributeDecimal);
					break;
				case AttributeType.Varchar:
					if (input.VarcharValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeVarchar = await _attributeVarcharRepository.GetAsync(x => x.Id == id);
					if (productAttributeVarchar == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
					}
					productAttributeVarchar.Value = input.VarcharValue;
					await _attributeVarcharRepository.UpdateAsync(productAttributeVarchar);
					break;
				case AttributeType.Text:
					if (input.TextValue == null)
					{
						throw new UserFriendlyException($"There attribute value is not valid !", NightMarketDomainErrorCodes.ProductAttributeValueIsNotValid);

					}
					var productAttributeText = await _attributeTextRepository.GetAsync(x => x.Id == id);
					if (productAttributeText == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {input.AttributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", input.AttributeId);
					}
					productAttributeText.Value = input.TextValue;
					await _attributeTextRepository.UpdateAsync(productAttributeText);
					break;
			}
			await UnitOfWorkManager.Current.SaveChangesAsync();
			return new ProductAttributeValueDto()
			{
				AttributeId = input.AttributeId,
				Code = attribute.Code,
				DataType = attribute.DataType,
				DateTimeValue = input.DateTimeValue,
				DecimalValue = input.DecimalValue,
				Id = id,
				IntValue = input.IntValue,
				Label = attribute.Label,
				Note = attribute.Note,
				ProductId = input.ProductId,
				TextValue = input.TextValue
			};
		}

		public async Task RemoveProductAttributeAsync(Guid attributeId,Guid id)
		{
			var attribute = await _attributeRepository.GetAsync(attributeId);
			if (attribute is null)
			{
				throw new UserFriendlyException($"There attribute is not exist with this id: {attributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", attributeId);
			}
			switch (attribute.DataType)
			{
				case AttributeType.Date:
					
					var productAttributeDateTime = await _attributeDateTimeRepository.GetAsync(x => x.Id == id);
					if (productAttributeDateTime == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {attributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", attributeId);
					}

					await _attributeDateTimeRepository.DeleteAsync(productAttributeDateTime);
					break;
				case AttributeType.Int:
					
					var productAttributeInt = await _attributeIntRepository.GetAsync(x => x.Id == id);
					if (productAttributeInt == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {attributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", attributeId);
					}
					await _attributeIntRepository.DeleteAsync(productAttributeInt);
					break;
				case AttributeType.Decimal:
					
					var productAttributeDecimal = await _attributeDecimalRepository.GetAsync(x => x.Id == id);
					if (productAttributeDecimal == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {attributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", attributeId);
					}
					await _attributeDecimalRepository.DeleteAsync(productAttributeDecimal);
					break;
				case AttributeType.Varchar:
					var productAttributeVarchar = await _attributeVarcharRepository.GetAsync(x => x.Id == id);
					if (productAttributeVarchar == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {attributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", attributeId);
					}
					await _attributeVarcharRepository.DeleteAsync(productAttributeVarchar);
					break;
				case AttributeType.Text:
				
					var productAttributeText = await _attributeTextRepository.GetAsync(x => x.Id == id);
					if (productAttributeText == null)
					{
						throw new UserFriendlyException($"There attribute is not exist with this id: {attributeId}", NightMarketDomainErrorCodes.ProductAttributeIsNotExists).WithData("id", attributeId);
					}

					await _attributeTextRepository.DeleteAsync(productAttributeText);
					break;
			}
			await UnitOfWorkManager.Current.SaveChangesAsync();
		}

		public async Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId)
		{
			var attributeQuery = await _attributeRepository.GetQueryableAsync();

			var attributeDateTimeQuery = await _attributeDateTimeRepository.GetQueryableAsync();
			var attributeDecimalQuery = await _attributeDecimalRepository.GetQueryableAsync();
			var attributeIntQuery = await _attributeIntRepository.GetQueryableAsync();
			var attributeVarcharQuery = await _attributeVarcharRepository.GetQueryableAsync();
			var attributeTextQuery = await _attributeTextRepository.GetQueryableAsync();

			var query = from a in attributeQuery
						join adate in attributeDateTimeQuery on a.Id equals adate.AttributeId into aDateTimeTable
						from adate in aDateTimeTable.DefaultIfEmpty()
						join adecimal in attributeDecimalQuery on a.Id equals adecimal.AttributeId into aDecimalTable
						from adecimal in aDecimalTable.DefaultIfEmpty()
						join aint in attributeIntQuery on a.Id equals aint.AttributeId into aIntTable
						from aint in aIntTable.DefaultIfEmpty()
						join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into aVarcharTable
						from aVarchar in aVarcharTable.DefaultIfEmpty()
						join aText in attributeTextQuery on a.Id equals aText.AttributeId into aTextTable
						from aText in aTextTable.DefaultIfEmpty()
						where (adate == null || adate.ProductId == productId)
						&& (adecimal == null || adecimal.ProductId == productId)
						 && (aint == null || aint.ProductId == productId)
						  && (aVarchar == null || aVarchar.ProductId == productId)
						   && (aText == null || aText.ProductId == productId)
						select new ProductAttributeValueDto()
						{
							Label = a.Label,
							AttributeId = a.Id,
							DataType = a.DataType,
							Code = a.Code,
							ProductId = productId,

							DateTimeValue = adate != null ? adate.Value : null,
							DecimalValue = adecimal != null ? adecimal.Value : null,
							IntValue = aint != null ? aint.Value : null,
							TextValue = aText != null ? aText.Value : null,
							VarcharValue = aVarchar != null ? aVarchar.Value : null,

							DateTimeId = adate != null ? adate.Id : null,
							DecimalId = adecimal != null ? adecimal.Id : null,
							IntId = aint != null ? aint.Id : null,
							TextId = aText != null ? aText.Id : null,
							VarcharId = aVarchar != null ? aVarchar.Id : null,
						};
			query = query.Where(x => x.DateTimeId != null
						   || x.DecimalId != null
						   || x.IntValue != null
						   || x.TextId != null
						   || x.VarcharId != null);
			return await AsyncExecuter.ToListAsync(query);
		}

		public async Task<PagedResultDto<ProductAttributeValueDto>> GetListProductAttributeAsync(ProductAttributeListFilterDto input)
		{
			var attributeQuery = await _attributeRepository.GetQueryableAsync();

			var attributeDateTimeQuery = await _attributeDateTimeRepository.GetQueryableAsync();
			var attributeDecimalQuery = await _attributeDecimalRepository.GetQueryableAsync();
			var attributeIntQuery = await _attributeIntRepository.GetQueryableAsync();
			var attributeVarcharQuery = await _attributeVarcharRepository.GetQueryableAsync();
			var attributeTextQuery = await _attributeTextRepository.GetQueryableAsync();

			var query = from a in attributeQuery
						join adate in attributeDateTimeQuery on a.Id equals adate.AttributeId into aDateTimeTable
						from adate in aDateTimeTable.DefaultIfEmpty()
						join adecimal in attributeDecimalQuery on a.Id equals adecimal.AttributeId into aDecimalTable
						from adecimal in aDecimalTable.DefaultIfEmpty()
						join aint in attributeIntQuery on a.Id equals aint.AttributeId into aIntTable
						from aint in aIntTable.DefaultIfEmpty()
						join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into aVarcharTable
						from aVarchar in aVarcharTable.DefaultIfEmpty()
						join aText in attributeTextQuery on a.Id equals aText.AttributeId into aTextTable
						from aText in aTextTable.DefaultIfEmpty()
						where (adate == null || adate.ProductId == input.ProductId)
						&& (adecimal == null || adecimal.ProductId == input.ProductId)
						 && (aint == null || aint.ProductId == input.ProductId)
						  && (aVarchar == null || aVarchar.ProductId == input.ProductId)
						   && (aText == null || aText.ProductId == input.ProductId)
						select new ProductAttributeValueDto()
						{
							Label = a.Label,
							AttributeId = a.Id,
							DataType = a.DataType,
							Code = a.Code,
							ProductId = input.ProductId,

							DateTimeValue = adate != null ? adate.Value : null,
							DecimalValue = adecimal != null ? adecimal.Value : null,
							IntValue = aint != null ? aint.Value : null,
							TextValue = aText != null ? aText.Value : null,
							VarcharValue = aVarchar != null ? aVarchar.Value : null,

							DateTimeId = adate != null ? adate.Id : null,
							DecimalId = adecimal != null ? adecimal.Id : null,
							IntId = aint != null ? aint.Id : null,
							TextId = aText != null ? aText.Id : null,
							VarcharId = aVarchar != null ? aVarchar.Id : null,
						};
			query = query.Where(x => x.DateTimeId != null
			|| x.DecimalId != null
			|| x.IntValue != null
			|| x.TextId != null
			|| x.VarcharId != null);
			var totalCount = await AsyncExecuter.LongCountAsync(query);
			var data = await AsyncExecuter.ToListAsync(
				query.OrderByDescending(x => x.Label)
				.Skip(input.SkipCount)
				.Take(input.MaxResultCount)
				);
			return new PagedResultDto<ProductAttributeValueDto>(totalCount, data);
		}

		
	}
}
