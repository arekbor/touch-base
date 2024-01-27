import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Tokens } from "../models/tokens.model";
import { StorageService } from "./storage.service";

@Injectable()
export class AuthService {
  constructor(
    private httpClient: HttpClient,
    private storageService: StorageService,
    private jwtHelperService: JwtHelperService
  ) {}

  login(email: string, password: string): Observable<Tokens | null> {
    return this.httpClient.post<Tokens>(`${environment.apiUrl}/Users/login`, {
      email: email,
      password: password,
    });
  }

  getValidAccessToken(): string | null {
    const accessToken = this.storageService.getAccessToken();
    if (accessToken && !this.jwtHelperService.isTokenExpired(accessToken)) {
      return accessToken;
    }
    return null;
  }

  getRefreshToken(refreshToken: string): Observable<Tokens | null> {
    return this.httpClient.post<Tokens>(`${environment.apiUrl}/Users/refresh`, {
      refreshToken: refreshToken,
    });
  }
}
