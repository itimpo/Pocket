
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result } from '../models/shared';
import { BaseService } from './_base.service';
import { Transaction, TransactionGroup } from '../models/transaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionService extends BaseService{

  url = {
    transaction: '/transactions',
  }

  //#region Transactions

  transactions(currency: string, search?:string): Observable<Transaction[]> {
    return super.get<Transaction[]>(`${this.url.transaction}`, { currency: currency, search: search??'' });
  }

  groups(): Observable<TransactionGroup[]> {
    return super.get<TransactionGroup[]>(`${this.url.transaction}/groups`);
  }

  transactionGet(id: string): Observable<Transaction> {
    return super.get(`${this.url.transaction}/${id}`);
  }

  transactionCreate(post: Transaction): Observable<Result> {
    return super.post(this.url.transaction, post)
  }

  transactionUpdate(id: string, post: Transaction): Observable<Result> {
    return super.put(`${this.url.transaction}/${id}`, post)
  }

  transactionDelete(id: string): Observable<Result> {
    return super.delete(`${this.url.transaction}/${id}`)
  }

  //#endregion
}
