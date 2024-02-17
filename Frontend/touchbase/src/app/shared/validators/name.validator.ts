import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const NameValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  if (!control.value) {
    return null;
  }

  const namePattern = /^[a-zA-Z]*$/;
  if (!namePattern.test(control.value)) {
    return { invalidname: true };
  }

  return null;
};
