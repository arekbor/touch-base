import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { EMPTY, catchError, map, throwError } from "rxjs";
import { Tokens } from "../models/tokens.model";
import { AuthService } from "../services/auth.service";
import { StorageService } from "../services/storage.service";
import { UserService } from "../services/user.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
})
export class LoginComponent implements OnInit {
  form: any;

  isUserLogged = false;
  isLoginFailed = false;
  username: string;
  errorDetail: string;
  errors: string[];

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private storageService: StorageService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.initUser();
  }

  onSubmit(): void {
    const formValues = this.form.getRawValue();

    this.authService
      .login(formValues.email, formValues.password)
      .pipe(
        map((tokens: Tokens | null) => {
          if (tokens) {
            this.storageService.setAccessToken(tokens.accessToken);
            this.storageService.setRefreshToken(tokens.refreshToken);

            this.isLoginFailed = false;
            window.location.reload();
            return EMPTY;
          }
          this.errorDetail = "Internal server error";
          this.isLoginFailed = true;

          return throwError(() => "Tokens not found");
        }),
        catchError((error: HttpErrorResponse) => {
          console.log("debug login");
          this.errorDetail = error.error.detail ?? error.statusText;
          this.errors = error.error.errors;
          this.isLoginFailed = true;

          return throwError(() => error);
        })
      )
      .subscribe();
  }

  getControlErrors(controlName: string) {
    const control = this.form.get(controlName);
    if (control && control.invalid && (control.dirty || control.touched)) {
      return control.errors;
    }

    return null;
  }

  private initForm() {
    this.form = new FormGroup({
      email: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required],
      }),
      password: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required],
      }),
    });
  }

  private initUser(): void {
    const claims = this.userService.getUserClaims();
    if (claims) {
      this.isUserLogged = !!claims;
      this.username = claims.unique_name;
    }
  }
}
