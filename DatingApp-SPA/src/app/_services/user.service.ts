import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
// lesson 84
// const httpOptions = {
//  headers: new HttpHeaders({
    // tslint:disable-next-line: object-literal-key-quotes 28-05-2020 to check
//    'Authorization': 'Bearer ' + localStorage.getItem('token')
//  })
// };


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseurl = environment.apiUrl; // lesson 84

constructor(private http: HttpClient) { }
getUsers(): Observable<User[]> {
 return this.http.get<User[]>(this.baseurl + 'users');
}

getUser(id): Observable<User> {
  return this.http.get<User>(this.baseurl + 'users/' + id);
}
}
