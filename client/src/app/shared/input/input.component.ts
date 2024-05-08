import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

import { InputType } from './input-type';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss',
})
export class InputComponent {
  @Input() label: string = '';
  @Input() control!: FormControl;
  @Input() inputType: InputType = InputType.text;
  @Input() controlType: string = 'input';
  @Input() placeholder: string = '';
  @Input() hint: string = '';
  debug: boolean = false;
  showHint: boolean = false;

  showErrors(): boolean {
    const { dirty, touched, errors } = this.control;
    return dirty && touched && errors !== null;
  }

  toggleHint(): void {
    console.log(this.hint);
    this.showHint = !this.showHint;
  }
}
