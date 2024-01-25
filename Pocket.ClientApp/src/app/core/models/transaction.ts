import { v4 as uuidv4 } from 'uuid';

export class Transaction {
  id: string;
  amount: number;
  currency: string;
  transactionGroup: string;
  description: string;
  transactionDate: Date;

  constructor() {
    this.id = uuidv4();
    this.amount = 0;
    this.currency = '';
    this.transactionGroup = '';
    this.description = '';
    this.transactionDate = new Date();
  }
}

export class TransactionGroup {
  id?: number;
  name: string;

  constructor() {
    this.name = '';
  }
}
