import type { EntityDto } from '@abp/ng.core';
import type { ProductType } from '../night-market/products/product-type.enum';
import type { BaseListFilterDto } from '../commons/models';

export interface CreateUpdateProductDto extends EntityDto<string> {
  manufacturerId?: string;
  name?: string;
  code?: string;
  slug?: string;
  productType: ProductType;
  sku?: string;
  sortOrder?: number;
  visibility: boolean;
  isActive: boolean;
  categoryId?: string;
  seoMetaDescription?: string;
  description?: string;
  thumbnailPicture?: string;
}

export interface ProductDto extends EntityDto<string> {
  manufacturerId?: string;
  name?: string;
  code?: string;
  slug?: string;
  productType: ProductType;
  sku?: string;
  sortOrder?: number;
  visibility: boolean;
  isActive: boolean;
  categoryId?: string;
  seoMetaDescription?: string;
  description?: string;
  thumbnailPicture?: string;
  sellPrice: number;
}

export interface ProductInListDto extends EntityDto<string> {
  manufacturerId?: string;
  name?: string;
  code?: string;
  slug?: string;
  productType: ProductType;
  sku?: string;
  sortOrder: number;
  visibility: boolean;
  isActive: boolean;
  categoryId?: string;
  description?: string;
  thumbnailPicture?: string;
  sellPrice: number;
}

export interface ProductListFilterDto extends BaseListFilterDto {
  categoryId?: string;
}
