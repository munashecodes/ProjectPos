import { Component, OnInit } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

    constructor(private primengConfig: PrimeNGConfig) { }

    ngOnInit() {
        this.primengConfig.ripple = true;

        sessionStorage.setItem('urls', 'https://localhost:7274/api');
        sessionStorage.setItem('imageUrl', 'https://localhost:7274/Resources/');
    }
}
