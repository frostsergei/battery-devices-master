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
import { ParameterConstants } from '~/shared/helpers/parameter-constants';
import { InputType } from '~/shared/input/input-type';
import { PrecisionValidator } from '~/shared/validators/precision';

@Component({
  selector: 'app-form-gen-demo',
  templateUrl: './form-gen-demo.component.html',
  styleUrl: './form-gen-demo.component.scss',
})
export class FormGenDemoComponent implements OnInit {
  testData: any;
  testForm: FormGroup = new FormGroup({});
  schemaFileEmpty: boolean = true;
  constructor(private readonly SchemaClient: SchemaClient) {}

  ngOnInit(): void {
    this.getYamlFromFileGenerateForm();
  }

  getYamlFromFileGenerateForm() {
    this.SchemaClient.getParameters().subscribe({
      next: (fileResponse: FileResponse) => {
        fileResponse.data.text().then((text: string) => {
          this.tryLoadParametersFromString(text);
        });
      },
      error: (err: any) => {
        this.schemaFileEmpty = true;
        console.log('Error retrieving parameters:', err);
      },
    });
  }

  private tryLoadParametersFromString(text: string) {
    try {
      this.testData = yaml.load(text);
      const parametersArray = Array.isArray(this.testData.parameters)
        ? this.testData.parameters
        : [this.testData.parameters];
      parametersArray.forEach((parameter: any) => {
        this.processParameter(parameter);
      });
      this.schemaFileEmpty = false;
    } catch (error) {
      this.schemaFileEmpty = true;
      console.log('Your yaml is broken...', error);
    }
  }

  private processParameter(parameter: any) {
    if (Object.hasOwn(parameter, ParameterConstants.typeKey)) {
      const validators = [Validators.required];
      this.addValidatorsBasedOnType(parameter, validators);
      this.addFormControlBasedOnType(
        parameter.type,
        parameter.name,
        parameter.description,
        parameter.hint,
        parameter.description,
        validators,
      );
    }
  }

  private addValidatorsBasedOnType(
    item: any,
    validators: ((
      control: AbstractControl<any, any>,
    ) => ValidationErrors | null)[],
  ) {
    switch (item.type) {
      case ParameterConstants.stringKey: {
        if (Object.hasOwn(item, ParameterConstants.regexKey)) {
          validators.push(Validators.pattern(item.regex));
        }
        break;
      }
      case ParameterConstants.integerKey: {
        if (Object.hasOwn(item, ParameterConstants.minKey)) {
          validators.push(Validators.min(item.min as number));
        }
        if (Object.hasOwn(item, ParameterConstants.maxKey)) {
          validators.push(Validators.max(item.max as number));
        }
        break;
      }
      case ParameterConstants.decimalKey: {
        if (Object.hasOwn(item, ParameterConstants.minKey)) {
          validators.push(Validators.min(item.min as number));
        }
        if (Object.hasOwn(item, ParameterConstants.maxKey)) {
          validators.push(Validators.max(item.max as number));
        }
        if (Object.hasOwn(item, ParameterConstants.precisionKey)) {
          validators.push(PrecisionValidator(item.precision as number));
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

  postJsonToServer(formData: any) {
    const jsonData = JSON.stringify(formData);
    this.SchemaClient.post({
      content: jsonData,
    } as Database).subscribe({
      next: (response: any) => {
        downloadFile(response.data, 'test.txt');
      },
      error: (error: any) => {
        console.error(error);
      },
    });
  }

  // TODO(go1vs1noob): make it more generic later
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
