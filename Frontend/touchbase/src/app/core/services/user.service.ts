import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Claims } from "../models/claims.model";
import { User } from "../models/user.models";
import { StorageService } from "./storage.service";

export interface Secret {
  message: string;
}

export interface FakeData {
  messages: string;
}

export interface CustomData {
  customMessage: string;
}

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(
    private storageService: StorageService,
    private jwtHelperService: JwtHelperService,
    private httpClient: HttpClient
  ) {}

  getUserClaims(): Claims | null {
    const accessToken = this.storageService.getAccessToken();
    if (accessToken) {
      return this.jwtHelperService.decodeToken(accessToken);
    }
    return null;
  }

  isLogged(): boolean {
    return this.getUserClaims() != null;
  }

  getUser(): Observable<User | null> {
    return this.httpClient.get<User>(`${environment.apiUrl}/Users/get`);
  }
}
