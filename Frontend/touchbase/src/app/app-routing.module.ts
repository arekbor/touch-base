import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "./core/guards/auth.guard";
import { NotAuthGuard } from "./core/guards/not-auth.guard";

const routes: Routes = [
  {
    path: "",
    redirectTo: "home",
    pathMatch: "full",
  },
  {
    path: "",
    canActivate: [AuthGuard],
    children: [
      {
        path: "home",
        loadChildren: () =>
          import("./modules/home/home.module").then((m) => m.HomeModule),
      },
      {
        path: "user",
        loadChildren: () =>
          import("./modules/user/user.module").then((m) => m.UserModule),
      },
      {
        path: "contact",
        loadChildren: () =>
          import("./modules/contact/contact.module").then(
            (m) => m.ContactModule
          ),
      },
    ],
  },
  {
    path: "auth",
    canActivate: [NotAuthGuard],
    loadChildren: () =>
      import("./modules/auth/auth.module").then((m) => m.AuthModule),
  },
  {
    path: "**",
    redirectTo: "home",
    pathMatch: "full",
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
