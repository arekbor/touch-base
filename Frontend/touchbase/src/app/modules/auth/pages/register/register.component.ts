import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { Register } from "src/app/core/models/register.model";
import { AuthService } from "src/app/core/services/auth.service";
import { FormGroupControl } from "src/app/core/utils/form-group-control";
import { BaseComponent } from "src/app/modules/base.component";
import { CustomValidators } from "src/app/shared/validators/custom-validators";
import { GroupValidators } from "src/app/shared/validators/group-validators";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
})
export class RegisterComponent extends BaseComponent implements OnInit {
  protected form: FormGroup<FormGroupControl<Register>>;
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
          this.errors = this.handleHttpErrors(err);
          throwError(() => err);
        },
      })
    );
  }

  private initForm() {
    this.form = new FormGroup<FormGroupControl<Register>>({
      username: new FormControl("", {
        nonNullable: true,
        validators: GroupValidators.username(),
      }),

      email: new FormControl("", {
        nonNullable: true,
        validators: GroupValidators.email(),
      }),

      password: new FormControl("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(40),
          CustomValidators.containsLowercase(),
          CustomValidators.containNumber(),
          CustomValidators.containsUppercase(),
          CustomValidators.containsSpecialChar(),
        ],
      }),
    });
  }
}
