import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnDestroy } from "@angular/core";
import { AbstractControl } from "@angular/forms";
import { Subscription } from "rxjs";

@Component({
  template: "",
})
export abstract class BaseComponent implements OnDestroy {
  protected errors: string[] = [];
  private subscriptions: Subscription[] = [];

  ngOnDestroy(): void {
    this.subscriptions
      .filter((sub: Subscription) => sub != null)
      .forEach((sub: Subscription) => sub.unsubscribe());
  }

  protected safeSub(...sub: Subscription[]): void {
    this.subscriptions = this.subscriptions.concat(sub);
  }

  protected getControlErrors(control: AbstractControl | null): string[] {
    const errors: string[] = [];
    if (
      control &&
      control.errors &&
      control.invalid &&
      (control.dirty || control.touched)
    ) {
      Object.keys(control.errors).map((error: string) => {
        errors.push(this.errorControlMessages[error](control.errors![error]));
      });
    }
    return errors;
  }

  protected isControlInvalid(control: AbstractControl | null): boolean {
    return control && control.invalid && (control.dirty || control.touched)
      ? true
      : false;
  }

  protected handleHttpErrors(err: HttpErrorResponse): void {
    const errors: string[] = [];

    const detail = err.error.detail || err.statusText;
    if (detail) {
      errors.push(detail);
    }

    const otherErrors = err.error.errors;
    if (otherErrors) {
      Object.keys(otherErrors).forEach((key) => {
        errors.push(otherErrors[key]);
      });
    }

    this.errors = errors;
  }

  private errorControlMessages: Record<string, any> = {
    required: () => `This field is required`,
    email: () => `Invalid email format.`,
    maxlength: (params: any) =>
      `Maximum ${params.requiredLength} characters are allowed.`,
    minlength: (params: any) =>
      `Minimum ${params.requiredLength} characters are required.`,
    notContainUppercase: () =>
      `This field must contain at least one uppercase letter.`,
    notContainLowercase: () =>
      `This field must contain at least one lowercase letter.`,
    notContainNumber: () => `This field must contain at least one number.`,
    notContainSpecialChar: () =>
      `This field must contain at least one special character.`,
    notContainsOnlyLetters: () => `This field can only contain letters.`,
    invalidPhone: () => `Invalid phone number format.`,
    invalidDatebirth: () => `Invalid date of birth.`,
  };
}
