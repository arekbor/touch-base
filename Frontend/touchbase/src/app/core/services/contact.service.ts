import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { ContactBody } from "../models/contact-body.model";
import { ContactDetails } from "../models/contact-details.model";
import { Contact } from "../models/contact.model";
import { PaginatedList } from "../models/paginated-list";

@Injectable({
  providedIn: "root",
})
export class ContactService {
  constructor(private httpClient: HttpClient) {}

  create(contactBody: ContactBody): Observable<void> {
    return this.httpClient.post<void>(
      `${environment.apiUrl}/Contacts/create`,
      contactBody
    );
  }

  update(contactBody: ContactBody, contactId: string): Observable<void> {
    return this.httpClient.put<void>(
      `${environment.apiUrl}/Contacts/update`,
      contactBody,
      { params: { id: contactId } }
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
