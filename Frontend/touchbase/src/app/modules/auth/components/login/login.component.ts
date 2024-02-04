import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { catchError, map, of, throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { Tokens } from "src/app/core/models/tokens.model";
import { AuthService } from "src/app/core/services/auth.service";
import { StorageService } from "src/app/core/services/storage.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
})
export class LoginComponent extends BaseComponent implements OnInit {
  protected form: FormGroup;
  protected isUserLogged = false;
  protected isLoginFailed = false;
  protected username: string;
  protected errorDetail: string;
  protected errors: string[];

  constructor(
    private authService: AuthService,
    private storageService: StorageService
  ) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
    this.initUser();
  }

  protected onSubmit(): void {
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
              return of(null);
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
    this.isUserLogged = this.authService.isLogged();

    const claims = this.authService.getUserClaims();
    if (claims) {
      this.username = claims.unique_name;
    }
  }
}
