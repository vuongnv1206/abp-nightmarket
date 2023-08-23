import type { AddUpdateProductAttributeDto, ProductAttributeListFilterDto, ProductAttributeValueDto } from './attributes/models';
import type { CreateUpdateProductDto, ProductDto, ProductInListDto, ProductListFilterDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  apiName = 'Default';
  

  addProductAttribute = (input: AddUpdateProductAttributeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductAttributeValueDto>({
      method: 'POST',
      url: '/api/app/product/product-attribute',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateUpdateProductDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductDto>({
      method: 'POST',
      url: '/api/app/product',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/product/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/product/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductDto>({
      method: 'GET',
      url: `/api/app/product/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductDto>>({
      method: 'GET',
      url: '/api/app/product',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductInListDto[]>({
      method: 'GET',
      url: '/api/app/product/all',
    },
    { apiName: this.apiName,...config });
  

  getListProductAttribute = (input: ProductAttributeListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductAttributeValueDto>>({
      method: 'GET',
      url: '/api/app/product/product-attribute',
      params: { productId: input.productId, keyWord: input.keyWord, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListProductAttributeAll = (productId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductAttributeValueDto[]>({
      method: 'GET',
      url: `/api/app/product/product-attribute-all/${productId}`,
    },
    { apiName: this.apiName,...config });
  

  getListWithFilter = (input: ProductListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductInListDto>>({
      method: 'GET',
      url: '/api/app/product/with-filter',
      params: { categoryId: input.categoryId, keyWord: input.keyWord, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getSuggestNewCode = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: '/api/app/product/suggest-new-code',
    },
    { apiName: this.apiName,...config });
  

  getThumbnailImage = (fileName: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: '/api/app/product/thumbnail-image',
      params: { fileName },
    },
    { apiName: this.apiName,...config });
  

  removeProductAttribute = (attributeId: string, id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/product/${id}/product-attribute/${attributeId}`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateProductDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductDto>({
      method: 'PUT',
      url: `/api/app/product/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateProductAttribute = (id: string, input: AddUpdateProductAttributeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductAttributeValueDto>({
      method: 'PUT',
      url: `/api/app/product/${id}/product-attribute`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
