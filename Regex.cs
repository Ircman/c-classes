var s = "3+6/87*120-100";
var tokens = Regex.Matches(s, @"[-+/*]|\d+").OfType<Match>().Select(m => m.Value).ToList();
на выходе: ["3", "+", "6", "/", "87", "*", "120","-","100"]
