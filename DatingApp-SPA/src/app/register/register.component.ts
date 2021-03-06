import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // optional on the properties


  constructor(private authService: AuthService, private router: Router,
              private alertify: AlertifyService, private fb: FormBuilder) { }
  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    },
    this.CreateRegisterForm();
  //  this.registerForm = new FormGroup({
  //    username: new FormControl('', Validators.required),
  //    password: new FormControl('',
  //     [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
  //    confirmPassword: new FormControl('', Validators.required)
   // }, this.passwordMatchValidator );
  }

  CreateRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validator: this.passwordMatchValidator}
    );
  }
  passwordMatchValidator(g: FormGroup) {
   // tslint:disable-next-line: object-literal-key-quotes
   return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }
  register() {
  if (this.registerForm.valid) {
    this.user = Object.assign({}, this.registerForm.value); // register form cloning to user
    this.authService.register(this.user).subscribe(() => {
      this.alertify.success('Registration successful');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.authService.login(this.user).subscribe(() => {
        this.router.navigate(['/members']);
      });
    });
  }


    //  console.log(this.registerForm.value);
  //  this.authService.register(this.model).subscribe(() => {
  //    this.alertify.success('Registration Succesful');
  //  }, error => {
  //    this.alertify.error(error) ;
  //  });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
