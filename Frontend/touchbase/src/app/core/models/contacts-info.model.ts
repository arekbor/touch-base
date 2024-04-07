import { Contact } from "./contact.model";

export interface ContactsInfo {
  countOfContacts: number;
  lastCreatedContact?: Contact;
}
