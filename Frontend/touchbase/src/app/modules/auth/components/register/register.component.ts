import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { BaseFormComponent } from "src/app/core/helpers/baseForm.component";
import { ControlsOf } from "src/app/core/helpers/controlsOf";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { Register } from "src/app/core/models/register.model";
import { AuthService } from "src/app/core/services/auth.service";
import { PasswordValidator } from "src/app/shared/validators/password.validator";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
})
export class RegisterComponent extends BaseFormComponent implements OnInit {
  protected form: FormGroup<ControlsOf<Register>>;
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

    const rawValue = this.form.getRawValue();

    this.safeSub(
      this.authService.register(rawValue).subscribe({
        next: () => {
          this.router.navigate(["auth/login"]);
        },
        error: (err: HttpErrorResponse) => {
          this.errors = handleErrors(err);
          throwError(() => err);
        },
      })
    );
  }

  private initForm() {
    this.form = new FormGroup<ControlsOf<Register>>({
      username: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required, Validators.maxLength(40)],
      }),

      email: new FormControl("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.email,
          Validators.maxLength(40),
        ],
      }),

      password: new FormControl("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(40),
          PasswordValidator,
        ],
      }),
    });
  }
}
