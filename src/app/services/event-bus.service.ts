import { Injectable } from '@angular/core';
import { EventData } from '../models/EventData';
import { filter, map } from 'rxjs/operators';
import { Subject, Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventBusService {
  private subject$ = new Subject<EventData>();

  constructor() { }

  emit(event: EventData) {
    this.subject$.next(event);
  }

  on(eventName: string, action: any): Subscription {
    return this.subject$.pipe(
      filter((e: EventData) => e.name === eventName),
      map((e: EventData) => e["value"])
    ).subscribe(action);
  }
}
