import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';

import { TransactionListComponent } from './components/transaction-list.component';
import { TransactionEditComponent } from './components/transaction-edit.component';
import { EventRoutingModule } from './transaction-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';

@NgModule({
  declarations: [
    TransactionListComponent,
    TransactionEditComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    EventRoutingModule,
    ButtonModule,
    DropdownModule,
    TableModule,
    InputTextModule
  ]
})
export class TransactionModule {}

