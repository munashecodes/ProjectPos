import { filter } from 'rxjs';
import { Component } from '@angular/core';
import jspdf from 'jspdf';
import autoTable from 'jspdf-autotable';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { Grade } from 'src/proxy/enums/grade';
import { InvFilter } from 'src/proxy/enums/inv-filter';
import { Status } from 'src/proxy/enums/status';
import { TransactionType } from 'src/proxy/enums/transaction-type';
import { Unit } from 'src/proxy/enums/unit';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { ProductDto } from 'src/proxy/interfaces/product-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { StockMovementDto } from 'src/proxy/interfaces/stock-movement-dto';
import { StockTakeReportDto } from 'src/proxy/interfaces/stock-take-report-dto';
import { StockTakeValueReportDto } from 'src/proxy/interfaces/stock-take-value-report-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ProductService } from 'src/proxy/services/product.service';
import { StockMovementServiceService } from 'src/proxy/services/stock-movement-service.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';
import { ProductInventorySnapShotService } from 'src/proxy/services/product-inventory-snap-shot.service';
import { SnapShotEnum } from 'src/proxy/enums/snap-shot-enum';
import { InventorySnapShotSummaryDto } from 'src/proxy/interfaces/inventory-snap-shot-summary-dto';
import { StockTakeLogDto } from 'src/proxy/interfaces/stock-take-log-dto';

@Component({
  selector: 'app-stock-take',
  templateUrl: './stock-take.component.html',
  styleUrls: ['./stock-take.component.scss']
})
export class StockTakeComponent {

    newProduct: ProductInventoryDto = {
      product: {} as ProductDto
    } as ProductInventoryDto;
  
    productInventories: ProductInventoryDto[] = [];
  
    filteredFreshInventories: ProductInventoryDto[] = [];
  
    filteredDryInventories: ProductInventoryDto[] = [];
  
    filteredInventories: ProductInventoryDto[] = [];
  
    products: ProductDto[] = []
  
    freshProducts: ProductInventoryDto[] = []
  
    dryProducts: ProductInventoryDto[] = []
  
    companies: CompanyDto[] = [];
  
    createModal = false;
  
    editModal = false;
  
    isFresh = true;
  
    deleteModal = false;
  
    category!: Category;
    
    updadteInventoryModal = false;
  
    submitted = false;
  
    units: Unit[] = [];
  
    status: Status[] = [];
  
    cols: any[] = [];
  
    user: UserDto = {} as UserDto;
  
    dialogClass: string = '';
  
    categories: SubCategoryDto[] = [];
  
    dryCategories: SubCategoryDto[] = [];
  
    freshCategories: SubCategoryDto[] = [];
  
    subCategory: SubCategoryDto = {} as SubCategoryDto; 
  
    filter: InvFilter = InvFilter.ALL;
  
    filterOptions: InvFilter[] = Object.values(InvFilter);
  
    movements: StockMovementDto[] = [];
  
    selectedProducts: ProductInventoryDto[] = [];
  
    upgradeProducts: ProductInventoryDto[] = [];
  
    damagedProducts: ProductInventoryDto[] = [];
  
    selectedProductsRemove: ProductInventoryDto[] = [];
  
    quantityModal = false;
  
    quantityModal2 = false;
  
    assortedProduct: ProductInventoryDto = {} as ProductInventoryDto;
  
    selectedProduct: ProductInventoryDto = {} as ProductInventoryDto;
  
    upgradeProduct: ProductInventoryDto = {} as ProductInventoryDto;
  
    upgradeQuantity: number = 0;
  
    movementModal = false;
  
    damagesModal = false;
  
    upgradeModal = false;

    stockTakeReportModal = false;

    stockValueReportModal = false;

    department!: Category

    stockTakeReport: StockTakeReportDto[] = [];

    filteredStockTakeReport: StockTakeReportDto[] = [];

    stockValueReport: StockTakeValueReportDto[] = [];

    filteredStockValueReport: StockTakeValueReportDto[] = [];

    openingStock: number = 0;

