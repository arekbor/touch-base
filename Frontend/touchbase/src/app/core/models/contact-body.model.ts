import { ContactLabel } from "../enums/contact-label.enum";
import { ContactRelationship } from "../enums/contact-relationship.enum";

export interface ContactBody {
  firstname: string;
  lastname: string;
  company: string | null;
  phone: string | null;
  label: ContactLabel;
  email: string;
  birthday: Date | null;
  relationship: ContactRelationship;
  notes: string | null;
}
