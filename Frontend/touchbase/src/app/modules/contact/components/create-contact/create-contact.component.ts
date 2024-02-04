import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ContactLabel } from "src/app/core/enums/contactLabel.enum";
import { ContactRelationship } from "src/app/core/enums/contactRelationship.enum";
import { PhoneValidator } from "src/app/shared/validators/phone.validator";

@Component({
  selector: "app-create-contact",
  templateUrl: "./create-contact.component.html",
})
export class CreateContactComponent implements OnInit {
  protected form: FormGroup;
  protected isLoading = false;
  protected contactLabel: typeof ContactLabel = ContactLabel;
  protected contactRelationship: typeof ContactRelationship =
    ContactRelationship;

  ngOnInit(): void {
    this.initForm();
  }

  protected onSubmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    const values = this.form.getRawValue();
  }

  private initForm() {
    this.form = new FormGroup({
      firstname: new FormControl("", Validators.required),
      surname: new FormControl("", Validators.required),
      company: new FormControl(""),
      phone: new FormControl("", PhoneValidator),
      label: new FormControl(this.contactLabel["No label"]),
      email: new FormControl("", [Validators.email, Validators.required]),
      birthday: new FormControl(""),
      relationship: new FormControl(this.contactRelationship["Assistant"]),
      notes: new FormControl("", Validators.maxLength(15)),
    });
  }
}
