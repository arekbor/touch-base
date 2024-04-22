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
import { BaseComponent } from "src/app/core/helpers/base.component";
import { FormGroupControl } from "src/app/core/helpers/form-group-control";
import { ContactBody } from "src/app/core/models/contact-body.model";
import { DatebirthValidator } from "src/app/shared/validators/datebirth.validator";
import { NameValidator } from "src/app/shared/validators/name.validator";
import { PhoneValidator } from "src/app/shared/validators/phone.validator";

@Component({
  selector: "app-contact-form",
  templateUrl: "./contact-form.component.html",
})
export class ContactFormComponent extends BaseComponent implements OnInit {
  @Input() contactBody?: ContactBody;
  @Output() contactBodyChange: EventEmitter<ContactBody> =
    new EventEmitter<ContactBody>();

  protected ContactLabelMap = ContactLabelMap;
  protected contactLabels = Object.values(ContactLabel).filter(
    (x) => typeof x === "number"
  ) as ContactLabel[];

  protected ContactRelationshipMap = ContactRelationshipMap;
  protected contactRelationships = Object.values(ContactRelationship).filter(
    (x) => typeof x === "number"
  ) as ContactRelationship[];

  protected form: FormGroup<FormGroupControl<ContactBody>>;

  ngOnInit(): void {
    this.initForm();
  }

  protected onSbmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    this.contactBodyChange.emit(this.form.getRawValue());
  }

  private initForm() {
    this.form = new FormGroup<FormGroupControl<ContactBody>>({
      firstname: new FormControl(this.contactBody?.firstname ?? "", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.maxLength(40),
          NameValidator,
        ],
      }),

      lastname: new FormControl(this.contactBody?.lastname ?? "", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.maxLength(40),
          NameValidator,
        ],
      }),

      company: new FormControl(
        this.contactBody?.company ?? "",
        Validators.maxLength(40)
      ),

      phone: new FormControl(this.contactBody?.phone ?? "", PhoneValidator),

      label: new FormControl(this.contactBody?.label ?? ContactLabel.NoLabel, {
        nonNullable: true,
      }),

      email: new FormControl(this.contactBody?.email ?? "", {
        nonNullable: true,
        validators: [
          Validators.email,
          Validators.required,
          Validators.maxLength(40),
        ],
      }),

      birthday: new FormControl(
        this.contactBody?.birthday ?? null,
        DatebirthValidator
      ),

      relationship: new FormControl(
        this.contactBody?.relationship ?? ContactRelationship.NoRelation,
        {
          nonNullable: true,
        }
      ),

      notes: new FormControl(
        this.contactBody?.notes ?? "",
        Validators.maxLength(15)
      ),
    });
  }
}
