import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import {
  ContactLabel,
  ContactLabelMap,
} from "src/app/core/enums/contact-label.enum";
import {
  ContactRelationship,
  ContactRelationshipMap,
} from "src/app/core/enums/contact-relationship.enum";
import { BaseFormComponent } from "src/app/core/helpers/base-form.component";
import { FormGroupControl } from "src/app/core/helpers/form-group-control";
import { ContactForm } from "src/app/core/models/contact-form.model";
import { DatebirthValidator } from "src/app/shared/validators/datebirth.validator";
import { NameValidator } from "src/app/shared/validators/name.validator";
import { PhoneValidator } from "src/app/shared/validators/phone.validator";

@Component({
  selector: "app-contact-form",
  templateUrl: "./contact-form.component.html",
})
export class ContactFormComponent extends BaseFormComponent implements OnInit {
  @Input() contactForm?: ContactForm;
  @Output() contactFormChange: EventEmitter<ContactForm> =
    new EventEmitter<ContactForm>();

  protected ContactLabelMap = ContactLabelMap;
  protected contactLabels = Object.values(ContactLabel).filter(
    (x) => typeof x === "number"
  ) as ContactLabel[];

  protected ContactRelationshipMap = ContactRelationshipMap;
  protected contactRelationships = Object.values(ContactRelationship).filter(
    (x) => typeof x === "number"
  ) as ContactRelationship[];

  protected form: FormGroup<FormGroupControl<ContactForm>>;

  ngOnInit(): void {
    this.initForm();
  }

  protected onSbmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    this.contactFormChange.emit(this.form.getRawValue());
  }

  private initForm() {
    this.form = new FormGroup<FormGroupControl<ContactForm>>({
      firstname: new FormControl(this.contactForm?.firstname ?? "", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.maxLength(40),
          NameValidator,
        ],
      }),

      surname: new FormControl(this.contactForm?.surname ?? "", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.maxLength(40),
          NameValidator,
        ],
      }),

      company: new FormControl(
        this.contactForm?.company ?? "",
        Validators.maxLength(40)
      ),

      phone: new FormControl(this.contactForm?.phone ?? "", PhoneValidator),

      label: new FormControl(this.contactForm?.label ?? ContactLabel.NoLabel, {
        nonNullable: true,
      }),

      email: new FormControl(this.contactForm?.email ?? "", {
        nonNullable: true,
        validators: [
          Validators.email,
          Validators.required,
          Validators.maxLength(40),
        ],
      }),

      birthday: new FormControl(
        this.contactForm?.birthday ?? null,
        DatebirthValidator
      ),

      relationship: new FormControl(
        this.contactForm?.relationship ?? ContactRelationship.NoRelation,
        {
          nonNullable: true,
        }
      ),

      notes: new FormControl(
        this.contactForm?.notes ?? "",
        Validators.maxLength(15)
      ),
    });
  }
}
