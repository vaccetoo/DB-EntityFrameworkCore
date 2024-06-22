﻿
using SoftUni.Models;
using System.Diagnostics;
using System.Text;

public class StartUp
{
    public static void Main()
    {
        var context = new SoftUniContext();

        Console.WriteLine(AddNewAddressToEmployee(context));
    }

    // Problem 01.
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToList();

        foreach (var e in employees)
        {
            output.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }

    // Problem 02.
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Salary > 50_000)
            .Select(e => new
            {
                e.FirstName,
                e.Salary
            })
            .OrderBy(e => e.FirstName)
            .ToList();

        foreach (var e in employees)
        {
            output.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return output.ToString().TrimEnd();
    }

    // Problem 03.
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                Departmen = e.Department.Name,
                e.Salary
            })
            .OrderBy (e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToList();

        foreach (var e in employees)
        {
            output
                .AppendLine($"{e.FirstName} {e.LastName} from {e.Departmen} - ${e.Salary:f2}");
        }
        return output.ToString().TrimEnd();
    }

    // Problem 04.
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        StringBuilder output = new StringBuilder();

        Address newAddress = new Address
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };
        context.Addresses.Add(newAddress);

        Employee employee = context.Employees
            .First(e => e.LastName == "Nakov");

        employee.Address = newAddress;
        context.SaveChanges();

        List<string> addressTexts = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address.AddressText)
            .ToList();

        foreach( var addressText in addressTexts)
        {
            output
                .AppendLine(addressText);
        }
        
        return output.ToString().TrimEnd();
    }
}
