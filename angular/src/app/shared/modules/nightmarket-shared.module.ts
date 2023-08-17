import { CommonModule } from '@angular/common';
import { CoreModule } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { ValidationMessageComponent } from './validation-message/validation-message.component';


@NgModule({
  imports: [CoreModule, CommonModule],
  declarations: [
    ValidationMessageComponent
  ],
  exports: [ValidationMessageComponent]
})

export class NightMarketSharedModule{

}
