import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const PasswordValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  if (!control.value) {
    return null;
  }

  const uppercaseLetterPattern = /[A-Z]+/;
  if (!uppercaseLetterPattern.test(control.value)) {
    return { invaliduppercaseletter: true };
  }

  const lowercaseLetterPattern = /[a-z]+/;
  if (!lowercaseLetterPattern.test(control.value)) {
    return { invalidlowercaseletter: true };
  }

  const numberPattern = /[0-9]+/;
  if (!numberPattern.test(control.value)) {
    return { invalidnumber: true };
  }

  const specialCharacterPattern = /[""!@#$%^&*(){}:;<>,.?/+_=|'~\\-]/;
  if (!specialCharacterPattern.test(control.value)) {
    return { invalidspecialcharacter: true };
  }

  return null;
};
