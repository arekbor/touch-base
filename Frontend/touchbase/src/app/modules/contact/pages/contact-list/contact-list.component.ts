import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { handleHttpErrors } from "src/app/core/helpers/handle-http-errors";
import { Contact } from "src/app/core/models/contact.model";
import { PaginatedList } from "src/app/core/models/paginated-list";
import { ContactService } from "src/app/core/services/contact.service";

@Component({
  selector: "app-contact-list",
  templateUrl: "./contact-list.component.html",
})
export class ContactListComponent extends BaseComponent implements OnInit {
  protected contacts: PaginatedList<Contact>;
  protected errors: string[];

  protected pageNumber: number = 1;
  protected searchTerm?: string;

  private readonly maxPageSize = 10;

  constructor(private contactService: ContactService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.fetchContacts();
  }

  protected onSearchTermChange(event: Event) {
    this.searchTerm = (event.target as HTMLInputElement).value;

    this.fetchContacts();
  }

  protected onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;

    this.fetchContacts();
  }

  protected onAddContact(): void {
    this.router.navigate(["contact/create"]);
  }

  protected onDetails(id: string): void {
    this.router.navigate(["/contact/details", id]);
  }

  private fetchContacts() {
    this.safeSub(
      this.contactService
        .getContacts(this.pageNumber, this.maxPageSize, this.searchTerm)
        .subscribe({
          next: (contacts: PaginatedList<Contact>) => {
            this.contacts = contacts;
          },
          error: (err: HttpErrorResponse) => {
            this.errors = handleHttpErrors(err);
            throwError(() => err);
          },
        })
    );
  }
}
