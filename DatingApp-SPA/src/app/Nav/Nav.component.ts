import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';



@Component({
   selector: 'app-nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string; // use for behavioural side pic

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {

    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }
  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in successfully');
    }, error => {
       this.alertify.error(error);
    }, () => {
      this.router.navigate(['members']);
    });
  }


  loggedIn() {
     return this.authService.loggedIn();
    }

  logout() {
     localStorage.removeItem('token');
     localStorage.removeItem('user'); // use for phot username
     this.authService.decodedToken = null;  // use for phot username
     this.authService.currentUser = null;  // use for phot username
     this.alertify.message('Logged Out');
     this.router.navigate(['home']);

  }
}
