import { Component, Input } from "@angular/core";

@Component({
  selector: "app-control-error-component",
  templateUrl: "./control-error.component.html",
})
export class ControlErrorComponent {
  @Input({ required: true }) errors: string[] | null;
}
