#r "nuget: System.IO.Abstractions, 19.2.29"

using System.IO;
using System.Text.RegularExpressions;

var root = Directory.GetCurrentDirectory();
var src = Path.Combine(root, "src");

var flowSteps = new List<string>();

foreach (var file in Directory.GetFiles(src, "*.cs", SearchOption.AllDirectories))
{
    var lines = File.ReadAllLines(file);
    foreach (var line in lines)
    {
        if (line.Contains("FLOWSTEP:"))
        {
            var step = line.Split("FLOWSTEP:")[1].Trim();
            flowSteps.Add(step);
        }
    }
}

var mermaid = "flowchart TD\n";

for (int i = 0; i < flowSteps.Count - 1; i++)
{
    mermaid += $"    A{i}[{flowSteps[i]}] --> A{i + 1}[{flowSteps[i + 1]}]\n";
}

File.WriteAllText("docs/flowchart.mmd", mermaid);

Console.WriteLine("Generated docs/flowchart.mmd");
