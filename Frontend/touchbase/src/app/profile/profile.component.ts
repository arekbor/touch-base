import { Component, OnInit } from "@angular/core";
import { User } from "../models/user.models";
import { Secret, UserService } from "../services/user.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
})
export class ProfileComponent implements OnInit {
  user: User;
  secret: Secret;
  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.initUser();
  }

  private initUser(): void {
    this.userService.getUser().subscribe((user: User | null) => {
      if (user) {
        this.user = user;
      }
    });

    this.userService.getSecret().subscribe((secret: Secret | null) => {
      if (secret) {
        this.secret = secret;
      }
    });
  }
}
