import { StockMovementLogDto } from './../../../proxy/interfaces/stock-movement-log-dto';
import { Component } from '@angular/core';
import jspdf from 'jspdf';
import autoTable from 'jspdf-autotable';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { Country } from 'src/proxy/enums/country';
import { Grade } from 'src/proxy/enums/grade';
import { InvFilter } from 'src/proxy/enums/inv-filter';
import { Status } from 'src/proxy/enums/status';
import { TransactionType } from 'src/proxy/enums/transaction-type';
import { Unit } from 'src/proxy/enums/unit';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { ProductDto } from 'src/proxy/interfaces/product-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { StockMovementDto } from 'src/proxy/interfaces/stock-movement-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ProductService } from 'src/proxy/services/product.service';
import { StockMovementServiceService } from 'src/proxy/services/stock-movement-service.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';

@Component({
  selector: 'app-product-inventory',
  templateUrl: './product-inventory.component.html',
  styleUrls: ['./product-inventory.component.scss']
})

export class ProductInventoryComponent {

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

  subCategories: SubCategoryDto[] = [];

  filter: InvFilter = InvFilter.ALL;

  filterOptions: InvFilter[] = Object.values(InvFilter);

  movements: StockMovementDto[] = [];

  selectedProducts: ProductInventoryDto[] = [];

  upgradeProducts: ProductInventoryDto[] = [];

  damagedProducts: ProductInventoryDto[] = [];

  selectedProductsRemove: ProductInventoryDto[] = [];

  filteredProducts: ProductInventoryDto[] = [];

  quantityModal = false;

  quantityModal2 = false;

  batch: StockMovementLogDto = {} as StockMovementLogDto;

  assortedProduct: ProductInventoryDto = {} as ProductInventoryDto;

  selectedProduct: ProductInventoryDto = {} as ProductInventoryDto;

  upgradeProduct: ProductInventoryDto = {} as ProductInventoryDto;

  upgradeQuantity: number = 0;

  caseSize: number = 0;

  quantity: number = 0;

  movementModal = false;

  moveCaseModal = false;

  damagesModal = false;

  upgradeModal = false;

  categoryModal = false;

  constructor(
    private productService: ProductService,
    private inventoryService: ProductInventoryService,
    private catergoryService: SubCategoryService,
    private messageService: MessageService,
    private stockMovementService: StockMovementServiceService
  ) { }

