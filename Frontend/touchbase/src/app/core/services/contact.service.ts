import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { CreateContact } from "../models/createContact.model";

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
}
