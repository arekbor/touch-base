import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { switchMap } from "rxjs";
import { User } from "src/app/core/models/user.models";
import { AuthService } from "src/app/core/services/auth.service";
import { UserService } from "src/app/core/services/user.service";
import { FormGroupControl } from "src/app/core/utils/form-group-control";
import { BaseComponent } from "src/app/modules/base.component";
import { GroupValidators } from "src/app/shared/validators/group-validators";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
})
export class ProfileComponent extends BaseComponent implements OnInit {
  protected user: User;

  protected form: FormGroup<FormGroupControl<User>>;

  constructor(
    private userService: UserService,
    private authService: AuthService
  ) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
    this.initUser();
  }

  protected onUpdateProfile() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    this.safeSub(
      this.userService
        .updateProfile(this.form.getRawValue())
        .pipe(switchMap(() => this.authService.reloadTokens()))
        .subscribe({
          next: () => {
            window.location.reload();
          },
          error: (err: HttpErrorResponse) => {
            this.handleHttpErrors(err);
          },
        })
    );
  }

  private initUser(): void {
    this.safeSub(
      this.userService.getUser().subscribe({
        next: (user: User | null) => {
          if (user) {
            this.user = user;
            this.updateForm(this.user);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.handleHttpErrors(err);
        },
      })
    );
  }

  private initForm() {
    this.form = new FormGroup<FormGroupControl<User>>({
      username: new FormControl("", {
        nonNullable: true,
        validators: GroupValidators.username(),
      }),
      email: new FormControl("", {
        nonNullable: true,
        validators: GroupValidators.email(),
      }),
    });
  }

  private updateForm(user: User) {
    this.form.setValue({ username: user.username, email: user.email });
  }
}
