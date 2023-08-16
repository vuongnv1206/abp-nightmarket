import { FormGroup } from '@angular/forms';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-validation-message',
  templateUrl: './validation-message.component.html',
  styleUrls: ['./validation-message.component.scss']
})
export class ValidationMessageComponent {
  @Input() entityForm:FormGroup;
  @Input() fieldName:string;
  @Input() validationMessages:any;

  constructor() {


  }
}
