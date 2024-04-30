import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';

import { saveAs as downloadFile } from 'file-saver';
import * as yaml from 'js-yaml';

import { Database, FileResponse, SchemaClient } from '~/client';
import { CustomFormControl } from '~/shared/classes/CustomFormControl';
import { InputType } from '~/shared/input/InputType';
import { precisionValidator } from '~/shared/validators/precision';

@Component({
  selector: 'app-form-gen-demo',
  templateUrl: './form-gen-demo.component.html',
  styleUrl: './form-gen-demo.component.scss',
})
export class FormGenDemoComponent implements OnInit {
  // eslint-disable-next-line
  testData: any;
  testForm: FormGroup = new FormGroup({});
  schemaFileEmpty: boolean = true;
  constructor(
    private readonly http: HttpClient,
    private readonly SchemaClient: SchemaClient,
  ) {}

  ngOnInit(): void {
    this.getYamlFromFileGenerateForm();
  }

  // TODO(go1vs1noob): segregate gen logic, fix ugliness
  getYamlFromFileGenerateForm() {
    this.SchemaClient.getParameters().subscribe({
      next: (fileResponse: FileResponse) => {
        // @ts-ignore
        fileResponse.data.text().then((text) => {
          try {
            this.testData = yaml.load(text);
            const dataArray = Array.isArray(this.testData.parameters)
              ? this.testData.parameters
              : [this.testData.parameters];
            // eslint-disable-next-line
            dataArray.forEach((item: any) => {
              // eslint-disable-next-line
              if (item.hasOwnProperty('type')) {
                const validators = [Validators.required];
                this.addValidatorsBasedOnType(item, validators);
                this.addFormControlBasedOnType(
                  item.type,
                  item.name,
                  item.description,
                  item.hint,
                  item.description,
                  validators,
                );
              }
            });
            this.schemaFileEmpty = false;
          } catch (error) {
            this.schemaFileEmpty = true;
            console.log('Your yaml is broken...', error);
          }
        });
      },
      error: (err) => {
        this.schemaFileEmpty = true;
        console.log('Error retrieving parameters:', err);
      },
    });
  }

  // TODO(go1vs1noob): опять поправить линтер, надоело ограничение на any...
  private addValidatorsBasedOnType(
    item: any,
    validators: ((
      control: AbstractControl<any, any>,
    ) => ValidationErrors | null)[],
  ) {
    switch (item.type) {
      case 'string': {
        if (item.hasOwnProperty('regex')) {
          validators.push(Validators.pattern(item.regex));
        }
        break;
      }
      case 'integer': {
        if (item.hasOwnProperty('min')) {
          validators.push(Validators.min(item.min as number));
        }
        if (item.hasOwnProperty('max')) {
          validators.push(Validators.max(item.max as number));
        }
        break;
      }
      case 'decimal': {
        if (item.hasOwnProperty('min')) {
          validators.push(Validators.min(item.min as number));
        }
        if (item.hasOwnProperty('max')) {
          validators.push(Validators.max(item.max as number));
        }
        if (item.hasOwnProperty('precision')) {
          validators.push(precisionValidator(item.precision as number));
        }
        break;
      }
      default: {
        break;
      }
    }
  }

  private addFormControlBasedOnType(
    type: string,
    name: string,
    placeholder: string,
    hint: string,
    description: string,
    validators: ((
      control: AbstractControl<any, any>,
    ) => ValidationErrors | null)[],
  ) {
    this.testForm.addControl(
      type + 'FieldInForm',
      new CustomFormControl('', validators, [], {
        type: type,
        name: name,
        placeholder: placeholder,
        hint: hint,
        description: description,
      }),
    );
  }

  // TODO(go1vs1noob): move it closer to InputType, maybe in the same file
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

  // eslint-disable-next-line
  postJsonToServer(formData: any) {
    const jsonData = JSON.stringify(formData);
    this.SchemaClient.post({
      content: jsonData,
    } as Database).subscribe({
      next: (responce) => {
        downloadFile(responce.data, 'test.txt');
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  // TODO(go1vs1noob): make it more generic later
  // eslint-disable-next-line
  getJsonForServer(form: FormGroup): any {
    // eslint-disable-next-line
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

  // TODO(go1vs1noob): convert this to pipe later
  toControl(absCtrl: AbstractControl | null): FormControl {
    const ctrl = absCtrl as FormControl;
    return ctrl;
  }

  // TODO(go1vs1noob): convert this to pipe later
  toCustomControl(absCtrl: AbstractControl | null): CustomFormControl {
    const ctrl = absCtrl as CustomFormControl;
    return ctrl;
  }
}
