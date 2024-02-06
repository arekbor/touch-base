import { Component, Input } from "@angular/core";

@Component({
  selector: "app-spinner-btn",
  templateUrl: "./spinner-btn.component.html",
})
export class SpinnerBtnComponent {
  @Input({ required: true }) isLoading: boolean;
  @Input({ required: true }) isFormValid: boolean;
}
