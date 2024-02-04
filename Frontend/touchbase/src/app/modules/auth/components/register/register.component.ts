import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { catchError, map, throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { AuthService } from "src/app/core/services/auth.service";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
})
export class RegisterComponent extends BaseComponent implements OnInit {
  protected form: FormGroup;
  protected errorDetail: string;
  protected errors: string[];
  protected isRegisterFailed = false;
  protected isRegistred = false;

  constructor(private authService: AuthService) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
  }

  protected onSubmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }
    const formValues = this.form.getRawValue();

    this.safeSub(
      this.authService
        .register(formValues.username, formValues.email, formValues.password)
        .pipe(
          map(() => {
            this.isRegisterFailed = false;
            this.isRegistred = true;
          }),
          catchError((error: HttpErrorResponse) => {
            this.errorDetail = error.error.detail ?? error.statusText;
            this.errors = error.error.errors;
            this.isRegistred = false;
            this.isRegisterFailed = true;

            return throwError(() => error);
          })
        )
        .subscribe()
    );
  }

  private initForm() {
    this.form = new FormGroup({
      username: new FormControl("", Validators.required),
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [
        Validators.required,
        Validators.minLength(8),
      ]),
    });
  }
}
