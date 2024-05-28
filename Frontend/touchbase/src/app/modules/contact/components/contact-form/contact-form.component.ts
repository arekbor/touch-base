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
import { ContactBody } from "src/app/core/models/contact-body.model";
import { FormGroupControl } from "src/app/core/utils/form-group-control";
import { BaseComponent } from "src/app/modules/base.component";
import { CustomValidators } from "src/app/shared/validators/custom-validators";
import { GroupValidators } from "src/app/shared/validators/group-validators";

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
        validators: GroupValidators.personName(),
      }),

      lastname: new FormControl(this.contactBody?.lastname ?? "", {
        nonNullable: true,
        validators: GroupValidators.personName(),
      }),

      company: new FormControl(
        this.contactBody?.company ?? "",
        Validators.maxLength(40)
      ),

      phone: new FormControl(
        this.contactBody?.phone ?? "",
        CustomValidators.phone()
      ),

      label: new FormControl(this.contactBody?.label ?? ContactLabel.NoLabel, {
        nonNullable: true,
      }),

      email: new FormControl(this.contactBody?.email ?? "", {
        nonNullable: true,
        validators: GroupValidators.email(),
      }),

      birthday: new FormControl(
        this.contactBody?.birthday ?? null,
        CustomValidators.datebirth()
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
