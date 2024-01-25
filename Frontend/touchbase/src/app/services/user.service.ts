import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment.development";
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

@Injectable()
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

  getUser(): Observable<User | null> {
    return this.httpClient.get<User>(`${environment.base_url}/Users/get`);
  }

  getSecret(): Observable<Secret | null> {
    return this.httpClient.get<Secret>(`${environment.base_url}/Users/secret`);
  }

  getFakeData(): Observable<FakeData[] | null> {
    return this.httpClient.get<FakeData[]>(
      `${environment.base_url}/Users/fakeData`
    );
  }

  getCustomData(): Observable<CustomData | null> {
    return this.httpClient.get<CustomData>(
      `${environment.base_url}/Users/customData`
    );
  }
}
