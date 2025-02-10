import { Component, OnInit, OnDestroy } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Product } from '../../api/product';
import { ProductService } from '../../service/product.service';
import { Subscription } from 'rxjs';
import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';
import { SalesOrderDto } from 'src/proxy/interfaces/sales-order-dto';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { Category } from 'src/proxy/enums/category';

@Component({
    templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit, OnDestroy {

    items!: MenuItem[];

    products!: Product[];

    chartData: any;

    chartOptions: any;

    subscription!: Subscription;

    todaySales: SalesOrderDto[] = [];

    orders = 0;

    revenue = 0;

    averageBusketSize = 0;

    averageOrderValue = 0;

    orderItems: SalesOrderItemDto[] = [];

    top5FreshProducts: SalesOrderItemDto[] = [];

    top5DryGoods: SalesOrderItemDto[] = [];

    monthSales: SalesOrderDto[] = [];

    monthOrders = 0;

    monthRevenue = 0;

    monthlyAverageBusketSize = 0;

    monthlyAverageOrderValue = 0;

    monthOrderItems: SalesOrderItemDto[] = [];

    monthTop5FreshProducts: SalesOrderItemDto[] = [];

    monthTop5DryGoods: SalesOrderItemDto[] = [];

    currentDate = new Date();

    constructor(
        private productService: ProductService, 
        public layoutService: LayoutService,
        private salesOrderService: SalesOrderService
        ) {
            this.subscription = this.layoutService.configUpdate$.subscribe(() => {
                this.initChart();
            });
        }

    ngOnInit() {
        this.initChart();
        this.productService.getProductsSmall().then(data => this.products = data);

        this.items = [
            { label: 'Add New', icon: 'pi pi-fw pi-plus' },
            { label: 'Remove', icon: 'pi pi-fw pi-minus' }
        ];

        // Get the current date
        this.currentDate = new Date();

        // Get the month (returns a zero-based index)
        const month = this.currentDate.getMonth();

        // Add 1 to the month to get a human-readable format (1-12)
        const humanReadableMonth = month + 1;

        console.log(humanReadableMonth);

        this.salesOrderService.getAllByMonth(humanReadableMonth)
        .subscribe(res => {
            console.log(res.data);
            this.monthSales = res.data;
            this.monthOrders = this.monthSales.length;
            this.monthRevenue = this.monthSales.reduce((acc, cur) => acc + cur.priceIncludingVat!, 0);
            this.monthlyAverageBusketSize = this.monthSales.map(so => so.salesOrderItems!).flat().length / this.monthOrders;
            this.monthlyAverageOrderValue = this.monthRevenue / this.monthOrders;
        });

        this.salesOrderService.getMonthAllItems(humanReadableMonth)
        .subscribe(res => {
            console.log(res.data);
            this.monthOrderItems = res.data;

            const freshProducts = this.monthOrderItems.filter(item => item.category! === Category.FreshProduce);
            const dryProducts = this.monthOrderItems.filter(item => item.category! === Category.DryGoods);
            
            // Step 3: Sort the array by quantity in descending order
            const sortedFreshProducts = freshProducts.sort((a, b) => b.price! - a.price!);

            // Step 3: Sort the array by price in descending order
            const sortedDryProducts = dryProducts.sort((a, b) => b.price! - a.price!);
            
            // Step 4: Get the top 5 most sold items
            this.monthTop5FreshProducts = sortedFreshProducts.slice(0, 5);

            this.monthTop5DryGoods = sortedDryProducts.slice(0, 5);
        });

        this.salesOrderService.getAllItems()
        .subscribe(res => {
            console.log(res.data);
            this.orderItems = res.data;

            const freshProducts = this.orderItems.filter(item => item.category! === Category.FreshProduce);
            const dryProducts = this.orderItems.filter(item => item.category! === Category.DryGoods);
            
            // Step 3: Sort the array by quantity in descending order
            const sortedFreshProducts = freshProducts.sort((a, b) => b.price! - a.price!);

            // Step 3: Sort the array by price in descending order
            const sortedDryProducts = dryProducts.sort((a, b) => b.price! - a.price!);
            
            // Step 4: Get the top 5 most sold items
            this.top5FreshProducts = sortedFreshProducts.slice(0, 5);

            this.top5DryGoods = sortedDryProducts.slice(0, 5);
        });

        this.salesOrderService.getAllToday()
        .subscribe(res => {
            console.log(res.data);  
            this.todaySales = res.data;
            this.orders = this.todaySales.length;
            this.revenue = this.todaySales.reduce((acc, cur) => acc + cur.priceIncludingVat!, 0);
            this.averageBusketSize = this.todaySales.map(so => so.salesOrderItems!).flat().length / this.orders;
            this.averageOrderValue = this.revenue / this.orders;
            
            // this.top5Items = this.orderItems.reduce((acc, cur) => {
            //     const existingItem = acc.find(item => item.productId === cur.productId);
            //     if (existingItem) {
            //         existingItem.quantity! += cur.quantity!;
            //     } else {
            //         acc.push(cur);
            //     }
            //     return acc;
            // }, [] as SalesOrderItemDto[])
        });

    }

    initChart() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

        this.chartData = {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
            datasets: [
                {
                    label: 'First Dataset',
                    data: [65, 59, 80, 81, 56, 55, 40],
                    fill: false,
                    backgroundColor: documentStyle.getPropertyValue('--bluegray-700'),
                    borderColor: documentStyle.getPropertyValue('--bluegray-700'),
                    tension: .4
                },
                {
                    label: 'Second Dataset',
                    data: [28, 48, 40, 19, 86, 27, 90],
                    fill: false,
                    backgroundColor: documentStyle.getPropertyValue('--green-600'),
                    borderColor: documentStyle.getPropertyValue('--green-600'),
                    tension: .4
                }
            ]
        };

        this.chartOptions = {
            plugins: {
                legend: {
                    labels: {
                        color: textColor
                    }
                }
            },
            scales: {
                x: {
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                },
                y: {
                    ticks: {
                        color: textColorSecondary
                    },
                    grid: {
                        color: surfaceBorder,
                        drawBorder: false
                    }
                }
            }
        };
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
}
