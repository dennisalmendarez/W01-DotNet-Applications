# W01 Assignment Notes

## Part 1: Web API Evidence

I added an additional pizza record to the Pizza List.

Example Pizza List:

```json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "Veggie",
    "isGlutenFree": true
  },
  {
    "id": 3,
    "name": "Pepperoni",
    "isGlutenFree": false
  }
]
```

## API Test Results

GET /pizza  
Status Code: 200 OK

POST /pizza  
Status Code: 201 Created

PUT /pizza/4  
Status Code: 204 No Content

DELETE /pizza/4  
Status Code: 204 No Content

---

## Part 2: Sales Summary Function

```csharp
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
```