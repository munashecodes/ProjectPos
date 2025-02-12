import { filter } from 'rxjs';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';
import { UserDto } from 'src/proxy/interfaces/user-dto';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    menuItems: any[] = [];

    filteredMenuItems: any[] = [];

    filteredModel: any[] = [];

    filteredProcurementItems: any[] = [];

    filteredAcountsItems: any[] = [];

    filteredSetupItems: any[] = [];

    ProcurementItems: any[] = [];

    AcountsItems: any[] = [];

    SetupItems: any[] = [];

    user: UserDto = {} as UserDto;

    role: any;

    constructor(public layoutService: LayoutService) { }

    ngOnInit() {
        this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

        this.model = [
            {
                label: 'Home',
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Cashier', 'Accountant', 'AccountsClerk', 'DebtorsController', 'FinanceManager', 'SysDeveloper', 'StockController', 'ReceivingClerk'],
                items: [
                    { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }
                ]
            },
            {
                label: 'Menu',
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Cashier', 'Accountant', 'AccountsClerk', 'DebtorsController', 'FinanceManager', 'SysDeveloper', 'StockController', 'ReceivingClerk'],
                items: this.filteredMenuItems
            },
            {
                label: 'Human Resourses',
                roles: ['Admin', 'Manager', 'SysAdmin', 'SysDeveloper'],
                items: [
                    { 
                        label: 'Employees', 
                        icon: 'pi pi-fw pi-eye', 
                        items: [
                            { 
                                label: 'Manage Employees', 
                                icon: 'pi pi-fw pi-id-card', 
                                routerLink: ['/employee-management/manage-employees'] 
                            },
                        ]
                    },
                ]
            },
            {
                label: 'User Management',
                roles: ['Admin', 'Manager', 'Supervisor', 'SysAdmin', 'SysDeveloper'],
                items: [
                    { 
                        label: 'Users', 
                        icon: 'pi pi-fw pi-eye', 
                        items: [
                            { 
                                label: 'Manage Users', 
                                icon: 'pi pi-fw pi-id-card', 
                                routerLink: ['/user-management/manage-users'] 
                            },
                        //     { 
                        //         label: 'Manage Roles', 
                        //         icon: 'pi pi-fw pi-id-card', 
                        //         routerLink: ['/setup/contact-persons'] 
                        //     },
                        //     { 
                        //         label: 'Manage Permissions', 
                        //         icon: 'pi pi-fw pi-id-card', 
                        //         routerLink: ['/setup/sub-categories'] 
                        //     },
                         ] 
                    },
                ]
            }
        ];

        // this.filteredProcurementItems = this.model.filter(item => item.label === 'Procument');

        this.menuItems = [
            { 
                label: 'Sales', 
                icon: 'pi pi-fw pi-id-card', 
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Accountant', 'AccountsClerk', 'DebtorsController', 'FinanceManager', 'SysDeveloper'],
                items: [
                    { 
                        label: 'Sales', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/sales/manage-sales'] 
                    },
                    { 
                        label: 'Day End', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/sales/end-day'] 
                    },
                ] 
            },
            { 
                label: 'Cashier', 
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'AccountsClerk', 'SysDeveloper'],
                icon: 'pi pi-fw pi-id-card', 
                items: [
                    { 
                        label: 'Cashier Reports', 
                        icon: 'pi pi-fw pi-file-import', 
                        routerLink: ['/cashier/cashier-reports'] 
                    },
                    { 
                        label: 'Recons Reports', 
                        icon: 'pi pi-fw pi-file-import', 
                        routerLink: ['/cashier/recon-reports'] 
                    },
                    { 
                        label: 'Exchange Rates', 
                        icon: 'pi pi-fw pi-bitcoin', 
                        routerLink: ['/cashier/manage-rates'] 
                    },
                    { 
                        label: 'BankNote Details', 
                        icon: 'pi pi-fw pi-dollar', 
                        routerLink: ['/cashier/bank-note-details'] 
                    },
                ]
            },
            { 
                label: 'Procument', 
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Accountant', 'AccountsClerk', 'DebtorsController', 'FinanceManager', 'SysDeveloper', 'StockController', 'ReceivingClerk'],
                icon: 'pi pi-fw pi-id-card', 
                items: this.filteredProcurementItems
            },
            { 
                label: 'Inventory Management', 
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Accountant', 'AccountsClerk', 'DebtorsController', 'FinanceManager', 'SysDeveloper', 'StockController' ],
                icon: 'pi pi-fw pi-id-card', 
                items: [
                    { 
                        label: 'Product Inventories', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/inventory/product-inventory'] 
                    },
                    { 
                        label: 'Stock Movement', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/inventory/stock-movement'] 
                    },
                ]
            },
            { 
                label: 'Accounts', 
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Accountant', 'AccountsClerk', 'DebtorsController', 'FinanceManager', 'SysDeveloper'],
                icon: 'pi pi-fw pi-id-card', 
                items: this.filteredAcountsItems
            },
            { 
                label: 'Reports', 
                roles: ['Admin', 'Supervisor', 'Manager', 'SysAdmin', 'Accountant', 'AccountsClerk', 'FinanceManager', 'SysDeveloper', 'StockController'],
                icon: 'pi pi-fw pi-id-card', 
                items: [
                    // { 
                    //     label: 'Sales Report', 
                    //     icon: 'pi pi-fw pi-id-card', 
                    //     routerLink: ['/setup/companies'] 
                    // },
                    // { 
                    //     label: 'Payments Report', 
                    //     icon: 'pi pi-fw pi-id-card', 
                    //     routerLink: ['/setup/contact-persons'] 
                    // },
                    // { 
                    //     label: 'CashUp Report', 
                    //     icon: 'pi pi-fw pi-id-card', 
                    //     routerLink: ['/setup/sub-categories'] 
                    // },
                    { 
                        label: 'Stock Report', 
                        icon: 'pi pi-fw pi-id-card', 
                        routerLink: ['/reporting/stock-take'] 
                    }
                    // { 
                    //     label: 'Manage Product Prices', 
                    //     icon: 'pi pi-fw pi-id-card', 
                    //     routerLink: ['/setup/product-prices'] 
                    // },
                ]
            },
            { 
                label: 'Setup', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'StockController'],
                icon: 'pi pi-fw pi-id-card', 
                items: this.filteredSetupItems
            },
        ]

        this.ProcurementItems = [
            { 
                label: 'Purchacing', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'StockController', 'ReceivingClerk'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/procurement/orders'] 
            },
            { 
                label: 'Receiving', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'ReceivingClerk'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/procurement/grvs'] 
            },
        ];

        this.AcountsItems = [
            { 
                label: 'Proof of Payments', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'Accountant', 'AccountsClerk'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/accounting/proof-of-payments'] 
            },
            { 
                label: 'Invoicing', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'Accountant', 'AccountsClerk', 'Cashier'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/accounting/invoicing'] 
            },
            // { 
            //     label: 'Credit Notes', 
            //     roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'Accountant', 'AccountsClerk'],
            //     icon: 'pi pi-fw pi-id-card', 
            //     routerLink: ['/accounts/credit-notes'] 
            // },
            { 
                label: 'Expenses', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'Accountant', 'AccountsClerk'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/accounting/expenses'] 
            }
            // { 
            //     label: 'Deptors & Creditors', 
            //     roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'Accountant', 'AccountsClerk'],
            //     icon: 'pi pi-fw pi-id-card', 
            //     // routerLink: [] 
            // },
            // { 
            //     label: 'Exception Reports', 
            //     roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'Accountant', 'AccountsClerk'],
            //     icon: 'pi pi-fw pi-id-card', 
            //     // routerLink: [] 
            // },
        ];

        this.SetupItems = [
            { 
                label: 'Manage Companies', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/setup/companies'] 
            },
            { 
                label: 'Manage Contact Persons', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/setup/contact-persons'] 
            },
            { 
                label: 'Manage Product Categories', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'StockController'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/setup/sub-categories'] 
            },
            { 
                label: 'Manage Products', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'StockController'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/setup/products'] 
            },
            { 
                label: 'Manage Product Prices', 
                roles: ['Admin', 'SysAdmin', 'SysDeveloper', 'Manager', 'Supervisor', 'StockController'],
                icon: 'pi pi-fw pi-id-card', 
                routerLink: ['/setup/product-prices'] 
            },
        ]

        if(this.user != null){
            this.role = this.user.role;
        }
        
        // filter procurement items by role
        this.ProcurementItems.forEach((x: any) => {
            x.roles.forEach((role: any) => {
                if(role === this.role){
                    this.filteredProcurementItems.push(x);
                }
            });
        });

        // filter accounts items by role
        this.AcountsItems.forEach((x: any) => {
            x.roles.forEach((role: any) => {
                if(role === this.role){
                    this.filteredAcountsItems.push(x);
                }
            });
        });

        // filter setup items by role
        this.SetupItems.forEach((x: any) => {
            x.roles.forEach((role: any) => {
                if(role === this.role){
                    this.filteredSetupItems.push(x);
                }
            });
        })

        // filter menu items by role
        this.menuItems.forEach((x: any) => {
            x.roles.forEach((role: any) => {
                if(role === this.role){
                    this.filteredMenuItems.push(x);
                }
            });
        });

        // filter model by role
        this.model.forEach((x: any) => {
            x.roles.forEach((role: any) => {
                if(role === this.role){
                    this.filteredModel.push(x);
                }
            });
        });
    }
}