  // life cycle hook
  ngOnInit(): void {

    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

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

  onUgrade(department: string){
    this.filteredProducts = this.productInventories.filter(x => x.category == Category[department as keyof typeof Category]);
    this.moveCaseModal = true;
  }

  onSelectUpgrade(){
    this.upgradeProducts = this.freshProducts.filter(x => x.productId === this.selectedProduct.id);
  }

  onSaveUpgrade(){

    let movement1: StockMovementDto = {
      productInventoryId: this.upgradeProduct.id,
      productName: this.upgradeProduct.name,
      barCode: this.upgradeProduct.barCode,
      quantity: this.caseSize*this.quantity,
      transactionType: TransactionType.Upgrade,
      isAuthorised: false,
      comment: '',
      creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      creatorId: this.user.id,
      lastModifierUserId: this.user.id
    }

    this.movements.push(movement1);

    let movement: StockMovementDto = {
      productInventoryId: this.selectedProduct.id,
      productName: this.selectedProduct.name,
      barCode: this.selectedProduct.barCode,
      quantity: 0 - this.quantity,
      transactionType: TransactionType.DownGrade,
      isAuthorised: false,
      comment: '',
      creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      creatorId: this.user.id,
      lastModifierUserId: this.user.id
    }

    this.movements.push(movement);

    this.batch = {
      transactionType: TransactionType.Upgrade,
      isssuedTo: 'Upgrade',
      isAuthorised: false,
      stockMovements: this.movements,
      creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      creatorId: this.user.id,
      lastModifierUserId: this.user.id
    }

    this.stockMovementService.create(this.batch)
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

    this.moveCaseModal = false;
    this.selectedProduct = {} as ProductInventoryDto;
    this.upgradeProduct = {} as ProductInventoryDto;
    this.batch = {} as StockMovementLogDto;
    this.movements = [];
  }

  onUpdateCategory(department: string){
    this.subCategories = this.categories.filter(x => x.category == Category[department as keyof typeof Category]);
    this.categoryModal = true;
  }

  onChangeCategory(){
    this.damagedProducts.forEach(product => {
      product.subCategoryName = this.subCategory.name;
      product.product!.subCategoryId = this.subCategory.id;
    });
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
        creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
        lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      }

      //push the stock movement object to the movements array
      this.movements.push(movement);

    });

    this.batch = {
      transactionType: TransactionType.Breakage,
      isssuedTo: 'Breakage',
      isAuthorised: false,
      stockMovements: this.movements,
      creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      creatorId: this.user.id,
      lastModifierUserId: this.user.id
    }
    
    this.damagesModal = true;
  }

  onSaveDamages(){

    this.batch.stockMovements!.forEach(movement => {
      movement.quantity = 0 - movement.quantity!;
    })

    //send the stock movement array to the server
    this.stockMovementService.create(this.batch)
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
    this.batch = {} as StockMovementLogDto;
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
      creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
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
        creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
        lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
        creatorId: this.user.id,
        lastModifierUserId: this.user.id
      }

      //push the stock movement object to the movements array
      this.movements.push(movement);

    });

    this.batch = {
      transactionType: TransactionType.Assorted,
      isssuedTo: 'Assorted',
      isAuthorised: false,
      stockMovements: this.movements,
      creationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      lastModificationTime: new Date(new Date().getTime() + 2 * 60 * 60 * 1000),
      creatorId: this.user.id,
      lastModifierUserId: this.user.id
    }
    
      //send the stock movement array to the server
      this.stockMovementService.create(this.batch)
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
      this.batch = {} as StockMovementLogDto;
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
    pdf.text('T&T SOLA', 10, 15);

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
    this.filteredFreshInventories = this.freshProducts.filter(x => x.subCategoryId === this.subCategory.id);
  }

  filterDryProducts(){
    this.filteredDryInventories = this.dryProducts.filter(x => x.subCategoryId === this.subCategory.id);
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

  // open edit modal
  edit(product: ProductDto){
    // initialize product to fill the edit form
    this.newProduct = { ...product};
    console.log(this.newProduct)
    this.editModal = true;
    this.submitted = false;
  }

  // open delete modal
  delete(product: ProductInventoryDto){
    // initialize product
    this.newProduct = { ...product};
    this.deleteModal = true;
    this.submitted = false;
  }

  // open delete modal to delete multiple selected items
  deleteSelected(){
  }

  // scalled save button event on create modal
  save(){
    this.submitted = true;

    this.newProduct.creationTime = new Date();
    this.newProduct.creatorId = this.user.id;
    this.newProduct.lastModificationTime = new Date();
    this.newProduct.lastModifierUserId = this.user.id;
    this.newProduct.productId = this.newProduct.product?.id
    this.newProduct.status = Status.OutOfStock;
    this.newProduct.name = this.newProduct.product?.name;
    this.newProduct.quantityOnHand = 0;
    this.newProduct.quantityOnShelf = 0;
    this.newProduct.barCode = this.newProduct.barCode?.toString();
    this.newProduct.pLUCode = this.newProduct.barCode!.substring(2, 6);
    this.newProduct.flag = Number(this.newProduct.barCode!.substring(0, 1));
    this.newProduct.isWeighted = this.newProduct.unit === Unit.KG ? true : false;
    
    // send
    this.inventoryService.create(this.newProduct)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.productInventories)){
          this.productInventories = []
        }
        else{
          this.productInventories = [...this.productInventories, res.data];
          this.freshProducts = this.productInventories.filter(x => x.category == Category.FreshProduce);
          this.dryProducts = this.productInventories.filter(x => x.category == Category.DryGoods);
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
  }

  updateRange(){
    this.submitted = true;

    // send
    this.inventoryService.updateRange(this.filteredInventories)
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
          life: 7000
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
    this.hideDialog();
    
  }

  hideDialog(){
    this.createModal = false;
    this.damagedProducts = [];
    this.moveCaseModal = false;
    this.movementModal = false;
    this.quantityModal = false;
    this.quantityModal2 = false;
    this.updadteInventoryModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.isFresh = true;
    this.damagesModal = false;
    this.upgradeModal = false;
    this.categoryModal = false;
    this.newProduct = {
      product: {} as ProductDto
    } as ProductInventoryDto;
  }

  confirmDelete(){
    this.inventoryService.delete(this.newProduct.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.productInventories)){
          this.productInventories = []
        }
        else{
          var index = this.productInventories.findIndex(x => x.id === this.newProduct.id);
          this.productInventories.splice(index, 1)

          this.freshProducts = this.productInventories.filter(x => x.category == Category.FreshProduce);
          this.dryProducts = this.productInventories.filter(x => x.category == Category.DryGoods);
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

    this.deleteModal = false;
  }

  update(){
    this.submitted = true;
    this.newProduct.barCode = this.newProduct.barCode?.toString();
    this.newProduct.pLUCode = this.newProduct.barCode!.substring(2, 6);
    this.newProduct.flag = Number(this.newProduct.barCode!.substring(0, 1));
    this.newProduct.isWeighted = this.newProduct.unit === Unit.KG ? true : false;

    this.inventoryService.update(this.newProduct)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.productInventories)){
          this.productInventories = []
        }
        
        const updatedItem = res.data; // The updated item
        this.freshProducts = this.freshProducts.map(item => item.id === updatedItem.id ? updatedItem : item);
        //this.freshProducts = this.productInventories.filter(x => x.category == Category.FreshProduce);
        this.dryProducts = this.productInventories.filter(x => x.category == Category.DryGoods);

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
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
}
