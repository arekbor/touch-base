import { Component, OnDestroy } from "@angular/core";
import { AbstractControl, ValidationErrors } from "@angular/forms";
import { Subscription } from "rxjs";

@Component({
  template: "",
})
export abstract class BaseComponent implements OnDestroy {
  private subscriptions: Subscription[] = [];

  ngOnDestroy(): void {
    this.subscriptions
      .filter((sub: Subscription) => sub != null)
      .forEach((sub: Subscription) => sub.unsubscribe());
  }

  protected safeSub(...sub: Subscription[]): void {
    this.subscriptions = this.subscriptions.concat(sub);
  }

  protected getFormFieldErrors(
    form: AbstractControl,
    field: string
  ): ValidationErrors | null {
    console.log("test", field);

    const control = form.get(field);
    if (control && control.invalid && (control.dirty || control.touched)) {
      return control.errors;
    }
    return null;
  }
}
