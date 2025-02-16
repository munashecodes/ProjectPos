import { Component, OnInit } from '@angular/core';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { OrderOption } from 'src/proxy/enums/order-option';
import { ReconFilter } from 'src/proxy/enums/recon-filter';
import { GroupedGrvItemsDto } from 'src/proxy/interfaces/grouped-grv-items-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { GoodsReceivedVoucherLineService } from 'src/proxy/services/goods-received-voucher-line.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';
import { UserService } from 'src/proxy/services/user.service';

@Component({
  selector: 'app-cost-of-goods-sold-report',
  templateUrl: './cost-of-goods-sold-report.component.html',
  styleUrls: ['./cost-of-goods-sold-report.component.scss']
})
export class CostOfGoodsSoldReportComponent implements OnInit {
 
  products: GroupedGrvItemsDto[] = [];

  filteredProducts: GroupedGrvItemsDto[] = [];

  totalCount = 0;

  totalSales = 0;

  openingCount = 0;

  openingCost = 0;

  closingCount = 0;

  closingCost = 0;

  balance = 0;

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
    private grvLinesService: GoodsReceivedVoucherLineService,
    private subCategoryService: SubCategoryService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    this.grvLinesService.getTodaySales()
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
      this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
      this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
      this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
      this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
      this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
      this.balance = this.openingCost + this.totalSales - this.closingCost;

      this.filteredProducts = this.products;
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
    this.openingCost = 0;
    this.openingCount = 0;
    this.closingCost = 0;
    this.closingCount = 0;
    this.balance = 0;

    var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;

    this.grvLinesService.getTodaySales()
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
      this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
      this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
      this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
      this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
      this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
      this.balance = this.openingCost + this.totalSales - this.closingCost;

      this.filteredProducts = this.products;
    })
  }

  getMonthSales() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.openingCost = 0;
    this.openingCount = 0;
    this.closingCost = 0;
    this.closingCount = 0;
    this.balance = 0;

    let mon = this.month.getMonth() + 1;
    var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;
    
    this.grvLinesService.getMonthAllItems(mon)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
      this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
      this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
      this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
      this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
      this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
      this.balance = this.openingCost + this.totalSales - this.closingCost;

      this.filteredProducts = this.products;
    })
  }

  getDateSales() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.openingCost = 0;
    this.openingCount = 0;
    this.closingCost = 0;
    this.closingCount = 0;
    this.balance = 0;

    this.date = new Date(new Date(this.date).getTime() + (2 * 60 * 60 * 1000)).toISOString();
    var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;
    
    this.grvLinesService.getAllItemsByDate(this.date)
    .subscribe(res => {
      console.log(res)
      this.products = res.data;
      this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
      this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
      this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
      this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
      this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
      this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
      this.balance = this.openingCost + this.totalSales - this.closingCost;

      this.filteredProducts = this.products;
    })
  }

  getDateRangeSales() {
    if(this.rangeDates![1]){
      this.totalCount = 0!;
      this.totalSales = 0;
      this.openingCost = 0;
      this.openingCount = 0;
      this.closingCost = 0;
      this.closingCount = 0;
      this.balance = 0;

      this.start = new Date(new Date(this.rangeDates![0]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      this.end = new Date(new Date(this.rangeDates![1]).getTime() + (2 * 60 * 60 * 1000)).toISOString();
      var id = this.recon === ReconFilter.USER ? this.tailor.id : 0;
      
      this.grvLinesService.getAllItemsByDateRange(this.start, this.end)
      .subscribe(res => {
        console.log(res)
        this.products = res.data;
        this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
        this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
        this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
        this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
        this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
        this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
        this.balance = this.openingCost + this.totalSales - this.closingCost;

        this.filteredProducts = this.products;
      })
    }
  }

  filterByCategory() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.openingCost = 0;
    this.openingCount = 0;
    this.closingCost = 0;
    this.closingCount = 0;
    this.balance = 0;

    this.filteredProducts = this.products.filter(product => product.subCategoryId === this.subCat.id);
    this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
    this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
    this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
    this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
    this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
    this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
    this.balance = this.openingCost + this.totalSales - this.closingCost;
  }

  onClearFilter() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.openingCost = 0;
    this.openingCount = 0;
    this.closingCost = 0;
    this.closingCount = 0;
    this.balance = 0;

    this.filteredSubCategories = this.subCategories;
    this.filteredProducts = this.products;
    this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
    this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
    this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
    this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
    this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
    this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
    this.balance = this.openingCost + this.totalSales - this.closingCost;
  }

  filterByDepartment() {
    this.totalCount = 0!;
    this.totalSales = 0;
    this.openingCost = 0;
    this.openingCount = 0;
    this.closingCost = 0;
    this.closingCount = 0;
    this.balance = 0;
    this.filteredProducts = this.products.filter(product => product.category === this.department);
    this.filteredSubCategories = this.subCategories.filter(subCat => subCat.category === this.department);
    this.totalCount = this.products.reduce((acc, product) => acc + product.quantity!, 0);
    this.totalSales = this.products.reduce((acc, product) => acc + product.totalCost!, 0);
    this.openingCount = this.products.reduce((acc, product) => acc + product.openingQuantity!, 0);
    this.openingCost = this.products.reduce((acc, product) => acc + product.openingStock!, 0);
    this.closingCount = this.products.reduce((acc, product) => acc + product.closingQuantity!, 0);
    this.closingCost = this.products.reduce((acc, product) => acc + product.closingStock!, 0);
    this.balance = this.openingCost + this.totalSales - this.closingCost;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

}
