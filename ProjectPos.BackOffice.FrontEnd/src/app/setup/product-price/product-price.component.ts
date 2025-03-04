import { Component } from "@angular/core";
import { MessageService } from "primeng/api";
import { Table } from "primeng/table";
import { Country } from "src/proxy/enums/country";
import { Unit } from "src/proxy/enums/unit";
import { CompanyDto } from "src/proxy/interfaces/company-dto";
import { ProductInventoryDto } from "src/proxy/interfaces/product-inventory-dto";
import { ProductPriceDto } from "src/proxy/interfaces/product-price-dto";
import { UserDto } from "src/proxy/interfaces/user-dto";
import { ProductInventoryService } from "src/proxy/services/product-inventory.service";
import { ProductPriceService } from "src/proxy/services/product-price.service";
import { ProductService } from "src/proxy/services/product.service";

@Component({
  selector: "app-product-price",
  templateUrl: "./product-price.component.html",
  styleUrls: ["./product-price.component.scss"],
})
export class ProductPriceComponent {
  newProductPrice: ProductPriceDto = {} as ProductPriceDto;

  products: ProductInventoryDto[] = [];

  product: ProductInventoryDto = {} as ProductInventoryDto;

  productPrices: ProductPriceDto[] = [];

  createModal = false;

  editModal = false;

  deleteModal = false;

  submitted = false;

  units: Unit[] = [];

  countries: Country[] = [];

  country!: Country;

  cols: any[] = [];

  user: UserDto = {} as UserDto;

  dialogClass: string = "";

  constructor(
    private productService: ProductInventoryService,
    private priceService: ProductPriceService,
    private messageService: MessageService
  ) {}

  // life cycle hook
  ngOnInit(): void {
    this.user = JSON.parse(sessionStorage.getItem("loggedUser") || "{}");

    // get all products from rhe database
    

    // get all prices from rhe database
    this.priceService.getAllList().subscribe((res) => {
      console.log(res);
      this.productPrices = res.data;
      
    });

    this.productService.getAllList().subscribe((res) => {
      console.log(res);
      this.products = res.data;
      this.updatePriceProducts();
    });

    // serialize enums
    this.countries = Object.values(Country);
    this.units = Object.values(Unit);
  }

  // calculate markup
  markUp() {
    var markup = this.newProductPrice.markUp! * this.newProductPrice.cost!;
    this.newProductPrice.price = markup + this.newProductPrice.cost!;
  }

  onSelectProduct() {
    this.newProductPrice.markUp = this.product.markUp;
  }

  updatePriceProducts(){
    this.products = this.products.filter(p => !this.productPrices.some(pp => pp.productInventoryId === p.id));

  }

  // open create modal
  create() {
    // initialize product
    this.newProductPrice = {} as ProductPriceDto;
    this.createModal = true;
    this.submitted = false;
  }

  // open edit modal
  edit(price: ProductPriceDto) {
    // initialize product to fill the edit form
    this.newProductPrice = {} as ProductPriceDto;
    console.log(price);
    this.newProductPrice = { ...price };
    this.product =
      this.products.find((x) => x.id === this.newProductPrice.product?.id) ||
      ({} as ProductInventoryDto);
    this.editModal = true;
    this.submitted = false;
  }

  // open delete modal
  delete(price: ProductPriceDto) {
    this.newProductPrice = {} as ProductPriceDto;
    // initialize product
    this.newProductPrice = { ...price };
    this.deleteModal = true;
    this.submitted = false;
  }

  // open delete modal to delete multiple selected items
  deleteSelected() {}

  // scalled save button event on create modal
  save() {
    this.submitted = true;

    this.newProductPrice.productInventoryId = this.product?.id;
    this.newProductPrice.creationTime = new Date();
    this.newProductPrice.creatorId = this.user.id;
    this.newProductPrice.lastModificationTime = new Date();
    this.newProductPrice.lastModifierUserId = this.user.id;
    this.newProductPrice.cost = this.newProductPrice.price;

    console.log(this.newProductPrice);

    // send
    this.priceService.create(this.newProductPrice).subscribe(
      (res) => {
        console.log(res);
        if (res.isSuccess) {
          this.products = this.products.filter(p => p.id !== this.newProductPrice.productInventoryId)
          if (!Array.isArray(this.productPrices)) {
            this.productPrices = [];
          } else {
            this.productPrices = [ res.data, ...this.productPrices];
            
          }

          this.messageService.add({
            severity: "success",
            summary: "Success",
            detail: res.message,
            life: 3000,
          });
        } else {
          this.messageService.add({
            severity: "error",
            summary: "Error",
            detail: res.message,
            life: 3000,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: "error",
          summary: "Error",
          detail: error.message,
          life: 3000,
        });
      }
    );

    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.hideDialog();
  }

  hideDialog() {
    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.product = {} as ProductInventoryDto;
    this.newProductPrice = {} as ProductPriceDto;
  }

  confirmDelete() {
    this.priceService.delete(this.newProductPrice.id!).subscribe(
      (res) => {
        console.log(res);
        if (res.isSuccess) {
          if (!Array.isArray(this.productPrices)) {
            this.products = [];
          } else {
            this.productPrices = this.productPrices.filter(pp => pp.id !== this.newProductPrice.id);
            this.updatePriceProducts();
          }

          this.messageService.add({
            severity: "success",
            summary: "Success",
            detail: res.message,
            life: 3000,
          });
        } else {
          this.messageService.add({
            severity: "error",
            summary: "Error",
            detail: res.message,
            life: 3000,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: "error",
          summary: "Error",
          detail: error.message,
          life: 3000,
        });
      }
    );

    this.deleteModal = false;
  }

  update() {
    this.submitted = true;

    this.priceService.update(this.newProductPrice).subscribe(
      (res) => {
        console.log(res);
        if (res.isSuccess) {
          if (!Array.isArray(this.productPrices)) {
            this.productPrices = [];
          } else {
            var index = this.productPrices.findIndex(
              (x) => x.id === this.newProductPrice.id
            );

            this.productPrices = [
              ...this.productPrices.slice(0, index),
              res.data,
              ...this.productPrices.slice(index + 1),
            ]
          }

          this.messageService.add({
            severity: "success",
            summary: "Success",
            detail: res.message,
            life: 3000,
          });
        } else {
          this.messageService.add({
            severity: "error",
            summary: "Error",
            detail: res.message,
            life: 3000,
          });
        }
      },
      (error) => {
        this.messageService.add({
          severity: "error",
          summary: "Error",
          detail: error.message,
          life: 3000,
        });
      }
    );

    this.createModal = false;
    this.editModal = false;
    this.deleteModal = false;
    this.hideDialog();
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, "contains");
  }
}
