namespace DZ3_ASOIU.Models;
public class ReportCityRow
{
    public string Name { get; set; } = "";
    public string CountryName { get; set; } = "";
    public int PopulationK { get; set; }
}
public class ReportCountRow
{
    public string CountryName { get; set; } = "";
    public int CityCount { get; set; }
}
public class ReportAvgRow
{
    public string CountryName { get; set; } = "";
    public double AvgPopulation { get; set; }
}
public class ReportsViewModel
{
    public List<ReportCityRow> Report1 { get; set; } = new();
    public List<ReportCountRow> Report2 { get; set; } = new();
    public List<ReportAvgRow> Report3 { get; set; } = new();
}
