import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  response!: any;
  constructor(
    private auth: AuthService, 
    private router: Router) { }

  async canActivate(_route: any, state: RouterStateSnapshot): Promise<any>  {
    return this.auth.isAuthenticated()
    .subscribe(res => {
      if(res){
        return true
      }
      else{
        this.router.navigate(['/auth/login'] , { queryParams: { returnUrl: state.url}});
        return false;
      }
    },
    (error) => {
      this.router.navigate(['/auth/login'] , { queryParams: { returnUrl: state.url}});
        return false;
    })
  }
}
