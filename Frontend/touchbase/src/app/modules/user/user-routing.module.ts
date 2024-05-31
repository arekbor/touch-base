import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PasswordComponent } from "./pages/password/password.component";
import { ProfileComponent } from "./pages/profile/profile.component";

const routes: Routes = [
  {
    path: "",
    redirectTo: "profile",
    pathMatch: "full",
  },
  {
    path: "profile",
    component: ProfileComponent,
  },
  {
    path: "password",
    component: PasswordComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
