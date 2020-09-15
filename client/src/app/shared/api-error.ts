import { HttpErrorResponse } from '@angular/common/http';

export function mapErrorResponse(
  callback: (error: APIError) => void,
): (response: HttpErrorResponse) => void {
  return (response) => callback(response.error as APIError);
}

export interface APIError {
  id: string;
  message: string;
}
