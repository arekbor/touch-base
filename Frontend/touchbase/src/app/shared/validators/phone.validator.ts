import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const PhoneValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  if (!control.value) {
    return null;
  }

  const phonePattern = /^\d{9}$/;
  if (!phonePattern.test(control.value)) {
    return { invalidphone: true };
  }

  return null;
};
