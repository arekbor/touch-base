import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { EnumToArray } from "./pipes/enumToArray.pipe";

@NgModule({
  declarations: [EnumToArray],
  exports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule,
    EnumToArray,
  ],
})
export class SharedModule {}
