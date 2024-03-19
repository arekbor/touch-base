import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { throwError } from "rxjs";
import { BaseFormComponent } from "src/app/core/helpers/baseForm.component";
import { ControlsOf } from "src/app/core/helpers/controlsOf";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { Login } from "src/app/core/models/login.model";
import { Tokens } from "src/app/core/models/tokens.model";
import { AuthService } from "src/app/core/services/auth.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
})
export class LoginComponent extends BaseFormComponent implements OnInit {
  protected form: FormGroup<ControlsOf<Login>>;
  protected errors: string[];

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

    const rawValue = this.form.getRawValue();

    this.safeSub(
      this.authService.login(rawValue).subscribe({
        next: (tokens: Tokens | null) => {
          if (tokens) {
            this.authService.setAuthTokens(tokens);
            window.location.reload();
            return;
          }

          throwError(() => "Tokens not found");
        },
        error: (err: HttpErrorResponse) => {
          this.errors = handleErrors(err);
          throwError(() => err);
        },
      })
    );
  }

  private initForm() {
    this.form = new FormGroup<ControlsOf<Login>>({
      email: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required, Validators.email],
      }),

      password: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required],
      }),
    });
  }
}
