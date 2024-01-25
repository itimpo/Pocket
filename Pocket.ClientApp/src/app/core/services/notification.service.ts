import { Injectable } from "@angular/core";
import { MessageService } from "primeng/api";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private message: MessageService) { }

  success(title: string = 'title', text: string = 'success', params: object = {}, life: number = 3000) {
    this.message.add({ severity: 'success', summary: title, detail: text, life: life });
  }

  error(title: string = 'title', text: string = 'error', params: object = {}, life: number = 3000) {
    this.message.add({ severity: 'error', summary: title, detail: text, life: life });
  }

  warning(title: string = 'title', text: string = 'warning', params: object = {}, life: number = 3000) {
    this.message.add({ severity: 'error', summary: title, detail: text, life: life });
  }

  info(title: string = 'title', text: string = 'info', params: object = {}, life: number = 3000) {
    this.message.add({ severity: 'error', summary: title, detail: text, life: life });
  }
}
