import { ContactLabel } from "../enums/contactLabel.enum";
import { ContactRelationship } from "../enums/contactRelationship.enum";

export interface CreateContact {
  firstname: string;
  surname: string;
  company: string | null;
  phone: string | null;
  label: ContactLabel;
  email: string;
  birthday: Date | null;
  relationship: ContactRelationship;
  notes: string | null;
}
