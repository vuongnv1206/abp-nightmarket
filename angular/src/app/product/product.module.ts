import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ProductComponent } from './product.component';
import { ProductRoutingModule } from './product-routing.module';
import { PanelModule} from 'primeng/panel';
import { TableModule} from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { BlockUIModule } from 'primeng/blockui';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductAttributeComponent } from './product-attribute/product-attribute.component';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { EditorModule } from 'primeng/editor';
import { NightMarketSharedModule } from '../shared/modules/nightmarket-shared.module';
import { BadgeModule } from 'primeng/badge';
import { TagModule } from 'primeng/tag';
import { ImageModule } from 'primeng/image';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
@NgModule({
  declarations: [ProductComponent, ProductDetailComponent, ProductAttributeComponent],
  imports: [SharedModule, ProductRoutingModule,
    PanelModule,
    TableModule,
    PaginatorModule,
    BlockUIModule,
    ButtonModule,
    DropdownModule,
    InputTextModule,
    ProgressSpinnerModule,
    DynamicDialogModule,
    InputNumberModule,
    CheckboxModule,
    InputTextareaModule,
    EditorModule,
    NightMarketSharedModule,
    BadgeModule,
    TagModule,
    ImageModule,
    ConfirmDialogModule
  ],


})
export class ProductModule {}
