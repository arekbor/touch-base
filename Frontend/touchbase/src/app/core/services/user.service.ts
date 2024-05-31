import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Password } from "../models/password.model";
import { User } from "../models/user.models";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private httpClient: HttpClient) {}

  getUser(): Observable<User | null> {
    return this.httpClient.get<User>(`${environment.apiUrl}/Users/get`);
  }

  updateProfile(user: User): Observable<void> {
    return this.httpClient.put<void>(
      `${environment.apiUrl}/Users/updateProfile`,
      user
    );
  }

  updatePassword(password: Password): Observable<void> {
    return this.httpClient.put<void>(
      `${environment.apiUrl}/Users/updatePassword`,
      password
    );
  }
}
