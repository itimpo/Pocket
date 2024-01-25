import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { NotificationService } from '../../../core/services/notification.service';
import { Transaction, TransactionGroup } from '../../../core/models/transaction';
import { TransactionService } from '../../../core/api/transaction.service';
import { lastValueFrom, map } from 'rxjs';

@Component({
  templateUrl: './transaction-edit.component.html',
  styles: ['.p-autocomplete-panel {z-index: 1003!important;}']
})
export class TransactionEditComponent implements OnInit {

  id!: string;
  currency!: string;
  groups: any[] = [];

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private transactionService: TransactionService,
    private ref: DynamicDialogRef,
    private config: DynamicDialogConfig,
    private notify: NotificationService
  ) {
    this.id = this.config.data.id;
    this.currency = this.config.data.currency;
  }

  async ngOnInit() {
    let transaction = new Transaction();
    this.form = this.fb.group(transaction);
    if (this.id) {
      transaction = await lastValueFrom(this.transactionService.transactionGet(this.id));
    } else {
      transaction.currency = this.currency;
    }
    this.form.patchValue(transaction);
    
    this.addValidators();

    this.transactionService.groups()
      .pipe(map((response: TransactionGroup[]) => response.map(s => s.name)))
      .subscribe(q => this.groups = q);
  }

  addValidators() {
    this.form.controls["amount"].addValidators([Validators.required]);
    this.form.controls["currency"].addValidators([Validators.required, Validators.minLength(3), Validators.maxLength(3)]);
    this.form.controls["transactionGroup"].addValidators([Validators.required, Validators.maxLength(100)]);
    this.form.controls["description"].addValidators([Validators.required, Validators.maxLength(250)]);
  }

  cancel() {
    this.ref.close();
  }

  save() {
    this.transactionService.
      transactionCreate(this.form.value)
        .subscribe(r => {
          if (r.success) {
            this.notify.success('Saved')
            this.ref.close(r);
          } else {
            this.notify.error('Failed', r.error);
          }
        })
  }
}
