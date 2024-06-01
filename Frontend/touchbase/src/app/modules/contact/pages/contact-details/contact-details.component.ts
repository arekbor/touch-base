import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { switchMap } from "rxjs";
import { ContactBody } from "src/app/core/models/contact-body.model";
import { ContactDetails } from "src/app/core/models/contact-details.model";
import { ContactService } from "src/app/core/services/contact.service";
import { BaseComponent } from "src/app/modules/base.component";

@Component({
  selector: "app-contact-details",
  templateUrl: "./contact-details.component.html",
})
export class ContactDetailsComponent extends BaseComponent implements OnInit {
  protected contactId: string;

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
          this.handleHttpErrors(err);
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
          this.handleHttpErrors(err);
        },
      })
    );
  }

  private fetchContact(): void {
    this.activatedRoute.params
      .pipe(
        switchMap((params: Params) =>
          this.contactService.getContact(params["id"])
        )
      )
      .subscribe({
        next: (contact: ContactDetails) => {
          this.contactId = contact.id;

          this.contactBody = {
            firstname: contact.firstname,
            lastname: contact.lastname,
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
          this.handleHttpErrors(err);
        },
      });
  }
}
