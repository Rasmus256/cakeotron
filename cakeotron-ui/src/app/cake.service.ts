import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

export interface ReferenceDate {
  date: string;
  description: string;
}

export interface CakeReason {
  reason: string;
  referenceDate: ReferenceDate;
}

@Injectable({
  providedIn: 'root'
})
export class CakeService {
  private readonly apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getReasons() {
    return this.http.get<CakeReason[]>(this.apiUrl, {
      withCredentials: false
    });
  }
}
