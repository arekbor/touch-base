import { HttpErrorResponse } from "@angular/common/http";

export function handleErrors(err: HttpErrorResponse): string[] {
  const errors: string[] = [];

  const detail = err.error.detail || err.statusText;
  if (detail) {
    errors.push(detail);
  }

  const otherErrors = err.error.errors;
  if (otherErrors) {
    Object.keys(otherErrors).forEach((key) => {
      errors.push(otherErrors[key]);
    });
  }

  return errors;
}
