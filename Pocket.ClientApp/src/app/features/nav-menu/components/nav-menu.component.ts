import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { UserStore } from '../../../core/state/user.store';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
})
export class NavMenuComponent {
  items: MenuItem[] | undefined;

  constructor(public userStore: UserStore) {

  }

  ngOnInit() {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-fw pi-home',
        routerLink: '/' 
      },
      {
        label: 'Transactions',
        icon: 'pi pi-fw pi-calendar',
        routerLink: '/transaction/list'
      }
    ];
  }

  logout() {
      this.userStore.setToken(undefined);
      // Redirect to homepage
      window.location.href = '/';
    }
}
