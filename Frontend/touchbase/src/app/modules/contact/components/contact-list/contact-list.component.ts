import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { Contact } from "src/app/core/models/contact.model";
import { PaginatedList } from "src/app/core/models/paginatedList";
import { ContactService } from "src/app/core/services/contact.service";

@Component({
  selector: "app-contact-list",
  templateUrl: "./contact-list.component.html",
})
export class ContactListComponent extends BaseComponent implements OnInit {
  protected contacts: PaginatedList<Contact>;
  private pageNumber = 1;
  private pageSize = 5;

  constructor(private contactService: ContactService, private router: Router) {
    super();
  }
  ngOnInit(): void {
    this.fetchContacts();
  }

  onPrevious(): void {
    this.pageNumber--;
    this.fetchContacts();
  }

  onNext(): void {
    this.pageNumber++;
    this.fetchContacts();
  }

  onPageSizeChange(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.pageSize = parseInt(value);
    this.fetchContacts();
  }

  onDetails(id: string): void {}

  onEdit(id: string): void {}

  onDelete(id: string): void {}

  onCreate(): void {
    this.router.navigate(["contact/create"]);
  }

  private fetchContacts() {
    this.contactService.getContacts(this.pageNumber, this.pageSize).subscribe({
      next: (contacts: PaginatedList<Contact>) => {
        this.contacts = contacts;
      },
    });
  }
}
