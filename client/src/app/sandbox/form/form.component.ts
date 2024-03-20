import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { InputType } from '~/shared/input/InputType';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrl: './form.component.scss',
})
export class FormComponent implements OnInit {
  testForm!: FormGroup;
  dateField!: Date;
  intField!: number;
  floatField!: number;
  stringField!: string;
  onlyIntegersAllowedPattern: string = '^\\d+$';

  constructor() {}

  ngOnInit() {
    this.testForm = new FormGroup({
      dateFieldInForm: new FormControl(this.dateField, [Validators.required]),
      intFieldInForm: new FormControl(this.intField, [
        Validators.required,
        Validators.pattern(this.onlyIntegersAllowedPattern),
      ]),
      floatFieldInForm: new FormControl(this.floatField, [Validators.required]),
      stringFieldInForm: new FormControl(this.stringField, [
        Validators.required,
      ]),
    });
  }

  get dateFieldInForm() {
    return this.testForm.controls['dateFieldInForm'] as FormControl;
  }

  get intFieldInForm() {
    return this.testForm.controls['intFieldInForm'] as FormControl;
  }

  get floatFieldInForm() {
    return this.testForm.controls['floatFieldInForm'] as FormControl;
  }

  get stringFieldInForm() {
    return this.testForm.controls['stringFieldInForm'] as FormControl;
  }

  onSubmit() {
    if (this.testForm.invalid) {
      return;
    }
  }

  getInputType(inputType: string): InputType {
    const returnValue: InputType | undefined =
      InputType[inputType as keyof typeof InputType];
    if (returnValue === undefined) {
      throw TypeError(
        `Incorrect enum type: ${inputType}. See InputType.ts for possible values`,
      );
    }
    return returnValue;
  }
}
