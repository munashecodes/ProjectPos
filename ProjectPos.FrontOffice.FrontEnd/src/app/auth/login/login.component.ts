import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { Role } from 'src/proxy/enums/role';
import { SystemName } from 'src/proxy/enums/system-name';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  valCheck: string[] = ['remember'];

    password!: string;

    user: UserDto = {} as UserDto;

    loggedUser: UserDto = {} as UserDto;

    token!: string;

    message: string = '';

    constructor(
      public layoutService: LayoutService,
      private authService: AuthService,
      private messageService: MessageService,
      private router: Router) { }

    login(user: UserDto){

      this.user.role = Role.Admin;
      this.user.isActive = false;
      this.user.system = SystemName.POS;
      
      console.log(user)
      if(!user.password){
        this.message = "Please Enter Your Password"
      }
      else{
        this.authService.login(user)
        .subscribe((res) => {
          console.log(res.data);
          
          if(res.isSuccess){
  
            this.message = res.message!;
            this.user = res.data
            this.user.system = SystemName.POS;
  
            this.router.navigate(['/']);
            sessionStorage.setItem('loggedUser', JSON.stringify(this.user));
            this.messageService.add({
              severity:'success', 
              summary: 'Success', 
              detail: res.message, 
              life: 3000
            });
          }
          else{
            this.message = res.message!;
            console.log(this.message);
  
            if(res.data !== null)
            {
              this.user = res.data
              this.user.system = SystemName.POS;
            }
  
            this.messageService.add({
              severity:'error', 
              summary: 'Success', 
              detail: res.message, 
              life: 3000
            });
          }
        },
        (error) => {
          this.messageService.add({
            severity:'error', 
            summary: 'Error', 
            detail: error.message, 
            life: 3000
          });
        });
      }
    }

    logout(){

      this.user.system = SystemName.POS;
      this.authService.logout(this.user)
      .subscribe((res) => {

          if(res.isSuccess){

              localStorage.removeItem('token');
              localStorage.removeItem('userId');
              localStorage.removeItem('userName');
              sessionStorage.removeItem('loggedUser');
              this.router.navigate(['/auth/login']);
              this.message = '';

              this.messageService.add({
                  severity:'success', 
                  summary: 'Success', 
                  detail: res.message , 
                  life: 3000
              });
          }
          else{
              this.messageService.add({
                  severity:'error', 
                  summary: 'Error', 
                  detail: res.message , 
                  life: 3000
              });
          }
      },
      (error) => {
          this.messageService.add({
              severity:'error', 
              summary: 'Error', 
              detail: error.message, 
              life: 3000
          });
      });
  }
}
