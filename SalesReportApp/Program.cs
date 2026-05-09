using System.Text;
using System.Text.Json;

Directory.CreateDirectory("stores");

string sampleJson = """
[
  { "total": 1200.50 },
  { "total": 350.25 }
]
""";

File.WriteAllText("stores/store1.json", sampleJson);

string sampleJson2 = """
[
  { "total": 890.75 },
  { "total": 430.00 }
]
""";

File.WriteAllText("stores/store2.json", sampleJson2);

GenerateSalesSummary("stores", "sales-summary.txt");

Console.WriteLine("Sales summary report created.");

static void GenerateSalesSummary(string salesFilesDirectory, string reportFilePath)
{
    StringBuilder report = new StringBuilder();

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");

    decimal grandTotal = 0;
    Dictionary<string, decimal> fileTotals = new Dictionary<string, decimal>();

    string[] files = Directory.GetFiles(salesFilesDirectory, "*.json");

    foreach (string file in files)
    {
        string json = File.ReadAllText(file);
        JsonDocument document = JsonDocument.Parse(json);

        decimal fileTotal = 0;

        foreach (JsonElement element in document.RootElement.EnumerateArray())
        {
            if (element.TryGetProperty("total", out JsonElement totalElement))
            {
                decimal saleTotal = totalElement.GetDecimal();
                fileTotal += saleTotal;
            }
        }

        fileTotals.Add(Path.GetFileName(file), fileTotal);
        grandTotal += fileTotal;
    }

    report.AppendLine($" Total Sales: {grandTotal:C}");
    report.AppendLine();
    report.AppendLine(" Details:");

    foreach (var item in fileTotals)
    {
        report.AppendLine($"  {item.Key}: {item.Value:C}");
    }

    File.WriteAllText(reportFilePath, report.ToString());
}