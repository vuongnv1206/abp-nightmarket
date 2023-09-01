import type { CreateUpdateRoleDto, RoleDto, RoleInListDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseListFilterDto } from '../../commons/models';
import type { GetPermissionListResultDto, UpdatePermissionsDto } from '../../volo/abp/permission-management/models';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  apiName = 'Default';
  

  create = (input: CreateUpdateRoleDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, RoleDto>({
      method: 'POST',
      url: '/api/app/role',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/role/${id}`,
    },
    { apiName: this.apiName,...config });
  

  deleteMultiple = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/role/multiple',
      params: { ids },
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, RoleDto>({
      method: 'GET',
      url: `/api/app/role/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<RoleDto>>({
      method: 'GET',
      url: '/api/app/role',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, RoleInListDto[]>({
      method: 'GET',
      url: '/api/app/role/all',
    },
    { apiName: this.apiName,...config });
  

  getListFilter = (input: BaseListFilterDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<RoleInListDto>>({
      method: 'GET',
      url: '/api/app/role/filter',
      params: { keyWord: input.keyWord, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPermissions = (providerName: string, providerKey: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPermissionListResultDto>({
      method: 'GET',
      url: '/api/app/role/permissions',
      params: { providerName, providerKey },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateRoleDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, RoleDto>({
      method: 'PUT',
      url: `/api/app/role/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updatePermissions = (providerName: string, providerKey: string, input: UpdatePermissionsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/app/role/permissions',
      params: { providerName, providerKey },
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
