import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

import { InputType } from './InputType';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss',
})
export class InputComponent {
  @Input() label = '';
  @Input() control = new FormControl('');
  @Input() inputType: InputType = InputType.text;
  @Input() controlType: string = 'input';
  @Input() placeholder: string = '';
  @Input() hint: string = '';
  showHint: boolean = false;

  showErrors(): boolean {
    const { dirty, touched, errors } = this.control;
    return dirty && touched && errors !== null;
  }

  toggleHint(): void {
    this.showHint = !this.showHint;
  }
}