    closingStock: number = 0;

    receivedStock: number = 0;

    soldStock: number = 0;

    damagedStock: number = 0;

    returnedStock: number = 0;

    quantityOnHand: number = 0;

    quantityOnShelf: number = 0;

    variance: number = 0;

    isFreshStockTake: boolean = false;

    isDryStockTake: boolean = false;
  
    constructor(
      private productService: ProductService,
      private inventoryService: ProductInventoryService,
      private catergoryService: SubCategoryService,
      private messageService: MessageService,
      private stockMovementService: StockMovementServiceService,
      private confirmationService: ConfirmationService,
      private inventorySnapShot: ProductInventorySnapShotService
    ) { }
  
    // life cycle hook
    ngOnInit(): void {
  
      this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

      this.inventoryService.checkStockTakeLog(Category.FreshProduce)
      .subscribe(res => {
        this.isFreshStockTake = res.isSuccess;
      });

      this.inventoryService.checkStockTakeLog(Category.DryGoods)
      .subscribe(res => {
        this.isDryStockTake = res.isSuccess;
      });
  
      // get all product Inventories from rhe database
      this.inventoryService.getAllList()
      .subscribe((res) => {
        console.log(res);
        this.productInventories = res.data;
        this.freshProducts = this.productInventories.filter(x => x.category == Category.FreshProduce);
        this.filteredFreshInventories = this.freshProducts;
        this.dryProducts = this.productInventories.filter(x => x.category == Category.DryGoods);
        this.filteredDryInventories = this.dryProducts;
      });
  
      //get all sub categories from the database
      this.catergoryService.getAllList()
      .subscribe((res) => {
        this.categories = res.data;
        this.freshCategories = this.categories.filter(x => x.category == Category.FreshProduce);
        this.dryCategories = this.categories.filter(x => x.category == Category.DryGoods);
        console.log(res);
      });
  
      // get all products from rhe database
      this.productService.getAllList()
      .subscribe((res) => {
        console.log(res);
        this.products = res.data;
      });
  
      // serialize enums
      this.units = Object.values(Unit);
    }

    onOpenStockTake(department: string){
      this.confirmationService.confirm({
        message: "Are you sure you want to open stocktake?",
        header: "Confirm",
        icon: "pi pi-exclamation-triangle",
        accept: () => {
          this.logStokeTake(department)
        },
        reject: () => {
        }
      });
    }

    onCloseStockTake(department: string){
      this.confirmationService.confirm({
        message: "Are you sure you want to close stocktake?",
        header: "Confirm",
        icon: "pi pi-exclamation-triangle",
        accept: () => {
          this.generateSnapShot(department);
        },
        reject: () => {
        }
      });
    }

    logStokeTake(department: string){
      let log = {
        department: Category[department as keyof typeof Category],
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      } as StockTakeLogDto

      this.inventoryService.createStockTakeLog(log)
      .subscribe(res => {
        if(res.isSuccess){
          
          this.isDryStockTake = department === 'DryGoods' ? true : this.isDryStockTake;
          this.isFreshStockTake = department === 'FreshProduce' ? true : this.isDryStockTake;
          
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

    closeStokeTakeLog(department: string){
      let log = {
        department: Category[department as keyof typeof Category],
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      } as StockTakeLogDto

      this.inventoryService.closeStockTakeLog(log)
      .subscribe(res => {
        if(res.isSuccess){
          
          this.isDryStockTake = department === 'DryGoods' ? false : this.isDryStockTake;
          this.isFreshStockTake = department === 'FreshProduce' ? false : this.isDryStockTake;

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

    generateSnapShot(department: string){
      let invSummary = {
        userId: this.user.id,
        snapShotType: SnapShotEnum.StockTake,
        department: department
      } as InventorySnapShotSummaryDto

      this.inventorySnapShot.create(invSummary)
          .subscribe(res => {
            if(res.isSuccess){
              
            this.closeStokeTakeLog(department)
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

    //reset stock take report
    onFilter(event: any){
      if(event.value === 'ALL'){
        this.filteredStockTakeReport = this.stockTakeReport;

        this.openingStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.openingStock;
                            }, 0);

        this.closingStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.closingStock;
                            }, 0);

        this.receivedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.receivedQuantity;
                            }, 0);

        this.soldStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.soldQuantity;
                            }, 0);

        this.damagedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.damagedQuantity;
                            }, 0);

        this.returnedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.returnedQuantity;
                            }, 0);

        this.quantityOnHand = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.quantityOnHand;
                            }, 0);

