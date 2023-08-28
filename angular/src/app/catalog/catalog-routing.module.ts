import { NgModule } from '@angular/core';
import { PermissionGuard } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ProductComponent } from './product/product.component';
import { AttributeComponent } from './attribute/attribute.component';

const routes: Routes = [
  {
    path: 'product',
    component: ProductComponent,
    //canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'NightMarketAdminCatalog.Product',
    },
  },
  {
    path: 'attribute',
    component: AttributeComponent,
    //canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'NightMarketAdminCatalog.Attribute',
    },
  },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    [RouterModule.forChild(routes)],
  ],
  exports: [RouterModule],
})
export class CatalogRoutingModule { }
