import { NgModule } from "@angular/core";
import { SharedModule } from "src/app/shared/shared.module";
import { PasswordComponent } from "./pages/password/password.component";
import { ProfileComponent } from "./pages/profile/profile.component";
import { UserRoutingModule } from "./user-routing.module";

@NgModule({
  declarations: [ProfileComponent, PasswordComponent],
  imports: [UserRoutingModule, SharedModule],
})
export class UserModule {}
