import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ControlErrorComponent } from "./components/control-error/control-error.component";
import { HttpErrorComponent } from "./components/http-error/http-error.component";
import { PaginatorComponent } from "./components/paginator/paginator.component";
import { SpinnerComponent } from "./components/spinner/spinner.component";

@NgModule({
  declarations: [
    HttpErrorComponent,
    ControlErrorComponent,
    SpinnerComponent,
    PaginatorComponent,
  ],
  imports: [CommonModule],
  exports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpErrorComponent,
    ControlErrorComponent,
    SpinnerComponent,
    PaginatorComponent,
  ],
})
export class SharedModule {}
