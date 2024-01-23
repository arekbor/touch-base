import { Injectable } from "@angular/core";

enum StorageKeys {
  ACCESS_TOKEN = "access-token",
  REFRESH_TOKEN = "refresh-token",
}

@Injectable()
export class StorageService {
  setAccessToken(token: string): void {
    window.localStorage.setItem(StorageKeys.ACCESS_TOKEN, token);
  }

  getAccessToken(): string | null {
    return window.localStorage.getItem(StorageKeys.ACCESS_TOKEN);
  }

  setRefreshToken(token: string): void {
    window.localStorage.setItem(StorageKeys.REFRESH_TOKEN, token);
  }

  getRefreshToken(): string | null {
    return window.localStorage.getItem(StorageKeys.REFRESH_TOKEN);
  }

  removeAuthorizationTokens() {
    window.localStorage.removeItem(StorageKeys.ACCESS_TOKEN);
    window.localStorage.removeItem(StorageKeys.REFRESH_TOKEN);
  }
}
