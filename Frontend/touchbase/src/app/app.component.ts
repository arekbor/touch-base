import { Component, OnInit } from "@angular/core";
import { AuthService } from "./core/services/auth.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent implements OnInit {
  isUserLogged = false;
  username = "";

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.initUser();
  }

  protected logout() {
    this.authService.logout();
  }

  private initUser(): void {
    this.isUserLogged = this.authService.isLogged();

    const claims = this.authService.getUserClaims();
    if (claims) {
      this.username = claims.unique_name;
    }
  }
}
