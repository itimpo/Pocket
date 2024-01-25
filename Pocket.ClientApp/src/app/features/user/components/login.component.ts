import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../core/api/user.service';
import { UserLogin } from '../../../core/models/user';
import { UserStore } from '../../../core/state/user.store';
import { Router } from '@angular/router';

@Component({
  selector: 'user-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private userStore: UserStore,
    private router: Router
  )
  {
    let model = new UserLogin();

    model.email = 'dtymofieiev@gmail.com';
    model.password = '123123';

    this.form = this.fb.group(model);
    this.form.controls["email"].addValidators([Validators.required, Validators.email, Validators.minLength(10), Validators.maxLength(50)]);
    this.form.controls["password"].addValidators([Validators.required, Validators.minLength(6)]);
  }

  login() {
    this.userService.login(this.form.value)
      .subscribe(r => {
        this.userStore.setToken(r.token);
        this.router.navigate(['/transaction/list'])
      })
  }
}

