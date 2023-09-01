﻿using NightMarket.Catalogs.ProductAttributes;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NightMarket.Admin.Catalogs.Products.Attributes
{
    public class ProductAttributeValueDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid AttributeId { get; set; }
        public string Code { get; set; }
        public AttributeType DataType { get; set; }
        public string Label { get; set; }
        public string? Note { get; set; }
        //Value
        public DateTime? DateTimeValue { get; set; }
        public decimal? DecimalValue { get; set; }
        public int? IntValue { get; set; }
        public string? VarcharValue { get; set; }
        public string? TextValue { get; set; }
        //Id
        public Guid? DateTimeId { get; set; }
        public Guid? DecimalId { get; set; }
        public Guid? IntId { get; set; }
        public Guid? TextId { get; set; }
        public Guid? VarcharId { get; set; }

    }
}