        this.quantityOnShelf = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.quantityOnShelf;
                            }, 0);

        this.variance = this.filteredStockTakeReport.reduce((accumulator, item) => {
                              return accumulator + item.variance;
                            }, 0);
      }
    }

    //filter stock take report
    filterStockTakeReport(event: any){
      this.filteredStockTakeReport = this.stockTakeReport.filter(x => x.subCategory === event.value.name);

      this.openingStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.openingStock;
                          }, 0);

      this.closingStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.closingStock;
                          }, 0);

      this.receivedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.receivedQuantity;
                          }, 0);

      this.soldStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.soldQuantity;
                          }, 0);

      this.damagedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.damagedQuantity;
                          }, 0);

      this.returnedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.returnedQuantity;
                          }, 0);

      this.quantityOnHand = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.quantityOnHand;
                          }, 0);

      this.quantityOnShelf = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.quantityOnShelf;
                          }, 0);

      this.variance = this.filteredStockTakeReport.reduce((accumulator, item) => {
                            return accumulator + item.variance;
                          }, 0);
    }

    // on generate stock take report
    generateStockTake(department: string){
      this.department = Category[department as keyof typeof Category];
      this.inventoryService.generateStockTakeReport(this.department)
      .subscribe((res) => {
        console.log(res);
        if(res.isSuccess){
          this.stockTakeReport = res.data;
          this.filteredStockTakeReport = this.stockTakeReport;

          this.openingStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.openingStock;
                              }, 0);

          this.closingStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.closingStock;
                              }, 0);

          this.receivedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.receivedQuantity;
                              }, 0);

          this.soldStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.soldQuantity;
                              }, 0);

          this.damagedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.damagedQuantity;
                              }, 0);

          this.returnedStock = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.returnedQuantity;
                              }, 0);

          this.quantityOnHand = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.quantityOnHand;
                              }, 0);

          this.quantityOnShelf = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.quantityOnShelf;
                              }, 0);

          this.variance = this.filteredStockTakeReport.reduce((accumulator, item) => {
                                return accumulator + item.variance;
                              }, 0);

          this.stockTakeReportModal = true;

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

    onFilterStockValue(event: any){
      if(event.value === 'ALL'){
        this.filteredStockValueReport = this.stockValueReport;  

        this.openingStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.openingStock;
                            }, 0);

        this.closingStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.closingStock;
                            }, 0);

        this.receivedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.receivedStock;
                            }, 0);

        this.soldStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.soldStock;
                            }, 0);

        this.damagedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.damagedStock;
                            }, 0);

        this.returnedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.returnedStock;
                            }, 0);

        this.quantityOnHand = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.stockOnHand;
                            }, 0);

        this.quantityOnShelf = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.stockOnShelf;
                            }, 0);

        this.variance = this.filteredStockValueReport.reduce((accumulator, item) => {
                              return accumulator + item.variance;
                            }, 0);
      }
    }

    filterStockValueReport(event: any){
      this.filteredStockValueReport = this.stockValueReport.filter(x => x.subCategory === event.value.name);  

          this.openingStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.openingStock;
                              }, 0);

          this.closingStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.closingStock;
                              }, 0);

          this.receivedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.receivedStock;
                              }, 0);

          this.soldStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.soldStock;
                              }, 0);

          this.damagedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.damagedStock;
                              }, 0);

          this.returnedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.returnedStock;
                              }, 0);

          this.quantityOnHand = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.stockOnHand;
                              }, 0);

          this.quantityOnShelf = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.stockOnShelf;
                              }, 0);

          this.variance = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.variance;
                              }, 0);
    }

     // on generate stock take report
    generateStockTakeValue(department: string){
      this.department = Category[department as keyof typeof Category];
      this.inventoryService.generateStockValueReport(this.department)
      .subscribe((res) => {
        console.log(res);
        if(res.isSuccess){
          this.stockValueReport = res.data;
          this.filteredStockValueReport = this.stockValueReport;

          this.openingStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.openingStock;
                              }, 0);

          this.closingStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.closingStock;
                              }, 0);

          this.receivedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.receivedStock;
                              }, 0);

          this.soldStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.soldStock;
                              }, 0);

          this.damagedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.damagedStock;
                              }, 0);

          this.returnedStock = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.returnedStock;
                              }, 0);

          this.quantityOnHand = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.stockOnHand;
                              }, 0);

          this.quantityOnShelf = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.stockOnShelf;
                              }, 0);

          this.variance = this.filteredStockValueReport.reduce((accumulator, item) => {
                                return accumulator + item.variance;
                              }, 0);

          this.stockValueReportModal = true;

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
  
    onUgrade(){
      this.upgradeModal = true;
    }
  
    onSelectUpgrade(){
      this.upgradeProducts = this.freshProducts.filter(x => x.productId === this.selectedProduct.id);
    }
  
    onSaveUpgrade(){
  
      let movement1: StockMovementDto = {
        productInventoryId: this.upgradeProduct.id,
        productName: this.upgradeProduct.name,
        barCode: this.upgradeProduct.barCode,
        quantity: this.upgradeQuantity,
        transactionType: TransactionType.Upgrade,
        isAuthorised: false,
        comment: '',
        creationTime: new Date(),
        lastModificationTime: new Date(),
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      }
  
      this.movements.push(movement1);
  
      let movement: StockMovementDto = {
        productInventoryId: this.selectedProduct.id,
        productName: this.selectedProduct.name,
        barCode: this.selectedProduct.barCode,
        quantity: 0 - this.upgradeQuantity,
        transactionType: TransactionType.DownGrade,
        isAuthorised: false,
        comment: '',
        creationTime: new Date(),
        lastModificationTime: new Date(),
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      }
  
      this.movements.push(movement);
  
      this.stockMovementService.createRange(this.movements)
      .subscribe((res) => {
        console.log(res);
        if(res.isSuccess){
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
  
      this.upgradeModal = false;
      this.movements = [];
    }
  
    processDamages(){
      this.damagedProducts.forEach(product => {
        //initialize the stock movement object
        let movement: StockMovementDto = {
          productInventoryId: product.id,
          productName: product.name,
          barCode: product.barCode,
          quantity: 0,
          transactionType: TransactionType.Breakage,
          isAuthorised: false,
          comment: '',
          creationTime: new Date(),
          lastModificationTime: new Date(),
          creatorId: this.user.id,
          lastModifierUserId: this.user.id
        }
  
        //push the stock movement object to the movements array
        this.movements.push(movement);
      });
  
      this.damagesModal = true;
    }
  
    onSaveDamages(){
  
      this.movements.forEach(movement => {
        movement.quantity = 0 - movement.quantity!;
      })
      //send the stock movement array to the server
      this.stockMovementService.createRange(this.movements)
      .subscribe((res) => {
        console.log(res);
        if(res.isSuccess){
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
  
      //clear the movements array
      this.movements = [];
      //clear the selected products array
      this.damagedProducts = [];
      this.damagesModal = false;
    }
  
    //add stock movement
    addStockMovement(){
      //initialize the stock movement object
      let movement: StockMovementDto = {
        productInventoryId: this.assortedProduct.id,
        quantity: this.assortedProduct.quantityOnHand!,
        transactionType: TransactionType.Assorted,
        isAuthorised: false,
        comment: '',
        creationTime: new Date(),
        lastModificationTime: new Date(),
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      }
  
      //push the stock movement object to the movements array
      this.movements.push(movement);
  
      //loop through the selected products
      this.selectedProducts.forEach(product => {
        //initialize the stock movement object
        let movement: StockMovementDto = {
          productInventoryId: product.id,
          quantity: 0 - product.quantityOnHand!,
          transactionType: TransactionType.Assorted,
          isAuthorised: false,
          comment: '',
          creationTime: new Date(),
          lastModificationTime: new Date(),
          creatorId: this.user.id,
          lastModifierUserId: this.user.id
        }
  
        //push the stock movement object to the movements array
        this.movements.push(movement);
  
      });
      
        //send the stock movement array to the server
        this.stockMovementService.createRange(this.movements)
        .subscribe((res) => {
          console.log(res);
          if(res.isSuccess){
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
  
        //clear the movements array
        this.movements = [];
        //clear the selected products array
        this.selectedProducts = [];
        //clear the assorted product object
        this.assortedProduct = {} as ProductInventoryDto;
        this.movementModal = false;
    }
  
    onAssortProduct(){
      this.movementModal = true;
    }
  
    //add selected product to the selected products array
    addProduct(){
      this.selectedProduct.quantityOnHand = undefined
      this.quantityModal2 = true;
    }
  
    assortedQuantity(){
      this.assortedProduct.quantityOnHand = undefined
      this.quantityModal = true;
    }
  
    assortedAtyClose(){
      this.quantityModal = false;
    }
  
    confirmAddProduct(){
      this.selectedProducts.push(this.selectedProduct);
      this.quantityModal2 = false;
      this.selectedProduct = {} as ProductInventoryDto;
    }
  
    //show all products
    showAll(){
      if(this.filter === InvFilter.ALL){
        this.filteredFreshInventories = this.freshProducts;
      }
    }
  
    // print the selected items
    printStockCountSheet(fresh: boolean){
      console.log(fresh)
      if(fresh){
        this.filteredInventories = this.filteredFreshInventories;
        console.log(this.filteredInventories)
      }
      else{
        this.filteredInventories = this.filteredDryInventories;
      }
      let pdf = new jspdf('p', 'pt', 'a4');
      pdf.setFontSize(16);
      pdf.text('Andile Fresh Farm Produce', 10, 15);
  
      pdf.setFontSize(14);
      pdf.text('Stock Count Sheet ' + this.subCategory.name?.toUpperCase(), 10, 30);
      pdf.line(10, 35, 565, 35, 'F');
  
      //table
      pdf.setFontSize(14);
  
      autoTable(pdf, {
        headStyles: { fillColor: [200, 200, 200], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: {lineWidth: 0.1 },
        footStyles: { fillColor: [200, 200, 200], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [['Bar Code', 'Product','Price', 'Qty', 'Unit']],
        body: this.filteredInventories.map(x => 
          [
            x.barCode!,
            x.name!,
            x.productPrice?.price!,
            '',
            x.unit!
          ]),
        foot: [[
          '', 
          'Total', 
          this.filteredInventories.map(x => x.quantityOnHand!).reduce((a, b) => a + b, 0)
        ]],
        startY: 40,
        margin: 10,
        theme: 'striped',
        styles: {
          fontSize: 10,
          cellPadding: 2,
          overflow: 'linebreak',
        },
      });
  
      const addFooter = (pdfInstance: jspdf) => {
      }
      const pageCount = pdf.getNumberOfPages();
  
      addFooter(pdf);
      for (let i = 1; i <= pageCount; i++) {
        pdf.setPage(i);
        addFooter(pdf);
      }
      pdf.save("Stock Sheet" + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
    }

    // print the selected items
    printStockCountSheet100(fresh: boolean){
      console.log(fresh)
      if(fresh){
        this.filteredInventories = this.filteredFreshInventories;
        console.log(this.filteredInventories)
      }
      else{
        this.filteredInventories = this.filteredDryInventories;
      }

      let baseLength = 25 + (this.filteredInventories.length*12);
      let pdf = new jspdf('p', 'mm', [100, baseLength]);
      pdf.setFontSize(8);
      pdf.text('Andile Fresh Farm Produce', 5, 5);
  
      pdf.setFontSize(8);
      pdf.text('Stock Count Sheet ' + this.subCategory.name?.toUpperCase(), 5, 10);
      pdf.line(5, 15, 95, 15, 'F');
  
      //table
      pdf.setFontSize(11);
  
      autoTable(pdf, {
        headStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1},
        bodyStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1 },
        footStyles: { fillColor: [255, 255, 255], textColor: [0, 0, 0], fontStyle: 'bold', lineWidth: 0.1, halign: 'right'},
        head: [['Bar Code', 'Product', 'Qty']],
        body: this.filteredInventories.map(x => 
          [
            x.barCode!,
            x.name!,
            ''
          ]),
        // foot: [[
        //   '', 
        //   'Total', 
        //   this.filteredInventories.map(x => x.quantityOnHand!).reduce((a, b) => a + b, 0)
        // ]],
        startY: 20,
        margin: 5,
        theme: 'striped',
        styles: {
          fontSize: 9,
          cellPadding: 2,
          overflow: 'linebreak',
        },
      });
  
      const addFooter = (pdfInstance: jspdf) => {
      }
      const pageCount = pdf.getNumberOfPages();
  
      addFooter(pdf);
      for (let i = 1; i <= pageCount; i++) {
        pdf.setPage(i);
        addFooter(pdf);
      }
      pdf.save("Stock Sheet" + new Date().toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + 'invoice.pdf');
    }
  
    updateFreshInventory(){
      this.filteredInventories = [];
      this.filteredInventories = this.filteredFreshInventories;
      this.filteredInventories.forEach(x => {
        x.stockCount = 0;
      });
      this.updadteInventoryModal = true;
    }
  
    updateDryInventory(){
      this.filteredInventories = [];
      this.filteredInventories = this.filteredDryInventories;
      this.updadteInventoryModal = true;
    }
  
    //filter products by subcategory
    filterFreshProducts(){
      this.filteredFreshInventories = this.freshProducts.filter(x => x.product?.subCategoryId === this.subCategory.id);
    }
  
    filterDryProducts(){
      this.filteredDryInventories = this.dryProducts.filter(x => x.product?.subCategoryId === this.subCategory.id);
    }
  
    fresh(){
      this.isFresh = true;
      this.newProduct.grade = Grade.None;
    }
  
    dry(){
      this.isFresh = false;
      this.newProduct.grade = Grade.None;
    }
  
    // open create modal
    create(){
      this.createModal = true;
      this.submitted = false;
    }
  
  
    updateRange(){
      this.submitted = true;

      this.filteredInventories.forEach(prod =>
        prod.quantity = Number(prod.quantity)
      )
       let filtered = this.filteredInventories.filter(prod => prod.quantity !== 0)
  
      // send
      this.inventoryService.updateRange(filtered)
      .subscribe((res) => {
        console.log(res);
        if(res.isSuccess){
  
          if(!Array.isArray(this.productInventories)){
            this.productInventories = [];
            this.productInventories = [...this.productInventories, res.data];
          }
          else{
            this.productInventories = [...this.productInventories, res.data];
            this.freshProducts = this.productInventories.filter(x => x.category == Category.FreshProduce);
            this.filteredFreshInventories = this.freshProducts;
            this.dryProducts = this.productInventories.filter(x => x.category == Category.DryGoods);
            this.filteredDryInventories = this.dryProducts;
          }
  
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
      
      this.createModal = false;
      this.editModal = false;
      this.deleteModal = false;
      this.updadteInventoryModal = false;
      
    }
  
    hideDialog(){
      this.createModal = false;
      this.movementModal = false;
      this.quantityModal = false;
      this.quantityModal2 = false;
      this.updadteInventoryModal = false;
      this.editModal = false;
      this.deleteModal = false;
      this.isFresh = true;
      this.damagesModal = false;
      this.newProduct = {
        product: {} as ProductDto
      } as ProductInventoryDto;
    }
  
    onGlobalFilter(table: Table, event: Event) {
      table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }
}
