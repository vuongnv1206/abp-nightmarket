import type { CreateUpdateProductCategoryDto, ProductCategoryDto, ProductCategoryInListDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseListFilterDto } from '../../commons/models';

@Injectable({
  providedIn: 'root',
})
export class ProductCategoryService {
  apiName = 'Default';
  

  create = (input: CreateUpdateProductCategoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductCategoryDto>({
      method: 'POST',
      url: '/api/app/product-category',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/product-category/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/product-category/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductCategoryDto>({
      method: 'GET',
      url: `/api/app/product-category/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductCategoryDto>>({
      method: 'GET',
      url: '/api/app/product-category',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductCategoryInListDto[]>({
      method: 'GET',
      url: '/api/app/product-category/all',
    },
    { apiName: this.apiName,...config });
  

  getListWithFilter = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProductCategoryInListDto>>({
      method: 'GET',
      url: '/api/app/product-category/with-filter',
      params: { keyWord: input.keyWord, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateProductCategoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProductCategoryDto>({
      method: 'PUT',
      url: `/api/app/product-category/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
