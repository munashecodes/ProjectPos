import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Role } from '../enums/role';
import { UserDto } from '../interfaces/user-dto';
import { ServiceResponse } from '../interfaces/service-response';
import { environment } from 'src/environments/environment';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8')
  .set('Access-Control-Allow-Origin', '*')
  .set('Access-Control-Allow-Methods', 'GET, POST, PATCH, PUT, DELETE, OPTIONS');

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  loggedUser: UserDto = {
    id: 0,
    userName: '',
    password: '',
    jwtToken: '',
    isActive: false,
    role: Role.Admin
  };

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router) {}

  login(user: UserDto) {
    let returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
    sessionStorage.setItem('returnUrl', returnUrl);
    const body = JSON.stringify(user)
    console.log(body)
    return this.http.post<ServiceResponse<UserDto>>(`${url}/login`, body, { headers });
  }

  register(user: UserDto){
    let returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
    sessionStorage.setItem('returnUrl', returnUrl);
    const body = JSON.stringify(user)
    return this.http.post<ServiceResponse<any>>(`${url}/signin`, body, { headers });
  }

  update(user: UserDto){
    const body = JSON.stringify(user)
    return this.http.put<ServiceResponse<UserDto>>(`${url}/update`, body, { headers });
  }

  logout(user: UserDto) {
    
    const body = user
    console.log(body)
    return this.http.post<ServiceResponse<UserDto>>(`${url}/logout`, body, { headers });
  }

  isAuthenticated() {
    var user = sessionStorage.getItem('loggedUser');
    const body = JSON.stringify(user)
    return this.http.post<boolean>(`${url}/verifyToken`, user, { headers })
  }

  getUser(userName: string) {
    return this.http.get<any>(`${url}/${userName}`);
  }

  getAllUsers(){
    return this.http.get<any>(`${url}/getAll`);
  }
}
