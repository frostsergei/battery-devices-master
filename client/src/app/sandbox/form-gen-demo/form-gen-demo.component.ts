import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import * as yaml from 'js-yaml';
import { InputType } from '../../shared/input/InputType';
import { CustomFormControl } from '../../shared/CustomFormControl';

@Component({
  selector: 'app-form-gen-demo',
  templateUrl: './form-gen-demo.component.html',
  styleUrl: './form-gen-demo.component.scss'
})
export class FormGenDemoComponent implements OnInit {

  testData: any;
  testForm: FormGroup = new FormGroup({});
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get('assets/test.yml', { responseType: 'text' }).subscribe(data => {
      this.testData = yaml.load(data);

      const dataArray = Array.isArray(this.testData.parameters) ? this.testData.parameters : [this.testData.parameters];
      dataArray.forEach((item: any) => {
        if (item.hasOwnProperty('type')) {
          this.addFormControlBasedOnType(item.type, item.name, item.description);
        }
       // console.log(this.toCustomControl(this.testForm.get(item.type + "FieldInForm"))?.name);
      });
    });
  }

  addFormControlBasedOnType(type: string, name: string, placeholder: string) {
    this.testForm.addControl(type + 'FieldInForm',
      new CustomFormControl('', [Validators.required], [],
        {
          type: type,
          name: name,
          placeholder: placeholder
        }));
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
  onSubmit() {
    throw new Error('Method not implemented.');
  }
  getControlNames(): string[] {
    return Object.keys(this.testForm.controls);
  }
  // convert this to pipe
  toControl(absCtrl: AbstractControl | null): FormControl {
    const ctrl = absCtrl as FormControl;
    // if(!ctrl) throw;
    return ctrl;
  }
  toCustomControl(absCtrl: AbstractControl | null): CustomFormControl {
    const ctrl = absCtrl as CustomFormControl;
    // if(!ctrl) throw;
    return ctrl;
  }
}

