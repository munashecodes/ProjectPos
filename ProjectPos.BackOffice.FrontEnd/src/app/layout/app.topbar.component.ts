import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MenuItem, MessageService } from 'primeng/api';
import { LayoutService } from "./service/app.layout.service";
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { AuthService } from 'src/proxy/services/auth.service';
import { Router } from '@angular/router';
import { SystemName } from 'src/proxy/enums/system-name';
import { interval } from 'rxjs';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent implements OnInit {

    items!: MenuItem[];

    user: UserDto = {} as UserDto;

    menuItems: MenuItem[] = [];
    
    label: any;

    currentDate = new Date();

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    constructor(public layoutService: LayoutService,
        private authService: AuthService,
        private router: Router,
        private messageService: MessageService,
        ) { }

    ngOnInit(): void {
        this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

        // Get the current date
        this.currentDate = new Date();

        // Update the time every second
        interval(1000).subscribe(() => {
            this.currentDate = new Date();
        });

        console.log(this.user);
            this.label = (this.user.fullName) || '';
            this.items = [
                { label: 'Angular.io', icon: 'pi pi-external-link', url: 'http://angular.io' },
                { label: 'Theming', icon: 'pi pi-bookmark', routerLink: ['/theming'] }
            ];
    
            this.menuItems = [
                {
                    label: 'Save', 
                    icon: 'pi pi-fw pi-check',
                },
                {
                    label: 'Profile', 
                    icon: 'pi pi-fw pi-user',
                    routerLink: ['/profiling/profile'] 
                },
                {
                    label: 'Settings', 
                    icon: 'pi pi-fw pi-cog'
                },
                {
                    separator: true
                },
                {
                    label: 'Log Out', 
                    icon: 'pi pi-fw pi-sign-out',
                    command: (onclick)=> {this.logout()}
                },
            ];
    }

    logout(){
        this.user = JSON.parse(sessionStorage.getItem('loggedUser')!);
        this.user.system = SystemName.BackOffice;

        console.log(this.user);
        
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
                this.router.navigate(['/auth/login']);
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
