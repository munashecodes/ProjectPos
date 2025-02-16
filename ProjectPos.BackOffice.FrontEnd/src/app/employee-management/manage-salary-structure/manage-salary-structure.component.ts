import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { Currency } from 'src/proxy/enums/currency';
import { EmployeeDto } from 'src/proxy/interfaces/employee-dto';
import { SalaryStructureDto } from 'src/proxy/interfaces/salary-structure-dto';
import { EmployeeService } from 'src/proxy/services/employee.service';
import { SalaryStructureService } from 'src/proxy/services/salary-structure.service';

@Component({
  selector: 'app-manage-salary-structure',
  templateUrl: './manage-salary-structure.component.html',
  styleUrls: ['./manage-salary-structure.component.scss']
})
export class ManageSalaryStructureComponent implements OnInit {
  
  newSalaryStructure: SalaryStructureDto = {} as SalaryStructureDto;
  salaryStructures: SalaryStructureDto[] = [];
  employees: EmployeeDto[] = [];
  currencies = Object.values(Currency);

  createModal = false;
  editModal = false;
  deleteModal = false;
  submitted = false;
  dialogVisible = false;

  cols: any[] = [];

  selectedEmployee: EmployeeDto | null = null;

  constructor(
    private salaryStructureService: SalaryStructureService,
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadSalaryStructures();
  }

  loadEmployees() {
    this.employeeService.getAllList()
      .subscribe(res => {
        if (res.isSuccess) {
          this.employees = res.data;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  loadSalaryStructureByEmployee() {
    if(this.selectedEmployee?.id == null) {
      this.loadSalaryStructures()
    } else {
    this.salaryStructureService.getByEmployeeIdAsync(this.selectedEmployee.id)
      .subscribe(res => {
        if (res.isSuccess) {
          this.salaryStructures = res.data
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: res.message,
            life: 3000
          });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      }, error => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error.message,
          life: 3000
        });
      });
    }
  }

  loadSalaryStructures() {
    this.salaryStructureService.getAllAsync()
      .subscribe(res => {
        if (res.isSuccess) {
          this.salaryStructures = res.data;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  create() {
    this.newSalaryStructure = {} as SalaryStructureDto;
    this.submitted = false;
    this.createModal = true;
    this.dialogVisible = true;
  }

  serializeAmounts() {
    this.newSalaryStructure.aidsLevyDeduction = this.newSalaryStructure.aidsLevyDeduction || 0;
    this.newSalaryStructure.taxDeduction = this.newSalaryStructure.taxDeduction || 0;
    this.newSalaryStructure.pensionDeduction = this.newSalaryStructure.pensionDeduction || 0;
    this.newSalaryStructure.otherDeduction = this.newSalaryStructure.otherDeduction || 0;
    this.newSalaryStructure.housingAllowance = this.newSalaryStructure.housingAllowance || 0;
    this.newSalaryStructure.transportAllowance = this.newSalaryStructure.transportAllowance || 0;
    this.newSalaryStructure.otherAllowance = this.newSalaryStructure.otherAllowance || 0;
    this.newSalaryStructure.medicalBenefit = this.newSalaryStructure.medicalBenefit || 0;
    this.newSalaryStructure.pensionBenefit = this.newSalaryStructure.pensionBenefit || 0;
    this.newSalaryStructure.otherBenefit = this.newSalaryStructure.otherBenefit || 0;
    this.newSalaryStructure.overtimeHours = this.newSalaryStructure.overtimeHours || 0;
    this.newSalaryStructure.overtimeRate = this.newSalaryStructure.overtimeRate || 0;
    this.newSalaryStructure.overtimeTotal = this.newSalaryStructure.overtimeTotal || 0;
    this.newSalaryStructure.hourlyRate = this.newSalaryStructure.hourlyRate || 0;
    this.newSalaryStructure.hoursWorked = this.newSalaryStructure.hoursWorked || 0;
    this.newSalaryStructure.taxableIncome = this.newSalaryStructure.taxableIncome || 0;
  }

  edit(salaryStructure: SalaryStructureDto) {
    this.newSalaryStructure = { ...salaryStructure };
    this.editModal = true;
    this.dialogVisible = true;
  }

  delete(salaryStructure: SalaryStructureDto) {
    this.newSalaryStructure = { ...salaryStructure };
    this.deleteModal = true;
  }

  save() {
    this.submitted = true;

    if (!this.newSalaryStructure.employeeId || !this.newSalaryStructure.basicSalary) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.serializeAmounts();

    this.salaryStructureService.createAsync(this.newSalaryStructure)
      .subscribe(res => {
        if (res.isSuccess) {
          this.salaryStructures.push(res.data);
          this.salaryStructures = [...this.salaryStructures];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: res.message,
            life: 3000
          });
          this.hideDialog();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  update() {
    this.submitted = true;

    if (!this.newSalaryStructure.employeeId || !this.newSalaryStructure.basicSalary) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Please fill in all required fields',
        life: 3000
      });
      return;
    }

    this.serializeAmounts();

    this.salaryStructureService.updateAsync(this.newSalaryStructure)
      .subscribe(res => {
        if (res.isSuccess) {
          const index = this.salaryStructures.findIndex(x => x.id === this.newSalaryStructure.id);
          this.salaryStructures[index] = res.data;
          this.salaryStructures = [...this.salaryStructures];
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: res.message,
            life: 3000
          });
          this.hideDialog();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: res.message,
            life: 3000
          });
        }
      });
  }

  confirmDelete() {
    if (this.newSalaryStructure.id) {
      // Using 1 as userId for now - should come from auth service
      this.salaryStructureService.deleteAsync(1, this.newSalaryStructure.id)
        .subscribe(success => {
          if (success) {
            this.salaryStructures = this.salaryStructures.filter(x => x.id !== this.newSalaryStructure.id);
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Salary structure deleted successfully',
              life: 3000
            });
          }
          this.hideDialog();
        });
    }
  }

  hideDialog() {
    this.createModal = false;
    this.editModal = false;
    this.dialogVisible = false;
    this.deleteModal = false;
    this.submitted = false;
    this.newSalaryStructure = {} as SalaryStructureDto;
  }

  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  calculateTotalAllowances(salaryStructure: SalaryStructureDto): number {
    return (salaryStructure.housingAllowance || 0) + 
           (salaryStructure.transportAllowance || 0) + 
           (salaryStructure.otherAllowance || 0);
  }

  calculateTotalDeductions(salaryStructure: SalaryStructureDto): number {
    return (salaryStructure.taxDeduction || 0) + 
           (salaryStructure.pensionDeduction || 0) + 
           (salaryStructure.aidsLevyDeduction || 0) + 
           (salaryStructure.otherDeduction || 0);
  }

  calculateTotalBenefits(salaryStructure: SalaryStructureDto): number {
    return (salaryStructure.medicalBenefit || 0) + 
           (salaryStructure.pensionBenefit || 0) + 
           (salaryStructure.otherBenefit || 0);
  }
} 
