import type { AttributeType } from '../../night-market/catalogs/product-attributes/attribute-type.enum';
import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateProductAttributeDto {
  code?: string;
  dataType: AttributeType;
  label?: string;
  sortOrder?: number;
  visibility: boolean;
  isActive: boolean;
  isRequired: boolean;
  isUnique: boolean;
  note?: string;
}

export interface ProductAttributeDto extends EntityDto<string> {
  code?: string;
  dataType: AttributeType;
  label?: string;
  sortOrder?: number;
  visibility: boolean;
  isActive: boolean;
  isRequired: boolean;
  isUnique: boolean;
  note?: string;
}

export interface ProductAttributeInListDto extends EntityDto<string> {
  code?: string;
  dataType: AttributeType;
  label?: string;
  sortOrder?: number;
  visibility: boolean;
  isActive: boolean;
  isRequired: boolean;
  isUnique: boolean;
}
