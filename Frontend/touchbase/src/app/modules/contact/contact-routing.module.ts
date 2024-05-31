import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ContactDetailsComponent } from "./pages/contact-details/contact-details.component";
import { ContactListComponent } from "./pages/contact-list/contact-list.component";
import { CreateContactComponent } from "./pages/create-contact/create-contact.component";

const routes: Routes = [
  {
    path: "",
    redirectTo: "list",
    pathMatch: "full",
  },
  {
    path: "details/:id",
    component: ContactDetailsComponent,
  },
  {
    path: "list",
    component: ContactListComponent,
  },
  {
    path: "create",
    component: CreateContactComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ContactRoutingModule {}
