import { KeyFilterModule } from 'primeng/keyfilter';
import { PickListModule } from 'primeng/picklist';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PanelModule } from 'primeng/panel';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { BlockUIModule } from 'primeng/blockui';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { EditorModule } from 'primeng/editor';
import { BadgeModule } from 'primeng/badge';
import { ImageModule } from 'primeng/image';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CalendarModule } from 'primeng/calendar';
import { NightMarketSharedModule } from '../shared/modules/nightmarket-shared.module';
import { SystemRoutingModule } from './system-routing.module';
import { UserDetailComponent } from './user/user-detail/user-detail.component';
import { SetPasswordComponent } from './user/set-password/set-password.component';
import { RoleAssignComponent } from './user/role-assign/role-assign.component';
import { RoleComponent } from './role/role.component';
import { RoleDetailComponent } from './role/role-detail/role-detail.component';
import { PermissionGrantComponent } from './role/permission-grant/permission-grant.component';
import { UserComponent } from './user/user.component';


@NgModule({
  declarations: [
    RoleComponent,
    RoleDetailComponent,
    PermissionGrantComponent,
    UserDetailComponent,
    SetPasswordComponent,
    RoleAssignComponent,
     PermissionGrantComponent,
     UserComponent,
     UserDetailComponent,
     RoleAssignComponent,
     SetPasswordComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
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
    ImageModule,
    ConfirmDialogModule,
    CalendarModule,
    SystemRoutingModule,
    PickListModule,
    KeyFilterModule
  ]
})
export class SystemModule { }
