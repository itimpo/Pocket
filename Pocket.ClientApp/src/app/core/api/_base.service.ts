import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationService } from '../services/notification.service';

export class BaseService {

  baseUrl: string;
  httpClient!: HttpClient;
  notification!: NotificationService;

  constructor(
  ) {
    this.baseUrl = environment.apiUrl;
    this.httpClient = inject(HttpClient);
    this.notification = inject(NotificationService);
  }

  protected httpOptions() {
    return {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      }
  }

  protected get<TOut>(url: string, filter: any = null): Observable<TOut> {
    return this.httpClient.get<TOut>(`${this.baseUrl}${url}`, { ...this.httpOptions(), params: filter })
      .pipe(catchError((e) => this.errorHandler(e)))
  }

  protected post<TIn, TOut>(url: string, post: TIn): Observable<TOut> {
    return this.httpClient.post<TOut>(`${this.baseUrl}${url}`, JSON.stringify(post), this.httpOptions())
      .pipe(catchError((e) => this.errorHandler(e)))
  }

  protected put<TIn, TOut>(url: string, post: TIn): Observable<TOut> {
    return this.httpClient.put<TOut>(`${this.baseUrl}${url}`, JSON.stringify(post), this.httpOptions())
      .pipe(catchError((e) => this.errorHandler(e)))
  }

  protected delete<TOut>(url: string) {
    return this.httpClient.delete<TOut>(`${this.baseUrl}${url}`, this.httpOptions())
      .pipe(catchError((e) => this.errorHandler(e)))
  }

  protected errorHandler(response: any){
    const error = response.error as Error;
    this.notification.error('message.title', response.error.message);
    return throwError(error);
  }
}
