import { Component, OnInit } from "@angular/core";
import { AuthService } from "./core/services/auth.service";
import { LoadingService } from "./core/services/loading.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent implements OnInit {
  protected isUserLogged = false;
  protected loading = false;
  protected username = "";

  constructor(
    private authService: AuthService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.initUser();
    this.loadingListener();
  }
  
  private loadingListener(): void {
    this.loadingService.loadingSub.subscribe((loading) => {
      this.loading = loading;
    });
  }

  private initUser(): void {
    this.isUserLogged = this.authService.isLogged();

    const claims = this.authService.getUserClaims();
    if (claims) {
      this.username = claims.unique_name;
    }
  }
}
