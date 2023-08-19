using NightMarket.Admin.Commons;
using NightMarket.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NightMarket.Admin.ProductCategories
{
    public class ProductCategoryAppService :
        CrudAppService<
            ProductCategory,
            ProductCategoryDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductCategoryDto>,
        IProductCategoryAppService
    {
        //private readonly IRepository<ProductCategory> _repository;
        public ProductCategoryAppService(IRepository<ProductCategory, Guid> repository) : base(repository)
        {
            //_repository = repository;
        }

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
           await Repository.DeleteManyAsync(ids);
           await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        public async Task<List<ProductCategoryInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();

            query = query.Where(x => x.IsActive == true);

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data);

        }

        public async Task<PagedResultDto<ProductCategoryInListDto>> GetListWithFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), x => x.Name.Contains(input.KeyWord));
            
            var totalCount = await AsyncExecuter.LongCountAsync(query);

            var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductCategoryInListDto>()
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ProductCategory>,List<ProductCategoryInListDto>>(data)
            };
            
        }
    }
}
