import { HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import {
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from "@angular/forms";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { ContactLabel } from "src/app/core/enums/contactLabel.enum";
import { ContactRelationship } from "src/app/core/enums/contactRelationship.enum";
import { BaseComponent } from "src/app/core/helpers/base.component";
import { ControlsOf } from "src/app/core/helpers/controlsOf";
import { handleErrors } from "src/app/core/helpers/handleErrors";
import { CreateContact } from "src/app/core/models/createContact.model";
import { ContactService } from "src/app/core/services/contact.service";
import { DatebirthValidator } from "src/app/shared/validators/datebirth.validator";
import { PhoneValidator } from "src/app/shared/validators/phone.validator";

@Component({
  selector: "app-create-contact",
  templateUrl: "./create-contact.component.html",
})
export class CreateContactComponent extends BaseComponent implements OnInit {
  protected form: FormGroup<ControlsOf<CreateContact>>;
  protected isLoading = false;
  protected errors: string[];

  protected contactLabel: typeof ContactLabel = ContactLabel;
  protected contactRelationship: typeof ContactRelationship =
    ContactRelationship;

  constructor(private contactService: ContactService, private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.initForm();
  }

  protected onSubmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    const createContact = this.form.getRawValue();
    this.isLoading = true;

    this.safeSub(
      this.contactService.create(createContact).subscribe({
        next: () => {
          this.router.navigate(["contact/list"]);
        },
        error: (err: HttpErrorResponse) => {
          this.isLoading = false;
          this.errors = handleErrors(err);
          throwError(() => err);
        },
      })
    );
  }

  protected getFieldErrors(field: string): ValidationErrors | null {
    const control = this.form.get(field);
    if (control && control.invalid && (control.dirty || control.touched)) {
      return control.errors;
    }
    return null;
  }

  private initForm() {
    this.form = new FormGroup<ControlsOf<CreateContact>>({
      firstname: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required],
      }),
      surname: new FormControl("", {
        nonNullable: true,
        validators: [Validators.required],
      }),
      company: new FormControl(""),
      phone: new FormControl("", PhoneValidator),
      label: new FormControl(0, {
        nonNullable: true,
      }),
      email: new FormControl("", {
        nonNullable: true,
        validators: [Validators.email, Validators.required],
      }),
      birthday: new FormControl(null, DatebirthValidator),
      relationship: new FormControl(0, {
        nonNullable: true,
      }),
      notes: new FormControl("", Validators.maxLength(15)),
    });
  }
}
