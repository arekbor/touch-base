import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Claims } from "../models/claims.model";
import { Login } from "../models/login.model";
import { Register } from "../models/register.model";
import { Tokens } from "../models/tokens.model";
import { StorageService } from "./storage.service";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  constructor(
    private httpClient: HttpClient,
    private storageService: StorageService,
    private jwtHelperService: JwtHelperService
  ) {}

  login(login: Login): Observable<Tokens | null> {
    return this.httpClient.post<Tokens>(
      `${environment.apiUrl}/Users/login`,
      login
    );
  }

  logout(): void {
    this.storageService.removeAuthorizationTokens();
    window.location.reload();
  }

  register(register: Register): Observable<void> {
    return this.httpClient.post<void>(
      `${environment.apiUrl}/Users/register`,
      register
    );
  }

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

  setAuthTokens(tokens: Tokens): void {
    this.storageService.setAccessToken(tokens.accessToken);
    this.storageService.setRefreshToken(tokens.refreshToken);
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
