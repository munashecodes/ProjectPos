import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { UserSignInDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  message: string | undefined
  confirmPassword = ''
  newUser: UserSignInDto = {} as UserSignInDto
  
  constructor(
    public layoutService: LayoutService,
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router) { }
    
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  signIn(user: UserSignInDto){
    
    console.log(user)

    if(user.password !== this.confirmPassword){
      this.message = "Passwords don't match"
    }
    else{
      this.authService.register(user)
      .subscribe((res) => {
        console.log(res.data);
        
        if(res.isSuccess){

          this.message = res.message!;

          
          this.messageService.add({
            severity:'success', 
            summary: 'Success', 
            detail: res.message, 
            life: 3000
          });

          setTimeout(() => {
            this.router.navigate(['auth/login']);
          }, 3001)
        }
        else{
          this.message = res.message!;
          console.log(this.message);

          

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
  
}
