import { FormControl } from '@angular/forms';

export class CustomFormControl extends FormControl {
  // Additional property
  name: string = '';
  type: string = '';
  placeholder: string = '';
  hint: string = '';
  description: string = '';

  constructor(
    // eslint-disable-next-line
    initialValue: any,
    // eslint-disable-next-line
    validatorOrOpts?: any,
    // eslint-disable-next-line
    asyncValidator?: any,
    // eslint-disable-next-line
    data?: any,
  ) {
    super(initialValue, validatorOrOpts, asyncValidator);
    this.name = data.name;
    this.type = data.type;
    this.placeholder = data.placeholder;
  }
}
