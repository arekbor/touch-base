import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpStatusCode,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import {
  BehaviorSubject,
  Observable,
  catchError,
  filter,
  finalize,
  switchMap,
  take,
  throwError,
} from "rxjs";
import { AuthService } from "../services/auth.service";
import { StorageService } from "../services/storage.service";

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
    private storageService: StorageService
  ) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.skipUrls.some((urls) => req.url.includes(urls))) {
      return next.handle(req);
    }

    return next.handle(this.setAuthorizationHeader(req)).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === HttpStatusCode.Unauthorized) {
          return this.handleUnauthorizedError(req, next);
        }
        return throwError(() => err);
      })
    );
  }

  private handleUnauthorizedError(
    req: HttpRequest<unknown>,
    next: HttpHandler
  ) {
    if (this.isRefreshingToken) {
      return this.tokenRefreshed$.pipe(
        filter(Boolean),
        take(1),
        switchMap(() => next.handle(this.setAuthorizationHeader(req)))
      );
    }

    this.isRefreshingToken = true;
    this.tokenRefreshed$.next(false);

    return this.authService.reloadTokens().pipe(
      switchMap(() => {
        this.tokenRefreshed$.next(true);
        return next.handle(this.setAuthorizationHeader(req));
      }),
      catchError((err) => {
        return this.handleError(err);
      }),
      finalize(() => {
        this.isRefreshingToken = false;
      })
    );
  }

  private handleError(err: string | HttpErrorResponse): Observable<never> {
    if (
      err instanceof HttpErrorResponse &&
      err.status === HttpStatusCode.Unauthorized
    ) {
      this.authService.logout();
    }
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
