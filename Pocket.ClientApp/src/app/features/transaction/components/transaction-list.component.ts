import { Component, OnInit } from '@angular/core';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UserStore } from '../../../core/state/user.store';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Result } from '../../../core/models/shared';
import { Transaction } from '../../../core/models/transaction';
import { TransactionService } from '../../../core/api/transaction.service';
import { TransactionEditComponent } from './transaction-edit.component';
import { CurrencyService } from '../../../core/api/currency.service';
import { Currency } from '../../../core/models/currency';

@Component({
  templateUrl: './transaction-list.component.html',
  providers: [DialogService, MessageService]
})
export class TransactionListComponent implements OnInit {

  transactions: Transaction[] = [];
  transactionDialog!: DynamicDialogRef;
  currency: string = 'EUR';
  currencies: Currency[] = [];
  searchTerm?: string;

  constructor(
    private dialogService: DialogService,
    private transactionService: TransactionService,
    private currencyService: CurrencyService,
    public userStore: UserStore,
    public router: Router
  ) {
      
  }

  ngOnInit() {
    this.currencyService.currencies()
      .subscribe(q => this.currencies = q);

    //this.refreshList();
    this.userStore.currency$.subscribe(q => {
      this.currency = q;
      this.refreshList();
    });
  }

  currencyChange(currency: string) {
    this.currency = currency;
    this.userStore.setCurrency(currency);
  }

  search(term: string | undefined = undefined) {
    if (term != undefined) {
      this.searchTerm = term;
    }
    this.refreshList();
  }

  refreshList() {
    this.transactionService.transactions(this.currency, this.searchTerm)
      .subscribe((response: Transaction[]) => this.transactions = response);
  }

  openTransactionDialog(id?:string) {
    this.transactionDialog = this.dialogService
      .open(TransactionEditComponent, {
        data: { id: id, currency: this.currency },
        header: 'Transaction',
        width: '500px'
      });

    this.transactionDialog.onClose
      .subscribe((result: Result) => {
        if (result && result.success) {
          this.refreshList();
        }
      });
  }

  deleteTransaction(id: string) {
    this.transactionService
      .transactionDelete(id)
      .subscribe(q => this.refreshList());
  }
}
