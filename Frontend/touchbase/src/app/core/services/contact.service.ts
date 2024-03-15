import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { ContactDetails } from "../models/contact-details.model";
import { Contact } from "../models/contact.model";
import { CreateContact } from "../models/createContact.model";
import { PaginatedList } from "../models/paginatedList";

@Injectable({
  providedIn: "root",
})
export class ContactService {
  constructor(private httpClient: HttpClient) {}

  create(createContact: CreateContact): Observable<void> {
    return this.httpClient.post<void>(
      `${environment.apiUrl}/Contacts/create`,
      createContact
    );
  }

  getContacts(
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedList<Contact>> {
    return this.httpClient.get<PaginatedList<Contact>>(
      `${environment.apiUrl}/Contacts/list`,
      {
        params: {
          pageNumber,
          pageSize,
        },
      }
    );
  }

  getContact(id: string): Observable<ContactDetails> {
    return this.httpClient.get<ContactDetails>(
      `${environment.apiUrl}/Contacts/details`,
      {
        params: {
          id: id,
        },
      }
    );
  }

  deleteContact(id: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${environment.apiUrl}/Contacts/delete`,
      {
        params: {
          id: id,
        },
      }
    );
  }
}
