import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TransactionListComponent } from './components/transaction-list.component';
import { AuthGuard } from '../../shared/auth-guard';
import { TransactionEditComponent } from './components/transaction-edit.component';

const routes: Routes = [
  { path: 'list', canActivate: [AuthGuard], component: TransactionListComponent },
//  { path: 'edit/:id', canActivate: [AuthGuard], component: TransactionEditComponent },
//  { path: 'edit', canActivate: [AuthGuard], component: TransactionEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventRoutingModule { }
