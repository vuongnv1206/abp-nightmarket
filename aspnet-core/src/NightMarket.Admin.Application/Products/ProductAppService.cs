using NightMarket.Admin.Commons;
using NightMarket.ProductCategories;
using NightMarket.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
        {
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

        public async Task<PagedResultDto<ProductInListDto>> GetListWithFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), x => x.Name.Contains(input.KeyWord));

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
