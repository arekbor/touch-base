import { Component, OnInit } from "@angular/core";
import { StorageService } from "./core/services/storage.service";
import { UserService } from "./core/services/user.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent implements OnInit {
  isUserLogged = false;
  username = "";

  constructor(
    private userService: UserService,
    private storageService: StorageService
  ) {}

  ngOnInit(): void {
    this.initUser();
  }

  protected logout() {
    this.storageService.removeAuthorizationTokens();
    window.location.reload();
  }

  private initUser(): void {
    this.isUserLogged = this.userService.isLogged();

    const claims = this.userService.getUserClaims();
    if (claims) {
      this.username = claims.unique_name;
    }
  }
}
