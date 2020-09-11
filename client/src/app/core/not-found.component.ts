import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-not-found',
  template: `
    <div class="ml-3 mr-3">
      <div class="row full-height">
        <div class="col">
          <h1>Could not find the page you were looking for</h1>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotFoundComponent {}
