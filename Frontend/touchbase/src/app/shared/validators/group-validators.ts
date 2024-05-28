import { ValidatorFn, Validators } from "@angular/forms";
import { CustomValidators } from "./custom-validators";

export class GroupValidators {
  static personName(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.maxLength(40),
      CustomValidators.containsOnlyLetters(),
    ];
  }

  static username(): ValidatorFn[] {
    return [Validators.required, Validators.maxLength(40)];
  }

  static email(): ValidatorFn[] {
    return [Validators.required, Validators.email];
  }
}
