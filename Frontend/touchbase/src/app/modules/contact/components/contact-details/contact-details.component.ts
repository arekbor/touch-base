import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { ContactDetails } from "src/app/core/models/contact-details.model";
import { ContactService } from "src/app/core/services/contact.service";

@Component({
  selector: "app-contact-details",
  templateUrl: "./contact-details.component.html",
})
export class ContactDetailsComponent extends BaseComponent implements OnInit {
  protected contact: ContactDetails;
  private contactId: string;

  protected isLoading = false;
  protected errors: string[];

  constructor(
    private contactService: ContactService,
    private activatedRoute: ActivatedRoute
  ) {
    super();
  }

  ngOnInit(): void {
    this.fetchContact();
  }

  private fetchContact(): void {
    this.isLoading = true;
    this.safeSub(
      this.activatedRoute.params.subscribe((params) => {
        this.contactId = params["id"];
      }),

      this.contactService.getContact(this.contactId).subscribe({
        next: (contact: ContactDetails) => {
          this.contact = contact;
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          this.isLoading = false;
          this.errors = handleErrors(err);
          throwError(() => err);
        },
      })
    );
  }
}
