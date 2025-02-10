import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { AuthGuardService } from 'src/proxy/services/auth-guard.service';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {
                path: '', component: AppLayoutComponent,
                children: [
                    { path: '', loadChildren: () => import('./demo/components/dashboard/dashboard.module').then(m => m.DashboardModule), canActivate: [AuthGuardService] },
                    { path: 'uikit', loadChildren: () => import('./demo/components/uikit/uikit.module').then(m => m.UIkitModule), canActivate: [AuthGuardService] },
                    { path: 'utilities', loadChildren: () => import('./demo/components/utilities/utilities.module').then(m => m.UtilitiesModule), canActivate: [AuthGuardService] },
                    { path: 'documentation', loadChildren: () => import('./demo/components/documentation/documentation.module').then(m => m.DocumentationModule), canActivate: [AuthGuardService] },
                    { path: 'blocks', loadChildren: () => import('./demo/components/primeblocks/primeblocks.module').then(m => m.PrimeBlocksModule), canActivate: [AuthGuardService] },
                    { path: 'pages', loadChildren: () => import('./demo/components/pages/pages.module').then(m => m.PagesModule), canActivate: [AuthGuardService] },
                    { 
                        path: 'setup', 
                        loadChildren: () => import(
                            './setup/setup.module'
                            ).then(m => m.SetupModule), 
                            canActivate: [AuthGuardService] 
                    },
                    { 
                        path: 'inventory', 
                        loadChildren: () => import(
                            './inventory/inventory.module'
                            ).then(m => m.InventoryModule) , 
                            canActivate: [AuthGuardService] 
                    },
                    { 
                        path: 'employee-management', 
                        loadChildren: () => import(
                            './employee-management/employee-management.module'
                            ).then(m => m.EmployeeManagementModule), 
                            canActivate: [AuthGuardService]  
                    },
                    { 
                        path: 'user-management', 
                        loadChildren: () => import(
                            './user-management/user-management.module'
                            ).then(m => m.UserManagementModule), 
                            canActivate: [AuthGuardService]  
                    },
                    { 
                        path: 'procurement', 
                        loadChildren: () => import(
                            './procurement/procurement.module'
                            ).then(m => m.ProcurementModule), 
                            canActivate: [AuthGuardService]  
                    },
                    { 
                        path: 'cashier', 
                        loadChildren: () => import(
                            './cashier/cashier.module'
                            ).then(m => m.CashierModule), 
                            canActivate: [AuthGuardService]  
                    },
                    { 
                        path: 'accounting', 
                        loadChildren: () => import(
                            './accounting/accounting.module'
                            ).then(m => m.AccountingModule), 
                            canActivate: [AuthGuardService]  
                    },
                    { 
                        path: 'sales', 
                        loadChildren: () => import(
                            './sales/sales.module'
                            ).then(m => m.SalesModule), 
                            canActivate: [AuthGuardService]  
                    },
                    { 
                        path: 'reporting', 
                        loadChildren: () => import(
                            './reporting/reporting.module'
                            ).then(m => m.ReportingModule), 
                            canActivate: [AuthGuardService]  
                    }
                ]
            },
            { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
            { path: 'auth', loadChildren: () => import('./demo/components/auth/auth.module').then(m => m.AuthModule) },
            { path: 'landing', loadChildren: () => import('./demo/components/landing/landing.module').then(m => m.LandingModule) },
            { path: 'notfound', component: NotfoundComponent },
            { path: '**', redirectTo: '/notfound' },
        ], { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
