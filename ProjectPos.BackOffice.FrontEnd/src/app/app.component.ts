import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { SystemName } from 'src/proxy/enums/system-name';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

    user: UserDto = {} as UserDto;

    constructor(
        private authService: AuthService,
        private router: Router,
        private messageService: MessageService,
        private primengConfig: PrimeNGConfig) { }

    ngOnInit() {
        this.primengConfig.ripple = true;

        sessionStorage.setItem('urls', 'https://localhost:7274/api');
    }

    @HostListener('window:unload', [ '$event' ])
    beforeUnloadHandler(event: { preventDefault: () => void; }) {
    this.logOut()
    event.preventDefault();
    return false;   
    }
    
    unloadHandler(event: any) {
        this.logOut()
        // ...
    }

    logOut(){
        this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');
        this.user.system = SystemName.BackOffice;
        
        this.authService.logout(this.user)
        .subscribe((res) => {

            if(res.isSuccess){

                localStorage.removeItem('token');
                localStorage.removeItem('userId');
                localStorage.removeItem('userName');
                sessionStorage.removeItem('loggedUser');
                this.router.navigate(['/auth/login']);

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
