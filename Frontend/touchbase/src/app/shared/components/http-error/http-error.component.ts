import { Component, Input } from "@angular/core";

@Component({
  selector: "app-http-error",
  templateUrl: "./http-error.component.html",
})
export class HttpErrorComponent {
  @Input({ required: true }) errors: string[];
}
