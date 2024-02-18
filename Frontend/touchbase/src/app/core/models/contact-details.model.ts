import { ContactLabel } from "../enums/contactLabel.enum";
import { ContactRelationship } from "../enums/contactRelationship.enum";

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
