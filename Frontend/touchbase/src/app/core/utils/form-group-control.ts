import { FormControl, FormGroup } from "@angular/forms";

export type FormGroupControl<T extends Record<string, any>> = {
  [K in keyof T]: T[K] extends Record<any, any>
    ? FormGroup<FormGroupControl<T[K]>>
    : FormControl<T[K]>;
};
