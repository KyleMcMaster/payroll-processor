namespace Payroll.Processor.Data.Domain.Features.Summary

open System

type Summary = {
    Amount: decimal
    Description: string
    PayrollPeriodStartOn: DateTimeOffset
    PayrollPeriodEndOn: DateTimeOffset
    //Risk: Risk
}
