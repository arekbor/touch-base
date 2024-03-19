import { AbstractControl, ValidationErrors } from "@angular/forms";
import { BaseComponent } from "./base.component";

export abstract class BaseFormComponent extends BaseComponent {
  protected getFormFieldErrors(
    form: AbstractControl,
    field: string
  ): ValidationErrors | null {
    console.log("execute field errors");
    const control = form.get(field);
    if (control && control.invalid && (control.dirty || control.touched)) {
      return control.errors;
    }
    return null;
  }
}
