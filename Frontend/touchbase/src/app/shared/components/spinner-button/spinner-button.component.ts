import { Component, Input } from "@angular/core";

@Component({
  selector: "app-spinner-button",
  templateUrl: "./spinner-button.component.html",
})
export class SpinnerButtonComponent {
  @Input({ required: true }) isLoading: boolean;
  @Input({ required: true }) isFormValid: boolean;
}
