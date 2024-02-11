import { NgModule } from "@angular/core";
import { SharedModule } from "src/app/shared/shared.module";
import { ContactListComponent } from "./components/contact-list/contact-list.component";
import { CreateContactComponent } from "./components/create-contact/create-contact.component";
import { ContactRoutingModule } from "./contact-routing.module";

@NgModule({
  declarations: [CreateContactComponent, ContactListComponent],
  imports: [ContactRoutingModule, SharedModule],
})
export class ContactModule {}
