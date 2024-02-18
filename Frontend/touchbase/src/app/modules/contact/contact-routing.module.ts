import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ContactDetailsComponent } from "./components/contact-details/contact-details.component";
import { ContactListComponent } from "./components/contact-list/contact-list.component";
import { CreateContactComponent } from "./components/create-contact/create-contact.component";

const routes: Routes = [
  {
    path: "",
    children: [
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
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ContactRoutingModule {}
