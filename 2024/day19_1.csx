using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;

AppContext.SetData("REGEX_NONBACKTRACKING_MAX_AUTOMATA_SIZE", 20_000);
var raw = File.ReadAllLines("day19.txt");
var sample = raw[0].Split(", ");
var maxLength = sample.Max(s => s.Length);
var sampleHash = new HashSet<string>(sample);
var pattern = $"^({raw[0].Replace(", ", "|")})+$";
var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.NonBacktracking);
var data = raw.Skip(2).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();

var counter = 0;
regex.Matches(data[1]);