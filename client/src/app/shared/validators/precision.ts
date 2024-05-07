import { ValidatorFn, AbstractControl } from '@angular/forms';

export function PrecisionValidator(precision: number): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const regex = new RegExp(`^-?\\d+(\\.\\d{1,${precision}})?$`);
    if (
      control.value === null ||
      control.value === '' ||
      regex.test(control.value)
    ) {
      return null; // Return null if there is no error
    } else {
      return {
        precisionError: `Number is not formatted correctly, expected no more than ${precision} decimal places.`,
      };
    }
  };
}
