import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt";
import { map, Observable } from "rxjs";
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

  reloadTokens() {
    const refreshToken = this.storageService.getRefreshToken();
    return this.getRefreshToken(refreshToken).pipe(
      map((tokens: Tokens | null) => {
        if (tokens && tokens.accessToken && tokens.refreshToken) {
          this.storageService.setAuthorizationTokens(tokens);
        }
      })
    );
  }

  private getRefreshToken(
    refreshToken: string | null
  ): Observable<Tokens | null> {
    return this.httpClient.post<Tokens>(`${environment.apiUrl}/Users/refresh`, {
      refreshToken: refreshToken,
    });
  }
}
