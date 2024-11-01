using System;
using System.Collections.Generic;

interface IReport
{
    string Generate();
}

class SalesReport : IReport
{
    public string Generate()
    {
        return "Отчет по продажам: Продукт А - 100 шт., Продукт Б - 200 шт.";
    }
}

class UserReport : IReport
{
    public string Generate()
    {
        return "Отчет по пользователям: Пользователь Х, Пользователь Y";
    }
}

abstract class ReportDecorator : IReport
{
    protected IReport report;

    public ReportDecorator(IReport report)
    {
        this.report = report;
    }

    public virtual string Generate()
    {
        return report.Generate();
    }
}

class DateFilterDecorator : ReportDecorator
{
    private DateTime startDate;
    private DateTime endDate;

    public DateFilterDecorator(IReport report, DateTime startDate, DateTime endDate) : base(report)
    {
        this.startDate = startDate;
        this.endDate = endDate;
    }

    public override string Generate()
    {
        return $"{report.Generate()}\nФильтрация по датам: от {startDate.ToShortDateString()} до {endDate.ToShortDateString()}";
    }
}

class SortingDecorator : ReportDecorator
{
    private string sortBy;

    public SortingDecorator(IReport report, string sortBy) : base(report)
    {
        this.sortBy = sortBy;
    }

    public override string Generate()
    {
        return $"{report.Generate()}\nСортировка по: {sortBy}";
    }
}

class CsvExportDecorator : ReportDecorator
{
    public CsvExportDecorator(IReport report) : base(report) { }

    public override string Generate()
    {
        return $"{report.Generate()}\nЭкспорт в формат CSV: Отчет сохранен в CSV.";
    }
}

class PdfExportDecorator : ReportDecorator
{
    public PdfExportDecorator(IReport report) : base(report) { }

    public override string Generate()
    {
        return $"{report.Generate()}\nЭкспорт в формат PDF: Отчет сохранен в PDF.";
    }
}

class AmountFilterDecorator : ReportDecorator
{
    private double minAmount;

    public AmountFilterDecorator(IReport report, double minAmount) : base(report)
    {
        this.minAmount = minAmount;
    }

    public override string Generate()
    {
        return $"{report.Generate()}\nФильтрация по сумме: минимум {minAmount}";
    }
}

class Program
{
    static void Main()
    {
        IReport report = new SalesReport();
        report = new DateFilterDecorator(report, new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));
        report = new SortingDecorator(report, "дата");
        report = new CsvExportDecorator(report);

        Console.WriteLine(report.Generate());

        IReport userReport = new UserReport();
        userReport = new AmountFilterDecorator(userReport, 100);
        userReport = new PdfExportDecorator(userReport);

        Console.WriteLine("\n" + userReport.Generate());
    }
}
