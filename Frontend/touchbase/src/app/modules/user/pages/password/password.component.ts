import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Password } from "src/app/core/models/password.model";
import { AuthService } from "src/app/core/services/auth.service";
import { UserService } from "src/app/core/services/user.service";
import { FormGroupControl } from "src/app/core/utils/form-group-control";
import { BaseComponent } from "src/app/modules/base.component";
import { GroupValidators } from "src/app/shared/validators/group-validators";

@Component({
  selector: "app-password",
  templateUrl: "./password.component.html",
})
export class PasswordComponent extends BaseComponent implements OnInit {
  protected form: FormGroup<FormGroupControl<Password>>;

  constructor(
    private userService: UserService,
    private authService: AuthService
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

    this.safeSub(
      this.userService.updatePassword(this.form.getRawValue()).subscribe({
        next: () => {
          this.authService.logout();
        },
        error: (err: HttpErrorResponse) => {
          this.handleHttpErrors(err);
        },
      })
    );
  }

  private initForm() {
    this.form = new FormGroup<FormGroupControl<Password>>({
      oldPassword: new FormControl("", {
        nonNullable: true,
        validators: Validators.required,
      }),
      newPassword: new FormControl("", {
        nonNullable: true,
        validators: GroupValidators.password(),
      }),
    });
  }
}
