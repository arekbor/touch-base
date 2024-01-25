import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../common/base.component";
import { User } from "../models/user.models";
import {
  CustomData,
  FakeData,
  Secret,
  UserService,
} from "../services/user.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
})
export class ProfileComponent extends BaseComponent implements OnInit {
  user: User;
  secret: Secret;
  fakeData: FakeData[];
  customData: CustomData;
  constructor(private userService: UserService) {
    super();
  }

  ngOnInit(): void {
    this.initUser();
  }

  private initUser(): void {
    this.safeSub(
      this.userService.getUser().subscribe((user: User | null) => {
        if (user) {
          this.user = user;
        }
      }),

      this.userService.getSecret().subscribe((secret: Secret | null) => {
        if (secret) {
          this.secret = secret;
        }
      }),

      this.userService
        .getCustomData()
        .subscribe((customData: CustomData | null) => {
          if (customData) {
            this.customData = customData;
          }
        }),

      this.userService
        .getFakeData()
        .subscribe((fakeData: FakeData[] | null) => {
          if (fakeData) {
            this.fakeData = fakeData;
          }
        })
    );
  }
}
