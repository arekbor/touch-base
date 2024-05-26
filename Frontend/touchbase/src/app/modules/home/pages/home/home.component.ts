import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { ContactsInfo } from "src/app/core/models/contacts-info.model";
import { ContactService } from "src/app/core/services/contact.service";
import { BaseComponent } from "src/app/modules/base.component";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
})
export class HomeComponent extends BaseComponent implements OnInit {
  protected contactsInfo: ContactsInfo;
  protected errors: string[];

  constructor(private contactService: ContactService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.fetchContactsInfo();
  }

  protected onAddContact(): void {
    this.router.navigate(["contact/create"]);
  }

  protected onContactDetails(id: string): void {
    this.router.navigate(["/contact/details", id]);
  }

  private fetchContactsInfo() {
    this.contactService.getContactsInfo().subscribe({
      next: (contactsInfo: ContactsInfo) => {
        this.contactsInfo = contactsInfo;
      },
      error: (err: HttpErrorResponse) => {
        this.errors = this.handleHttpErrors(err);
        throwError(() => err);
      },
    });
  }
}
