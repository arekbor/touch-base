import { NgModule } from "@angular/core";
import { SharedModule } from "src/app/shared/shared.module";
import { ContactFormComponent } from "./components/contact-form/contact-form.component";
import { ContactRoutingModule } from "./contact-routing.module";
import { ContactDetailsComponent } from "./pages/contact-details/contact-details.component";
import { ContactListComponent } from "./pages/contact-list/contact-list.component";
import { CreateContactComponent } from "./pages/create-contact/create-contact.component";

@NgModule({
  declarations: [
    ContactFormComponent,
    CreateContactComponent,
    ContactListComponent,
    ContactDetailsComponent,
  ],
  imports: [ContactRoutingModule, SharedModule],
})
export class ContactModule {}
