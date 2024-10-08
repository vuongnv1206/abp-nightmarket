﻿using NightMarket.Admin.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NightMarket.Admin.Catalogs.Manufacturers
{
    public interface IManufacturerAppService :
        ICrudAppService<ManufacturerDto,
            Guid,
            PagedResultRequestDto,
            CreateUpdateManufacturerDto>
    {
        Task<PagedResultDto<ManufacturerInListDto>> GetListWithFilterAsync(BaseListFilterDto input);


        Task<List<ManufacturerInListDto>> GetListAllAsync();

        Task DeleteMultipleAsync(IEnumerable<Guid> ids);
    }
}
