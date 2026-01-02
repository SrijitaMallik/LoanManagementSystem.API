using System;

namespace LoanManagementSystem.API.Services
{
    public class EmiCalculatorService
    {
        public decimal CalculateEmi(decimal amount, int months, decimal rate)
        {
            decimal monthlyRate = rate / 12 / 100;
            decimal pow = (decimal)Math.Pow((double)(1 + monthlyRate), months);

            decimal emi = (amount * monthlyRate * pow) / (pow - 1);
            return Math.Round(emi, 2);
        }
    }
}
