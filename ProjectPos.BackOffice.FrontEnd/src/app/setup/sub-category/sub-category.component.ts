import { SubCategoryDto } from './../../../proxy/interfaces/sub-category-dto';
import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Category } from 'src/proxy/enums/category';
import { Country } from 'src/proxy/enums/country';
import { Unit } from 'src/proxy/enums/unit';
import { SubCategoryService } from 'src/proxy/services/sub-category.service';

@Component({
  selector: 'app-sub-category',
  templateUrl: './sub-category.component.html',
  styleUrls: ['./sub-category.component.scss']
})
export class SubCategoryComponent implements OnInit {

  newSubCategory: SubCategoryDto = {} as SubCategoryDto

  subCategories: SubCategoryDto[] = [];

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  units: Unit[] = [];

  catedories: Category[] = [];

  country!: Country;

  cols: any[] = [];

  dialogClass: string = '';

  constructor(
    private subCategoryService: SubCategoryService,
    private messageService: MessageService
  ) { }

  // life cycle hook
  ngOnInit(): void {

    // get all subCategories from rhe database
    this.subCategoryService.getAllList()
    .subscribe((res) => {
      console.log(res);
      this.subCategories = res.data;
    });

    // serialize enums
    this.catedories = Object.values(Category);
    this.units = Object.values(Unit);
  }

  // open create modal
  create(){
    this.createModal = true;
    this.submitted = false;
  }

  // open edit modal
  edit(subCategory: SubCategoryDto){
    // initialize subCategory to fill the edit form
    this.newSubCategory = { ...subCategory};
    this.editModal = true;
    this.submitted = false;
  }

  // open delete modal
  delete(subCategory: SubCategoryDto){
    // initialize subCategory
    this.newSubCategory = { ...subCategory};
    this.deleteModal = true;
    this.submitted = false;
  }

  // open delete modal to delete multiple selected items
  deleteSelected(){
  }

  // scalled save button event on create modal
  save(){
    this.submitted = true;

    // send
    this.subCategoryService.create(this.newSubCategory)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.subCategories)){
          this.subCategories = []
        }
        else{
          this.subCategories = [...this.subCategories, res.data];
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

  hideDialog(){
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.newSubCategory = {} as SubCategoryDto;
  }

  confirmDelete(){
    this.subCategoryService.delete(this.newSubCategory.id!)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.subCategories)){
          this.subCategories = []
        }
        else{
          var index = this.subCategories.findIndex(x => x.id === this.newSubCategory.id);
        this.subCategories.splice(index, 1)
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

    this.subCategoryService.update(this.newSubCategory)
    .subscribe((res) => {
      console.log(res);
      if(res.isSuccess){

        if(!Array.isArray(this.subCategories)){
          this.subCategories = []
        }
        else{
          var index = this.subCategories.findIndex(x => x.id === this.newSubCategory.id);
          this.subCategories.splice(index, 1);

          this.subCategories = [...this.subCategories, res.data];
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
