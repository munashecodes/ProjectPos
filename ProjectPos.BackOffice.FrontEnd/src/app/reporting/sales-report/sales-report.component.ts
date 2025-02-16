import { Component, OnInit } from '@angular/core';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { OrderOption } from 'src/proxy/enums/order-option';
import { ReconFilter } from 'src/proxy/enums/recon-filter';
import { GroupedGrvItemsDto } from 'src/proxy/interfaces/grouped-grv-items-dto';
import { SalesOrderItemDto } from 'src/proxy/interfaces/sales-order-item-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { GoodsReceivedVoucherLineService } from 'src/proxy/services/goods-received-voucher-line.service';
import { SalesOrderService } from 'src/proxy/services/sales-order.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';
import { UserService } from 'src/proxy/services/user.service';

@Component({
  selector: 'app-sales-report',
  templateUrl: './sales-report.component.html',
  styleUrls: ['./sales-report.component.scss']
})
export class SalesReportComponent implements OnInit {
  
  products: SalesOrderItemDto[] = [];

  filteredProducts: SalesOrderItemDto[] = [];

  totalCount = 0;

  totalSales = 0;

  totalPurchases = 0;

  user: UserDto = {} as UserDto;

  tailor: UserDto = {} as UserDto;

  tailors: UserDto[] = [];

  reconFilters: ReconFilter[] = Object.values(ReconFilter);

  recon: ReconFilter = ReconFilter.ALL;

  month = new Date();

  date = new Date().toISOString();

  start = new Date().toISOString();

  end = new Date().toISOString();
  
  rangeDates: Date[] | undefined;

  subCat: SubCategoryDto = {} as SubCategoryDto;

  subCategories: SubCategoryDto[] = [];

  filteredSubCategories: SubCategoryDto[] = [];

  filterOptions: OrderOption[] = Object.values(OrderOption);

  option = OrderOption.TODAY;
  
  cols: any[] = [];

  departments: Category[] = Object.values(Category);

  department: Category = Category.DryGoods;
  

  constructor(
    private salesOrderService: SalesOrderService,
    private grvLinesService: GoodsReceivedVoucherLineService,
    private subCategoryService: SubCategoryService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    this.salesOrderService.getTodaySales(0)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })

      this.filteredProducts = this.products;
    })

    this.grvLinesService.getTodaySales()
    .subscribe(res => {
      console.log(res)
      res.data.forEach((product: GroupedGrvItemsDto) => {
        this.totalPurchases += product.totalCost!;
      })
    })
    

    this.subCategoryService.getAllList()
    .subscribe(res => {
      console.log(res)
      this.subCategories = res.data;
      this.filteredSubCategories = this.subCategories;
    })

    this.userService.getAllList()
    .subscribe(res => {
      console.log(res)
      this.tailors = res.data;
    })
  }

  onMainFilter(){
    if(this.option === OrderOption.TODAY){
      this.getTodaySales();
    }
  }

  getTodaySales() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.totalPurchases = 0;
    var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;

    this.salesOrderService.getTodaySales(id!)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })

      this.filteredProducts = this.products;
    })

    this.grvLinesService.getTodaySales()
    .subscribe(res => {
      console.log(res)
      res.data.forEach((product: GroupedGrvItemsDto) => {
        this.totalPurchases += product.totalCost!;
      })
    })
  }

  getMonthSales() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.totalPurchases = 0;
    let mon = this.month.getMonth() + 1;
    var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;
    
    this.salesOrderService.getMonthAllItems(mon, id!)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })

      this.filteredProducts = this.products;
    })

    this.grvLinesService.getMonthAllItems(mon)
    .subscribe(res => {
      console.log(res)
      res.data.forEach((product: GroupedGrvItemsDto) => {
        this.totalPurchases += product.totalCost!;
      })
    })
  }

  getDateSales() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.totalPurchases = 0;
    this.date = new Date(new Date(this.date).getTime() + (2 * 60 * 60 * 1000)).toISOString();
    var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;
    
    this.salesOrderService.getAllItemsByDate(this.date, id!)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.products.forEach(product => {
        this.totalCount += product.quantity!;
        this.totalSales += product.price!;
      })

      this.filteredProducts = this.products;
    })

    this.grvLinesService.getAllItemsByDate(this.date)
    .subscribe(res => {
      console.log(res)
      res.data.forEach((product: GroupedGrvItemsDto) => {
        this.totalPurchases += product.totalCost!;
      })
    })
  }

  getDateRangeSales() {
    if(this.rangeDates![1]){
      this.totalCount = 0!;
      this.totalSales = 0;
      this.totalPurchases = 0;
      this.start = new Date(new Date(this.rangeDates![0]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      this.end = new Date(new Date(this.rangeDates![1]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;
      
      this.salesOrderService.getAllItemsByDateRange(this.start, this.end, id!)
      .subscribe(res => {
        console.log(res)
        this.products = res.data;
        this.products.forEach(product => {
          this.totalCount += product.quantity!;
          this.totalSales += product.price!;
        })

        this.filteredProducts = this.products;
      })

      this.grvLinesService.getAllItemsByDateRange(this.start, this.end)
      .subscribe(res => {
        console.log(res)
        res.data.forEach((product: GroupedGrvItemsDto) => {
          this.totalPurchases += product.totalCost!;
        })
      })
    }
  }

  filterByCategory() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.filteredProducts = this.products.filter(product => product.subCategoryId === this.subCat.id);
    this.filteredProducts.forEach(product => {
      this.totalCount += product.quantity!;
      this.totalSales += product.price!;
    })
  }

  onClearFilter() {
    this.totalCount = 0!;
    this.totalSales = 0;

    this.filteredSubCategories = this.subCategories;
    this.filteredProducts = this.products;
    this.filteredProducts.forEach(product => {
      this.totalCount += product.quantity!;
      this.totalSales += product.price!;
    })
  }

  filterByDepartment() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.filteredProducts = this.products.filter(product => product.category === this.department);
    this.filteredSubCategories = this.subCategories.filter(subCat => subCat.category === this.department);
    this.filteredProducts.forEach(product => {
      this.totalCount += product.quantity!;
      this.totalSales += product.price!;
    })
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
