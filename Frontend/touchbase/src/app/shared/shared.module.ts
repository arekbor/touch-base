import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { HttpErrorComponent } from "./components/http-error/http-error.component";
import { PaginatorComponent } from "./components/paginator/paginator.component";
import { SpinnerComponent } from "./components/spinner/spinner.component";

@NgModule({
  declarations: [HttpErrorComponent, SpinnerComponent, PaginatorComponent],
  imports: [CommonModule],
  exports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpErrorComponent,
    SpinnerComponent,
    PaginatorComponent,
  ],
})
export class SharedModule {}
