import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { EMPTY, catchError, map, throwError } from "rxjs";
import { BaseComponent } from "../common/base.component";
import { Tokens } from "../models/tokens.model";
import { AuthService } from "../services/auth.service";
import { StorageService } from "../services/storage.service";
import { UserService } from "../services/user.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
})
export class LoginComponent extends BaseComponent implements OnInit {
  form: FormGroup;

  isUserLogged = false;
  isLoginFailed = false;
  username: string;
  errorDetail: string;
  errors: string[];

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private storageService: StorageService
  ) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
    this.initUser();
  }

  onSubmit(): void {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }
    const formValues = this.form.getRawValue();

    this.safeSub(
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
            this.errorDetail = error.error.detail ?? error.statusText;
            this.errors = error.error.errors;
            this.isLoginFailed = true;

            return throwError(() => error);
          })
        )
        .subscribe()
    );
  }

  private initForm() {
    this.form = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", Validators.required),
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
