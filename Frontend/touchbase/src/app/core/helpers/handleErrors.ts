import { HttpErrorResponse } from "@angular/common/http";

export function handleErrors(err: HttpErrorResponse): string[] {
  const errors: string[] = [];

  const detail = err.error.detail || err.statusText;
  if (detail) {
    errors.push(detail);
  }

  const otherErrors = err.error.errors;
  if (otherErrors) {
    errors.push(...otherErrors);
  }

  return errors;
}
