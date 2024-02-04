import { NgModule } from "@angular/core";
import { ProfileComponent } from "./components/profile/profile.component";
import { ProfileRoutingModule } from "./profile-routing.module";
import { SharedModule } from "src/app/shared/shared.module";

@NgModule({
  declarations: [ProfileComponent],
  imports: [ProfileRoutingModule, SharedModule],
})
export class ProfileModule {}
