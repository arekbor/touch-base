import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";

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
      firstname: new FormControl(""),
      surname: new FormControl(""),
      company: new FormControl(""),
      phone: new FormControl(""),
      label: new FormControl(""),
      email: new FormControl("", [Validators.email, Validators.required]),
      birthday: new FormControl(""),
      relationship: new FormControl(""),
      notes: new FormControl(""),
    });
  }
}
