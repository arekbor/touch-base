import { Component, OnInit } from "@angular/core";
import { StorageService } from "./services/storage.service";
import { UserService } from "./services/user.service";

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
  
  logout() {
    this.storageService.removeAuthorizationTokens();
    window.location.reload();
  }

  private initUser(): void {
    const claims = this.userService.getUserClaims();
    if (claims) {
      this.isUserLogged = !!claims;
      this.username = claims.unique_name;
    }
  }
}
