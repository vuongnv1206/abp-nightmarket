import { PermissionGuard } from '@abp/ng.core';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleComponent } from './role/role.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  {
    path: 'role',
    component: RoleComponent,
    // canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'AbpIdentity.Roles',
    },
  },
  {
    path: 'user',
    component: UserComponent,
    // canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'AbpIdentity.Users',
    },
  },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule],
})
export class SystemRoutingModule { }
