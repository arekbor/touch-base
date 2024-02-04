import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const PhoneValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const phonePattern = /^\d{9}$/;

  if (control.value && !phonePattern.test(control.value)) {
    console.log("debug");
    return { invalidphone: true };
  }

  return null;
};
