import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CreateContactComponent } from "./components/create-contact/create-contact.component";

const routes: Routes = [
  {
    path: "",
    children: [
      {
        path: "create",
        component: CreateContactComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ContactRoutingModule {}
