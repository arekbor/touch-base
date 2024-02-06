import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { JWT_OPTIONS, JwtHelperService } from "@auth0/angular-jwt";
import { SharedModule } from "../shared/shared.module";
import { AuthTokenInterceptor } from "./interceptors/auth-token.interceptor";

const HttpInterceptors = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthTokenInterceptor, multi: true },
];

@NgModule({
  imports: [SharedModule],
  providers: [
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    HttpInterceptors,
  ],
})
export class CoreModule {}
