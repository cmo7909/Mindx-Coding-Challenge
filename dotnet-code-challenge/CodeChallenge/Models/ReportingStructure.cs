using System;
using System.Collections.Generic;
using CodeChallenge.Models;

//New type containing employee and number of reports

public class ReportingStructure
{
    public Employee Employee { get; set; }

    public int NumberOfReports { get; set; }
}