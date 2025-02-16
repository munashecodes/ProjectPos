using ProjectPos.Data.AggregateRoots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class Employee : FullAuditedAggregateRoot<int>
    {
        public string? Name { get; set; } = null!;
        public string? Surname { get; set; } = null!;
        public string? NatId { get; set; }
        public DateTime? DOB { get; set; }
        public int? AddressId { get; set; }
        public string? Cell { get; set; }
        public string? Email { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; }

//dependant entities
        public User? User { get; set; }
        public EmployeeDetails? EmployeeDetails { get; set; }
        public SalaryStructure? SalaryStructure { get; set; }

        public List<PaySlip>? PaySlips { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<OvertimeRecord> OvertimeRecords { get; set; }
        public virtual ICollection<EmployeeDeduction> Deductions { get; set; }

        public Employee()
        {
            Attendances = new HashSet<Attendance>();
            OvertimeRecords = new HashSet<OvertimeRecord>();
            Deductions = new HashSet<EmployeeDeduction>();
        }

    }
}
