import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { JWT_OPTIONS, JwtHelperService } from "@auth0/angular-jwt";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { LoginComponent } from "./login/login.component";
import { ProfileComponent } from "./profile/profile.component";
import { RegisterComponent } from "./register/register.component";
import { AuthTokenInterceptor } from "./services/auth-token.interceptor";
import { AuthService } from "./services/auth.service";
import { StorageService } from "./services/storage.service";
import { UserService } from "./services/user.service";

const HttpInterceptors = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthTokenInterceptor, multi: true },
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProfileComponent,
    RegisterComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
  ],
  providers: [
    AuthService,
    StorageService,
    UserService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    HttpInterceptors,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
