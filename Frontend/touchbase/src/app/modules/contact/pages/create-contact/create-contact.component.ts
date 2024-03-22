import { HttpErrorResponse } from "@angular/common/http";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { ContactLabel } from "src/app/core/enums/contact-label.enum";
import { ContactRelationship } from "src/app/core/enums/contact-relationship.enum";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleHttpErrors } from "src/app/core/helpers/handle-http-errors";
import { ContactForm } from "src/app/core/models/contact-form.model";
import { ContactService } from "src/app/core/services/contact.service";

@Component({
  selector: "app-create-contact",
  templateUrl: "./create-contact.component.html",
})
export class CreateContactComponent extends BaseComponent {
  protected errors: string[];

  protected contactLabel: typeof ContactLabel = ContactLabel;
  protected contactRelationship: typeof ContactRelationship =
    ContactRelationship;

  constructor(private contactService: ContactService, private router: Router) {
    super();
  }

  protected onContactFormChange(contactForm: ContactForm) {
    this.safeSub(
      this.contactService.create(contactForm).subscribe({
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
}
