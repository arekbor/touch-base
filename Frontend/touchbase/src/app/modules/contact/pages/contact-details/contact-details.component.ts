import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleHttpErrors } from "src/app/core/helpers/handle-http-errors";
import { ContactBody } from "src/app/core/models/contact-body.model";
import { ContactDetails } from "src/app/core/models/contact-details.model";
import { ContactService } from "src/app/core/services/contact.service";

@Component({
  selector: "app-contact-details",
  templateUrl: "./contact-details.component.html",
})
export class ContactDetailsComponent extends BaseComponent implements OnInit {
  private contactId: string;
  protected errors: string[];

  protected contactBody: ContactBody;

  constructor(
    private contactService: ContactService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    super();
  }

  ngOnInit(): void {
    this.fetchContact();
  }

  protected onList(): void {
    this.router.navigate(["contact/list"]);
  }

  protected onContactBodyChange(contactBody: ContactBody) {
    this.safeSub(
      this.contactService.update(contactBody, this.contactId).subscribe({
        next: () => {
          this.router.navigate(["contact/list"]);
        },
        error: (err: HttpErrorResponse) => {
          this.errors = handleHttpErrors(err);
          throwError(() => err);
        },
      })
    );
  }

  protected onDelete(): void {
    this.safeSub(
      this.contactService.deleteContact(this.contactId).subscribe({
        next: () => {
          this.router.navigate(["contact/list"]);
        },
        error: (err: HttpErrorResponse) => {
          this.errors = handleHttpErrors(err);
          throwError(() => err);
        },
      })
    );
  }

  private fetchContact(): void {
    this.safeSub(
      this.activatedRoute.params.subscribe((params) => {
        this.contactId = params["id"];
      }),

      this.contactService.getContact(this.contactId).subscribe({
        next: (contact: ContactDetails) => {
          this.contactBody = {
            firstname: contact.firstname,
            surname: contact.surname,
            company: contact.company,
            phone: contact.phone,
            label: contact.label,
            email: contact.email,
            birthday: contact.birthday,
            relationship: contact.relationship,
            notes: contact.notes,
          };
        },
        error: (err: HttpErrorResponse) => {
          this.errors = handleHttpErrors(err);
          throwError(() => err);
        },
      })
    );
  }
}
