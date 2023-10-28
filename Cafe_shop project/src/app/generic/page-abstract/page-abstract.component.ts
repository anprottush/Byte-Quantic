import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-page-abstract',
  templateUrl: './page-abstract.component.html',
  styleUrls: ['./page-abstract.component.css']
})
export class PageAbstractComponent implements OnDestroy {
  protected unsubscribe: Subject<boolean> = new Subject<boolean>();

  constructor(protected router: Router) { }

  public ngOnDestroy(): void {
    this.unsubscribe.next(true);
    this.unsubscribe.complete();
  }

  protected refreshComponent(url: string) {
    this.router.navigateByUrl('/', { skipLocationChange: true })
      .then(() => {
        this.router.navigate([url]);
      });
  }

  public async navigate(path: string): Promise<void> {
    await this.router.navigateByUrl(path);
  }

}
