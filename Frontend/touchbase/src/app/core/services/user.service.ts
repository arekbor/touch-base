import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { User } from "../models/user.models";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private httpClient: HttpClient) {}

  getUser(): Observable<User | null> {
    return this.httpClient.get<User>(`${environment.apiUrl}/Users/get`);
  }
}
