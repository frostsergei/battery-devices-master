import { Component } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';

import { saveAs as downloadFile } from 'file-saver';
import * as yaml from 'js-yaml';

import { Database, SchemaClient } from '~/client';
import { CustomFormControl } from '~/shared/classes/CustomFormControl';
import { ParameterConstants } from '~/shared/helpers/parameter-constants';
import { InputType } from '~/shared/input/input-type';
import { PrecisionValidator } from '~/shared/validators/precision';

@Component({
  selector: 'app-form-gen-demo',
  templateUrl: './form-gen-demo.component.html',
  styleUrl: './form-gen-demo.component.scss',
})
export class FormGenDemoComponent {
  testData: any;
  testForm: FormGroup = new FormGroup({});
  schemaFileEmpty: boolean = true;
  tabs: any[] = [];

  constructor(private readonly SchemaClient: SchemaClient) {}

  private tryLoadParametersFromString(text: string) {
    try {
      this.testData = yaml.load(text);
      const parametersArray = Array.isArray(this.testData.parameters)
        ? this.testData.parameters
        : null;
      const windowsArray = Array.isArray(this.testData.windows)
        ? this.testData.windows
        : null;
      if (parametersArray === null) {
        throw Error('Отсутствует вкладка параметров');
      }
      if (windowsArray === null) {
        this.tabs = [
          {
            name: 'Все параметры',
            description: 'Все параметры в одной вкладке',
            parameters: parametersArray.map((parameter: any) => parameter.name),
          },
        ];
      } else {
        this.tabs = windowsArray[1].tabs;
      }
      this.testForm.reset();
      this.testForm = new FormGroup({});
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
      name,
      new CustomFormControl('', validators, [], {
        type: type,
        name: name,
        placeholder: placeholder,
        hint: hint,
        description: description,
      }),
    );
  }

  getInputType(inputType: string): InputType {
    const returnValue: InputType | undefined =
      InputType[inputType as keyof typeof InputType];
    if (returnValue === undefined) {
      this.schemaFileEmpty = true;
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
        downloadFile(response.data, 'data.xml');
      },
      error: (error: any) => {
        console.error(error);
      },
    });
  }

  getJsonForServer(form: FormGroup): any {
    const formData: any = {
      TagList: {
        '@TargetDevice': 'SPT944',
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

  handleFileData(fileData: string): void {
    this.tryLoadParametersFromString(fileData);
  }

  getControlNames(): string[] {
    return Object.keys(this.testForm.controls);
  }

  getControlsByTab(tabName: string): string[] {
    const tab = this.tabs.find((t) => t.name === tabName);
    return tab ? tab.parameters : [];
  }

  toControl(absCtrl: AbstractControl | null): FormControl {
    const ctrl = absCtrl as FormControl;
    return ctrl;
  }

  toCustomControl(absCtrl: AbstractControl | null): CustomFormControl {
    const ctrl = absCtrl as CustomFormControl;
    return ctrl;
  }
}
