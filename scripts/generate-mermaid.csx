#nullable enable
#r "nuget: System.IO.Abstractions, 19.2.29"

using System.IO;
using System.Text.RegularExpressions;

var root = Directory.GetCurrentDirectory();
var src = Path.Combine(root, "src");
var csFiles = Directory.GetFiles(src, "*.cs", SearchOption.AllDirectories);

var nodes = new Dictionary<string, string>();
var edges = new List<string>();

string? lastNodeId = null;
string? lastDecisionId = null;

int nodeCounter = 0;

string CreateNode(string label, string shape)
{
    var id = $"N{nodeCounter++}";
    string formatted = shape switch
    {
        "decision" => $"{id}{{\"{label}\"}}",
        "start" => $"{id}([\"{label}\"])",
        _ => $"{id}[\"{label}\"]"
    };

    nodes[id] = formatted;
    return id;
}

foreach (var file in csFiles)
{
    var lines = File.ReadAllLines(file);

    foreach (var rawLine in lines)
    {
        var line = rawLine.Trim();

        if (line.StartsWith("// FLOW START:"))
        {
            var label = line.Replace("// FLOW START:", "").Trim();
            var id = CreateNode(label, "start");
            lastNodeId = id;
            lastDecisionId = null;
        }

        else if (line.StartsWith("// FLOW STEP:"))
        {
            var label = line.Replace("// FLOW STEP:", "").Trim();
            var id = CreateNode(label, "step");

            if (lastNodeId != null)
                edges.Add($"{lastNodeId} --> {id}");

            lastNodeId = id;
            lastDecisionId = null;
        }

        else if (line.StartsWith("// FLOW DECISION:"))
        {
            var label = line.Replace("// FLOW DECISION:", "").Trim();
            var id = CreateNode(label, "decision");

            if (lastNodeId != null)
                edges.Add($"{lastNodeId} --> {id}");

            lastNodeId = id;
            lastDecisionId = id;
        }

        else if (line.StartsWith("// FLOW YES:"))
        {
            if (lastDecisionId == null) continue;

            var label = line.Replace("// FLOW YES:", "").Trim();
            var id = CreateNode(label, "step");

            edges.Add($"{lastDecisionId} -->|Yes| {id}");

            lastNodeId = id;
        }

        else if (line.StartsWith("// FLOW NO:"))
        {
            if (lastDecisionId == null) continue;

            var label = line.Replace("// FLOW NO:", "").Trim();
            var id = CreateNode(label, "step");

            edges.Add($"{lastDecisionId} -->|No| {id}");

            lastNodeId = id;
        }

        else if (line.StartsWith("// FLOW LINK:"))
        {
            var link = line.Replace("// FLOW LINK:", "").Trim();
            var parts = link.Split("->");

            if (parts.Length == 2)
            {
                var fromLabel = parts[0].Trim();
                var toLabel = parts[1].Trim();

                var fromNode = nodes.FirstOrDefault(n => n.Value.Contains(fromLabel)).Key;
                var toNode = nodes.FirstOrDefault(n => n.Value.Contains(toLabel)).Key;

                if (fromNode != null && toNode != null)
                    edges.Add($"{fromNode} --> {toNode}");
            }
        }
    }
}

var mermaid = new StringBuilder();
mermaid.AppendLine("flowchart TD");

foreach (var node in nodes.Values)
    mermaid.AppendLine($"    {node}");

foreach (var edge in edges)
    mermaid.AppendLine($"    {edge}");


File.WriteAllText("docs/flowchart.mmd", mermaid.ToString());

Console.WriteLine("Generated docs/flowchart.mmd");
