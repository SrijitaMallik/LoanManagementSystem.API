using System;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.Models;

public class EMI
{
    public int EMIId { get; set; }
    public int LoanId { get; set; }
    public Loan? Loan { get; set; }
    public int InstallmentNumber { get; set; }
    public DateTime DueDate { get; set; }
    public decimal EMIAmount { get; set; }

    public bool IsPaid { get; set; } = false;
    public DateTime? PaidDate { get; set; }
}