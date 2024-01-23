import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import {
  BehaviorSubject,
  Observable,
  catchError,
  concatMap,
  filter,
  finalize,
  take,
  throwError,
} from "rxjs";
import { AuthService } from "./auth.service";
import { StorageService } from "./storage.service";

@Injectable()
export class AuthTokenInterceptor implements HttpInterceptor {
  private isRefreshingToken = false;
  private tokenRefreshed$ = new BehaviorSubject<boolean>(false);

  private readonly skipUrls = [
    `/Users/register`,
    `/Users/login`,
    `/Users/refresh`,
  ];

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private router: Router
  ) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(this.setAuthorizationHeader(req)).pipe(
      catchError((err: HttpErrorResponse) => {
        if (this.skipUrls.some((urls) => req.url.includes(urls))) {
          return next.handle(req);
        }

        if (err.status === 401) {
          return this.handle401Error(req, next);
        }
        return this.handleError(err);
      })
    );
  }

  private handle401Error(req: HttpRequest<unknown>, next: HttpHandler) {
    if (this.isRefreshingToken) {
      return this.tokenRefreshed$.pipe(
        filter(Boolean),
        take(1),
        concatMap(() => next.handle(this.setAuthorizationHeader(req)))
      );
    }

    this.isRefreshingToken = true;
    this.tokenRefreshed$.next(false);

    const refreshToken = this.storageService.getRefreshToken();
    return this.authService.getRefreshToken(refreshToken!).pipe(
      concatMap((tokens) => {
        if (tokens) {
          this.storageService.setAccessToken(tokens.accessToken);
          this.storageService.setRefreshToken(tokens.refreshToken);

          this.tokenRefreshed$.next(true);
          return next.handle(this.setAuthorizationHeader(req));
        }
        return this.handleError("Tokens not found");
      }),
      catchError((err) => {
        return this.handleError(err);
      }),
      finalize(() => {
        this.isRefreshingToken = false;
      })
    );
  }

  private handleError(err: unknown): Observable<never> {
    this.storageService.removeAuthorizationTokens();
    this.router.navigate(["login"]);
    return throwError(() => err);
  }

  private setAuthorizationHeader(
    req: HttpRequest<unknown>
  ): HttpRequest<unknown> {
    const accessToken = this.storageService.getAccessToken();
    return accessToken
      ? req.clone({
          headers: req.headers.set("Authorization", `Bearer ${accessToken}`),
        })
      : req;
  }
}
