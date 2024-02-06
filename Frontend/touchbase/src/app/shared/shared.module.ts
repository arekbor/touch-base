import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ErrorsComponent } from "./components/errors/errors.component";
import { SpinnerBtnComponent } from "./components/spinner-btn/spinner-btn.component";
import { EnumToArray } from "./pipes/enumToArray.pipe";

@NgModule({
  declarations: [EnumToArray, SpinnerBtnComponent, ErrorsComponent],
  imports: [CommonModule],
  exports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule,
    EnumToArray,
    SpinnerBtnComponent,
    ErrorsComponent,
  ],
})
export class SharedModule {}
