import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import * as yaml from 'js-yaml';

import { CustomFormControl } from '../../shared/classes/CustomFormControl';
import { InputType } from '../../shared/input/InputType';

import { JsonForm, JsonFormClient } from './../../../build/client';

@Component({
  selector: 'app-form-gen-demo',
  templateUrl: './form-gen-demo.component.html',
  styleUrl: './form-gen-demo.component.scss',
})
export class FormGenDemoComponent implements OnInit {
  testData: any;
  testForm: FormGroup = new FormGroup({});
  constructor(
    private readonly http: HttpClient,
    private readonly jsonFormClient: JsonFormClient,
  ) {}

  ngOnInit(): void {
    this.getYamlFromFileGenerateForm();
  }

  // remake it so yml gets recieved from handle, segregate gen logic
  getYamlFromFileGenerateForm() {
    this.http
      .get('assets/test.yml', { responseType: 'text' })
      .subscribe((data) => {
        try {
          this.testData = yaml.load(data);
          const dataArray = Array.isArray(this.testData.parameters)
            ? this.testData.parameters
            : [this.testData.parameters];
          dataArray.forEach((item: any) => {
            if (item.hasOwnProperty('type')) {
              this.addFormControlBasedOnType(
                item.type,
                item.name,
                item.description,
                item.hint,
                item.description,
              );
            }
          });
        } catch {
          console.error('Your yaml is broken...');
        }
      });
  }

  // there will be some logic (adding validators) based on "type" later
  addFormControlBasedOnType(
    type: string,
    name: string,
    placeholder: string,
    hint: string,
    description: string,
  ) {
    this.testForm.addControl(
      type + 'FieldInForm',
      new CustomFormControl('', [Validators.required], [], {
        type: type,
        name: name,
        placeholder: placeholder,
        hint: hint,
        description: description,
      }),
    );
  }

  // move it closer to InputType, maybe in the same file
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
    const formData = this.getJsonForServer(this.testForm);
    this.postJsonToServer(formData);
  }

  postJsonToServer(formData: any) {
    const jsonData = JSON.stringify(formData);
    this.jsonFormClient
      .post({
        form: jsonData,
      } as JsonForm)
      .subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (error) => {
          console.error(error);
        },
      });
  }

  // make it more generic later
  getJsonForServer(form: FormGroup): any {
    const formData: any = {
      TagList: {
        '@TargetDevice': 'TSPT941_20',
        '@Id': '90253',
        '@SerialNumber': '',
        Channel: {
          '@No': '0',
          '@Name': 'ОБЩ',
          '@Kind': 'Common',
          '@Prefix': 'ОБЩ',
          '@Description': 'системный канал',
          Tag: [],
        },
      },
    };
    Object.keys(form.controls).forEach((key, index) => {
      const control = form.get(key) as CustomFormControl;
      formData.TagList.Channel.Tag.push({
        '@Ordinal': index.toString(),
        '@Name': control.name,
        '@Id': control.name,
        '@Value': control.value,
        '@Eu': '',
      });
    });
    return formData;
  }

  getControlNames(): string[] {
    return Object.keys(this.testForm.controls);
  }

  // convert this to pipe later
  toControl(absCtrl: AbstractControl | null): FormControl {
    const ctrl = absCtrl as FormControl;
    return ctrl;
  }

  // convert this to pipe later
  toCustomControl(absCtrl: AbstractControl | null): CustomFormControl {
    const ctrl = absCtrl as CustomFormControl;
    return ctrl;
  }
}