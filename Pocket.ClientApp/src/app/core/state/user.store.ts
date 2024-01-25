import { Injectable } from '@angular/core';
import { createStore, select, withProps } from '@ngneat/elf';
import { localStorageStrategy, persistState } from '@ngneat/elf-persist-state';


interface UserProps {
  user?: string | null;
  currency: string;
  token?: string;
}

const userStore = createStore(
  { name: 'user' },
  withProps<UserProps>({ currency:'EUR' })
);

//save in local storage
export const persist = persistState(userStore, {
  key: 'auth',
  storage: localStorageStrategy,
});

userStore.subscribe((state) => console.log(state));
persist.initialized$.subscribe(console.log);

@Injectable({ providedIn: 'root' })
export class UserStore {

  store$ = userStore;

  user$ = userStore.pipe(select((state) => state.user));

  token$ = userStore.pipe(select(state => state.token));

  currency$ = userStore.pipe(select(state => state.currency));

  get current(): UserProps {
    return userStore.getValue();
  }

  get isAuthenticated(): boolean {
    return userStore.getValue().token != null;
  }

  setUser(user: string | null) {
    userStore.update((state) => ({
      ...state,
      user
    }));
  }

  setToken(token?: string ) {
    userStore.update((state) => ({
      ...state,
      token,
    }));
  }

  setCurrency(currency: string) {
    userStore.update((state) => ({
      ...state,
      currency,
    }));
  }
}
