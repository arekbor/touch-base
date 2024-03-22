import { NgModule } from "@angular/core";
import { SharedModule } from "src/app/shared/shared.module";
import { ProfileComponent } from "./pages/profile/profile.component";
import { ProfileRoutingModule } from "./profile-routing.module";

@NgModule({
  declarations: [ProfileComponent],
  imports: [ProfileRoutingModule, SharedModule],
})
export class ProfileModule {}
