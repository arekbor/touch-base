import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { throwError } from "rxjs";
import { Login } from "src/app/core/models/login.model";
import { Tokens } from "src/app/core/models/tokens.model";
import { AuthService } from "src/app/core/services/auth.service";
import { StorageService } from "src/app/core/services/storage.service";
import { FormGroupControl } from "src/app/core/utils/form-group-control";
import { BaseComponent } from "src/app/modules/base.component";
import { GroupValidators } from "src/app/shared/validators/group-validators";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
})
export class LoginComponent extends BaseComponent implements OnInit {
  protected form: FormGroup<FormGroupControl<Login>>;

  constructor(
    private authService: AuthService,
    private storageService: StorageService
  ) {
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
            this.storageService.setAuthorizationTokens(tokens);
            window.location.reload();
            return;
          }

          throwError(() => "Tokens not found");
        },
        error: (err: HttpErrorResponse) => {
          this.handleHttpErrors(err);
        },
      })
    );
  }

  private initForm() {
    this.form = new FormGroup<FormGroupControl<Login>>({
      email: new FormControl("", {
        nonNullable: true,
        validators: GroupValidators.email(),
      }),

      password: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required],
      }),
    });
  }
}
