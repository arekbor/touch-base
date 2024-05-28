import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class CustomValidators {
  static datebirth(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const datePattern = /^\d{4}-\d{2}-\d{2}$/;
      if (!datePattern.test(control.value)) {
        return { invalidDatebirth: true };
      }

      const inputDate = new Date(control.value);
      const today = new Date();
      if (inputDate >= today) {
        return { invalidDatebirth: true };
      }

      return null;
    };
  }

  static phone(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }

      const phonePattern = /^\d{9}$/;
      if (!phonePattern.test(control.value)) {
        return { invalidPhone: true };
      }

      return null;
    };
  }

  static containsOnlyLetters(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const pattern = /^[a-zA-Z]*$/;
      if (!pattern.test(control.value)) {
        return { notContainsOnlyLetters: true };
      }

      return null;
    };
  }

  static containsUppercase(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const uppercaseLetterPattern = /[A-Z]+/;
      if (!uppercaseLetterPattern.test(control.value)) {
        return { notContainUppercase: true };
      }

      return null;
    };
  }

  static containsLowercase(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const lowercaseLetterPattern = /[a-z]+/;
      if (!lowercaseLetterPattern.test(control.value)) {
        return { notContainLowercase: true };
      }

      return null;
    };
  }

  static containNumber(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const numberPattern = /[0-9]+/;
      if (!numberPattern.test(control.value)) {
        return { notContainNumber: true };
      }

      return null;
    };
  }

  static containsSpecialChar(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const specialCharacterPattern = /[""!@#$%^&*(){}:;<>,.?/+_=|'~\\-]/;
      if (!specialCharacterPattern.test(control.value)) {
        return { notContainSpecialChar: true };
      }

      return null;
    };
  }
}
