namespace LoanManagementSystem.API.Services;

public class EmiCalculatorService
{
    public decimal CalculateEMI(decimal principal, decimal annualRate, int months)
    {
        var r = annualRate / 12 / 100;
        var emi = principal * r * (decimal)Math.Pow((double)(1 + r), months)
                / ((decimal)Math.Pow((double)(1 + r), months) - 1);

        return Math.Round(emi, 2);
    }
}
