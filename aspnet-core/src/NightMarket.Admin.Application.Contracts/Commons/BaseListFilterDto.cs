using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NightMarket.Admin.Commons
{
    public class BaseListFilterDto : PagedResultRequestDto
    {
        public string? KeyWord { get; set; }
    }
}
