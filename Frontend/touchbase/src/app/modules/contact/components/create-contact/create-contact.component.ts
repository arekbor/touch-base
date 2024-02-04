import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { PhoneValidator } from "src/app/core/validators/phone.validator";

@Component({
  selector: "app-create-contact",
  templateUrl: "./create-contact.component.html",
})
export class CreateContactComponent implements OnInit {
  protected form: FormGroup;

  ngOnInit(): void {
    this.initForm();
  }

  protected onSubmit() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }
  }

  private initForm() {
    this.form = new FormGroup({
      firstname: new FormControl("", Validators.required),
      surname: new FormControl("", Validators.required),
      company: new FormControl(""),
      phone: new FormControl("", PhoneValidator),
      label: new FormControl(""),
      email: new FormControl("", [Validators.email, Validators.required]),
      birthday: new FormControl(""),
      relationship: new FormControl(""),
      notes: new FormControl("", Validators.maxLength(15)),
    });
  }
}
