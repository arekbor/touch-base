import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import {
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from "@angular/forms";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { AuthService } from "src/app/core/services/auth.service";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
})
export class RegisterComponent extends BaseComponent implements OnInit {
  protected form: FormGroup;
  protected isLoading = false;
  protected errors: string[];

  constructor(private authService: AuthService, private router: Router) {
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

    const value = this.form.getRawValue();
    this.isLoading = true;
    this.safeSub(
      this.authService
        .register(value.username, value.email, value.password)
        .subscribe({
          next: () => {
            this.router.navigate(["auth/login"]);
          },
          error: (err: HttpErrorResponse) => {
            this.isLoading = false;
            this.errors = handleErrors(err);
            throwError(() => err);
          },
        })
    );
  }

  protected getFieldErrors(field: string): ValidationErrors | null {
    const control = this.form.get(field);
    if (control && control.invalid && (control.dirty || control.touched)) {
      return control.errors;
    }
    return null;
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
