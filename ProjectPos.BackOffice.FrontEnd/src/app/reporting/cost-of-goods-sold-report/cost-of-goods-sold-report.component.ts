import { Component, OnInit } from '@angular/core';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { OrderOption } from 'src/proxy/enums/order-option';
import { ReconFilter } from 'src/proxy/enums/recon-filter';
import { CogsReport, CostOfGoodsReport, CostOfGoodsReportItem } from 'src/proxy/interfaces/cogs-report-dtos';
import { GroupedGrvItemsDto } from 'src/proxy/interfaces/grouped-grv-items-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { GoodsReceivedVoucherLineService } from 'src/proxy/services/goods-received-voucher-line.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ReportFilter, ReportService } from 'src/proxy/services/report.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';
import { UserService } from 'src/proxy/services/user.service';

@Component({
  selector: 'app-cost-of-goods-sold-report',
  templateUrl: './cost-of-goods-sold-report.component.html',
  styleUrls: ['./cost-of-goods-sold-report.component.scss']
})
export class CostOfGoodsSoldReportComponent implements OnInit {
  rangeDates: any[] = []
  filter: ReportFilter = {};
  soldReport: CogsReport = {} as CogsReport;
  products: ProductInventoryDto[] = [];
  report: CostOfGoodsReport = {} as CostOfGoodsReport; // Your report data
  categories = Category; 
  subCategories: SubCategoryDto[] = [];
  filteredSubCategories: SubCategoryDto[] = [];
  selectedCategory: Category = {} as Category;
  estimatedProfit: number = 0;
  selectedSubCategory: SubCategoryDto = {} as SubCategoryDto;
  filteredItems:CostOfGoodsReportItem[] = [];
  timeRanges = [
    'today',
    'month',
    'year',
    'custom',
    'date'

  ]
  constructor(
    private reportService: ReportService,
     private productInventoryService: ProductInventoryService,
        private categoryService: SubCategoryService,
  ) {
    
  }
  ngOnInit(): void {
    this.filter = {
      startDate: new Date(new Date().getTime() + 2 *  60 * 60 * 1000).toISOString().split('T')[0],
      endDate: new Date(new Date().getTime() + 2 * 60 * 60 * 1000).toISOString().split('T')[0],
      timeRange: 'today'
    }
    this.loadReport();
    this.loadReportSold()
    this.productInventoryService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.products = res.data;
    });

    // get all subCategories from rhe database
    this.categoryService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.subCategories = res.data;
    });
  }

  loadReport() {
    this.reportService.getCostOfGoodsReport().subscribe(res => {
      this.report = res.data || {} as CostOfGoodsReport
      this.filteredItems = this.report.items;
      this.estimatedProfit = this.report.totalEstimatedProfit;
    }
    );
  }

  loadReportSold() {
    this.reportService.getCogsReport(this.filter).subscribe(res => this.soldReport = res.data || {} as CogsReport);
  }

  onCategoryChange() {
    // Filter subcategories based on the selected category
    this.filteredSubCategories = this.subCategories
      .filter(subCat => subCat.category === this.selectedCategory);
  }

  applyFilters() {
    
    this.filteredItems = this.report.items.filter(item => {
      const matchesSubCategory = this.selectedSubCategory 
        ? item.subCategoryId === this.selectedSubCategory.id 
        : true;

      return matchesSubCategory;
    });

    this.estimatedProfit = this.filteredItems.reduce((acc, cur) => acc + cur.estimatedProfit, 0);
  }

  resetFilters() {
    // Reset the filters and show all items
    this.selectedCategory = {} as Category;
    this.selectedSubCategory = {} as SubCategoryDto;
    this.filteredItems = this.report.items; // Reset to all items
    this.estimatedProfit = this.report.totalEstimatedProfit;
    this.filteredSubCategories = []; // Clear subcategory options
  }
  

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  onTimeRangeChange() {
    // Logic to handle time range selection
    if (this.filter.timeRange === 'custom') {
      this.filter.startDate = this.rangeDates[0] || null;
      this.filter.endDate = this.rangeDates[1] || null;
    } else {
      this.filter.startDate = null;
      this.filter.endDate = null;
    }
  }

}
