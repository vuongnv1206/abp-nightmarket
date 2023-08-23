import type { BaseListFilterDto } from '../../commons/models';
import type { EntityDto } from '@abp/ng.core';
import type { AttributeType } from '../../night-market/product-attributes/attribute-type.enum';

export interface AddUpdateProductAttributeDto {
  productId?: string;
  attributeId?: string;
  dateTimeValue?: string;
  decimalValue?: number;
  intValue?: number;
  varcharValue?: string;
  textValue?: string;
}

export interface ProductAttributeListFilterDto extends BaseListFilterDto {
  productId?: string;
}

export interface ProductAttributeValueDto extends EntityDto<string> {
  productId?: string;
  attributeId?: string;
  code?: string;
  dataType: AttributeType;
  label?: string;
  note?: string;
  dateTimeValue?: string;
  decimalValue?: number;
  intValue?: number;
  varcharValue?: string;
  textValue?: string;
  dateTimeId?: string;
  decimalId?: string;
  intId?: string;
  textId?: string;
  varcharId?: string;
}
