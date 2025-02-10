import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { Country } from 'src/proxy/enums/country';
import { Grade } from 'src/proxy/enums/grade';
import { Status } from 'src/proxy/enums/status';
import { Unit } from 'src/proxy/enums/unit';
import { AddressDto } from 'src/proxy/interfaces/address-dto';
import { CompanyDto } from 'src/proxy/interfaces/company-dto';
import { ProductInventoryDto } from 'src/proxy/interfaces/product-inventory-dto';
import { SubCategoryDto } from 'src/proxy/interfaces/sub-category-dto';
import { UserDto } from 'src/proxy/interfaces/user-dto';
import { FileService } from 'src/proxy/services/file.service';
import { ProductInventoryService } from 'src/proxy/services/product-inventory.service';
import { ProductService } from 'src/proxy/services/product.service';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  newProduct: ProductInventoryDto = {} as ProductInventoryDto;

  products: ProductInventoryDto[] = [];

  grades: Grade[] = [];

  companies: CompanyDto[] = [];

  isFresh = true;

  createModal = false;

  subCategory: SubCategoryDto = {} as SubCategoryDto

  editModal = false;

  deleteModal = false;

  submitted = false;

  units = Unit;

  countries: Country[] = [];

  country!: Country;

  cols: any[] = [];

  dialogClass: string = '';

  categories: Category[] = [];

  subCategories: SubCategoryDto[] = [];

  filteredSubCategories: SubCategoryDto[] = [];

  user: UserDto = {} as UserDto;

  file!: File;

  isTaxable: any;

  categoryModal = false;

  constructor(
    private productInventoryService: ProductInventoryService,
    private categoryService: SubCategoryService,
    private fileService: FileService,
    private messageService: MessageService,
    private inventoryService: ProductInventoryService
  ) { }

  // life cycle hook
  ngOnInit(): void {

    this.user = JSON.parse(sessionStorage.getItem('loggedUser') || '{}');

    // get all products from rhe database
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

    // serialize enums
    this.countries = Object.values(Country);
    this.categories = Object.values(Category);
    this.grades = Object.values(Grade);
  }

 

  fresh(){
    this.isFresh = false;
    this.newProduct.grade = Grade.None;
  }

  //stringToImage
  stringToImage(base64String: string){
    if (!base64String) {
      return null;
    }

    const image = new Image();
    image.src = base64String;
    return image;
  }

  // open create modal
  create(){
    // this.newProduct = {} as ProductInventoryDto;
    // this.isFresh = false;
    // this.createModal = true;
    // this.submitted = false;

    this.inventoryService.generateInventory()
    .subscribe(res => {
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
  }

  createFresh(){
    this.newProduct = {} as ProductInventoryDto;
    this.isFresh = true;
    this.createModal = true;
    this.submitted = false;
  }

  //filter subCategories
  filterCategories(event: any){
    this.filteredSubCategories = [];
    var cat = event.value;
    this.filteredSubCategories = this.subCategories.filter(x => x.category === cat)
  }

  // open edit modal
  edit(product: ProductInventoryDto){
    this.newProduct = {} as ProductInventoryDto;
    this.isFresh = false;
    // initialize product to fill the edit form
    this.newProduct = { ...product};
    this.filteredSubCategories = this.subCategories
    this.subCategory = this.subCategories.filter(c => c.id === this.newProduct.subCategoryId)[0]
    this.editModal = true;
    this.submitted = false;
  }

  editFresh(product: ProductInventoryDto){
    this.newProduct = {} as ProductInventoryDto;
    this.isFresh = true;
    // initialize product to fill the edit form
    this.newProduct = { ...product};
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

  saveFile(){
    
    if(this.newProduct.id){
      this.update()
    }
    else{
      this.save();
    }
    
   
  }

  // scalled save button event on create modal
  save(){
    this.submitted = true;

    this.newProduct.subCategoryId = this.subCategory?.id;
    this.newProduct.creationTime = new Date();
    this.newProduct.creatorId = this.user.id;
    this.newProduct.lastModificationTime = new Date();
    this.newProduct.lastModifierUserId = this.user.id;
    this.newProduct.quantityOnHand = (this.newProduct.quantityOnHand === undefined) ? 0 : this.newProduct.quantityOnHand
    this.newProduct.quantity = (this.newProduct.quantity === undefined) ? 0 : this.newProduct.quantity
    this.newProduct.quantityOnShelf = (this.newProduct.quantityOnShelf === undefined) ? 0 : this.newProduct.quantityOnShelf
    if(this.newProduct.quantityOnHand < 0){
      this.newProduct.status = Status.OutOfStock
    } else if (this.newProduct.quantityOnHand > this.newProduct?.idealQuantity!){
      this.newProduct.status = Status.InStock
    }else{
      
    }
    if(this.isTaxable === "true"){
      this.newProduct.isTaxable = true;
    }
    else{
      this.newProduct.isTaxable = false;
    }

    console.log(this.newProduct)

    // send
    this.productInventoryService.create(this.newProduct)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.products)){
          this.products = []
        }
        else{
          this.products = [...this.products, res.data];
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
    this.hideDialog();
  }

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.newProduct = {} as ProductInventoryDto;
  }

  confirmDelete(){
    this.productInventoryService.delete(this.newProduct.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.products)){
          this.products = []
        }
        else{
          var index = this.products.findIndex(x => x.id === this.newProduct.id);
        this.products.splice(index, 1)
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

    this.newProduct.subCategoryId = this.subCategory?.id;
    this.newProduct.lastModificationTime = new Date();
    this.newProduct.lastModifierUserId = this.user.id;
    this.newProduct.isTaxable = true

    this.productInventoryService.update(this.newProduct)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.products)){
          this.products = []
        }
        else{
          var index = this.products.findIndex(x => x.id === this.newProduct.id);
          this.products.splice(index, 1);

          this.products = [...this.products, res.data];
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

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }


}
