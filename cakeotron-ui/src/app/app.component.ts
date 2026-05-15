import { Component, OnInit } from '@angular/core';
import { CakeReason, CakeService } from './cake.service';

interface ReasonGroup {
  label: string;
  date: string;
  reasons: string[];
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  reasons: CakeReason[] = [];
  loading = false;
  error = '';

  constructor(private cakeService: CakeService) {}

  ngOnInit(): void {
    this.loadReasons();
  }

  get groupedReasons(): ReasonGroup[] {
    const map = new Map<string, ReasonGroup>();
    for (const item of this.reasons) {
      const dateKey = `${item.referenceDate.description} — ${new Date(item.referenceDate.date).toLocaleDateString()}`;
      const existing = map.get(dateKey);
      if (existing) {
        existing.reasons.push(item.reason);
      } else {
        map.set(dateKey, {
          label: item.referenceDate.description,
          date: new Date(item.referenceDate.date).toLocaleDateString(),
          reasons: [item.reason]
        });
      }
    }
    return [...map.values()];
  }

  get headerText(): string {
    if (this.loading) {
      return 'Baking the reasons...';
    }
    if (this.error) {
      return 'CakeOTron needs a moment to recover.';
    }
    return 'Today is a perfect cake day!';
  }

  loadReasons(): void {
    this.loading = true;
    this.error = '';
    this.cakeService.getReasons().subscribe({
      next: reasons => {
        this.reasons = reasons ?? [];
        this.loading = false;
      },
      error: err => {
        this.loading = false;
        this.error = 'Could not reach the CakeOTron API. Make sure your mainservice is running and reachable.';
        console.error(err);
      }
    });
  }
}
