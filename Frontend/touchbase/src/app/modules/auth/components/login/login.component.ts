import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import {
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from "@angular/forms";
import { throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { Tokens } from "src/app/core/models/tokens.model";
import { AuthService } from "src/app/core/services/auth.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
})
export class LoginComponent extends BaseComponent implements OnInit {
  protected form: FormGroup;
  protected isLoading = false;
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

    const value = this.form.getRawValue();
    this.isLoading = true;
    this.safeSub(
      this.authService.login(value.email, value.password).subscribe({
        next: (tokens: Tokens | null) => {
          if (tokens) {
            this.authService.setAuthTokens(tokens);
            window.location.reload();
            return;
          }

          this.isLoading = false;
          throwError(() => "Tokens not found");
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
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", Validators.required),
    });
  }
}
