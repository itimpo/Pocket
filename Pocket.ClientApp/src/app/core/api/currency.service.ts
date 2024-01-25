
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Currency } from '../models/currency';
import { Result } from '../models/shared';
import { BaseService } from './_base.service';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService extends BaseService{

  url = {
    currencies: '/currencies',
  }

  //#region Currencies

  currencies(): Observable<Currency[]> {
    return super.get<Currency[]>(`${this.url.currencies}`);
  }

  currencyGet(id: number): Observable<Currency> {
    return super.get(`${this.url.currencies}/${id}`);
  }

  currencyCreate(post: Currency): Observable<Result> {
    return super.post(this.url.currencies, post)
  }

  currencyUpdate(id: number, post: Currency): Observable<Result> {
    return super.put(`${this.url.currencies}/${id}`, post)
  }

  //#endregion
}
