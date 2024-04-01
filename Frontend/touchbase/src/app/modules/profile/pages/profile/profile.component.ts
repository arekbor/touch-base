import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { User } from "src/app/core/models/user.models";
import { AuthService } from "src/app/core/services/auth.service";
import { UserService } from "src/app/core/services/user.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
})
export class ProfileComponent extends BaseComponent implements OnInit {
  protected user: User;
  constructor(
    private userService: UserService,
    private authService: AuthService
  ) {
    super();
  }

  ngOnInit(): void {
    this.initUser();
  }

  protected logout() {
    this.authService.logout();
  }

  private initUser(): void {
    this.safeSub(
      this.userService.getUser().subscribe((user: User | null) => {
        if (user) {
          this.user = user;
        }
      })
    );
  }
}
