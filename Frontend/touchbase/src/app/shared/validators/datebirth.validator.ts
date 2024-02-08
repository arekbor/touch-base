import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const DatebirthValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  if (!control.value) {
    return null;
  }

  const datePattern = /^\d{4}-\d{2}-\d{2}$/;
  if (!datePattern.test(control.value)) {
    return { invaliddatebirth: true };
  }

  const inputDate = new Date(control.value);
  const today = new Date();
  if (inputDate >= today) {
    return { invaliddatebirth: true };
  }

  return null;
};
