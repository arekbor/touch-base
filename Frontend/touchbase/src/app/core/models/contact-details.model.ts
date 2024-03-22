import { ContactLabel } from "../enums/contact-label.enum";
import { ContactRelationship } from "../enums/contact-relationship.enum";

export interface ContactDetails {
  id: string;
  firstname: string;
  surname: string;
  phone: string;
  email: string;
  company: string;
  label: ContactLabel;
  birthday: Date;
  relationship: ContactRelationship;
  notes: string;
}
