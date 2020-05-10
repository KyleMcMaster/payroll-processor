import { map } from 'rxjs/operators';

/**
 * Represents the shape of an API response containing a collection of items
 */
export interface ListResponse<T> {
  readonly data: T[];
}

/**
 * Maps API list responses to the collection of objects they contain
 */
export function mapListResponseToData<T>() {
  return map<ListResponse<T>, T[]>(({ data }) => data);
}
