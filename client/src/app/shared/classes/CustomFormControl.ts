import { FormControl } from '@angular/forms';

export class CustomFormControl extends FormControl {
  // Additional properties
  name: string = '';
  type: string = '';
  placeholder: string = '';
  hint: string = '';
  description: string = '';

  /* eslint-disable */
  constructor(
    initialValue: any,
    validatorOrOpts?: any,
    asyncValidator?: any,
    data?: any,
  ) {
    super(initialValue, validatorOrOpts, asyncValidator);
    this.name = data.name;
    this.type = data.type;
    this.hint = data.hint;
    this.placeholder = data.placeholder;
    this.description = data.description;
  }
  /* eslint-enable */
}
