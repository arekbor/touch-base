import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { Contact } from "src/app/core/models/contact.model";
import { PaginatedList } from "src/app/core/models/paginatedList";
import { ContactService } from "src/app/core/services/contact.service";

@Component({
  selector: "app-contact-list",
  templateUrl: "./contact-list.component.html",
})
export class ContactListComponent extends BaseComponent implements OnInit {
  protected contacts: PaginatedList<Contact>;
  protected errors: string[];
  protected isLoading = false;

  constructor(private contactService: ContactService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.fetchContacts(1);
  }

  protected onPageChange(page: number) {
    this.fetchContacts(page);
  }

  protected onCreate(): void {
    this.router.navigate(["contact/create"]);
  }

  protected onDetails(id: string): void {
    this.router.navigate(["/contact/details", id]);
  }

  protected onEdit(id: string): void {}

  protected onDelete(id: string): void {}

  private fetchContacts(pageNumber: number) {
    this.isLoading = true;
    this.safeSub(
      this.contactService.getContacts(pageNumber, 10).subscribe({
        next: (contacts: PaginatedList<Contact>) => {
          this.contacts = contacts;
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
