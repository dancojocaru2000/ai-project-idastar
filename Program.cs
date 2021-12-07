using IdaStar;

string[] labyrinthIN = File.ReadAllLines(@"./labyrinth.txt");
string[] labyrinth = FormatLabyrinth(labyrinthIN);

Console.OutputEncoding = System.Text.Encoding.UTF8;

var algoBoard = new IdaStar.WorkingBoard(labyrinth.Select((row) => row.ToList()).ToList());
int step = 0;
ConsoleColor border = ConsoleColor.Magenta;

Console.WriteLine("The input labyrinth: ");
PrintBoard(0, true, false);

bool printSteps = false;

Console.WriteLine();
Console.WriteLine("Show each step? (Y/N) ");
if(Console.ReadLine()?.Trim().ToUpperInvariant() == "Y") {
    printSteps = true;
}

if(printSteps) {
    algoBoard.AlgorithmStep += (_, threshold) => {
        PrintBoard(threshold, false);
    };
}

algoBoard.RunIdaStar();
if(printSteps) Console.Clear();
Console.WriteLine("The solved labyrinth is:");
PrintBoard(0, true, clearScreen: false);


if (args.Contains("-w") || args.Contains("--wait")) {
	Console.ReadKey();
}

static void FormattedLabRow(string line) {
    char[] characters = line.ToCharArray();
    foreach (char c in characters) {
        switch ( c ) {
            case '#': {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("   "); 
                break;
            }

            case '.': {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("   "); 
                break;
            }

            case 'S': {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("<•>"); 
                break;
            }

            case 'F': {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("[ ]"); 
                break;
            }

            case 'P': {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" • "); 
                break;
            }

            case 'G': {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(" • "); 
                break;
            }

            default: break;
        }
    }
    Console.ResetColor();
}

string[] FormatLabyrinth(string[] labIN) {
    var maxW = 0;
    List<string> lab = new List<string>();
    foreach (string line in labIN)
    {
        if(maxW < line.Length) {
            maxW = line.Length;
        }
    }

    foreach (string line in labIN)
    {
        if(maxW > line.Length) {
            var dif = maxW - line.Length;
            string fLine = line;
            while (dif > 0) {
                fLine = fLine+ "#";
                dif--;
            }
            lab.Add(fLine);
        }else {
            lab.Add(line);
        }
    }

    string[] FormattedLabyrinth = lab.ToArray();
    return FormattedLabyrinth;
}

void PrintBoard(int threshold, bool done, bool clearScreen = true){
    if (clearScreen) Console.Clear();
    step++;
    if(done){
        
    }else if(step%2 == 0) {
        Console.WriteLine($"Computing (threshold: {threshold}) [• ]");
    }else {
        Console.WriteLine($"Computing (threshold: {threshold}) [ •]");
    }

    //top border
    Console.BackgroundColor = border;
    for (var i=0; i< algoBoard.Board[0].Count + 2; i++){
        Console.Write("   "); 
    }
    Console.ResetColor();
    Console.WriteLine();

    foreach (var line in algoBoard.Board)
    {
        var charlist = line.Select((state) => CellStateUtil.ToInput(state));
        var str = string.Join("", charlist);

        //left border
        Console.BackgroundColor = border;
        Console.Write("   "); 
        Console.ResetColor();

        //labyrinth line
        FormattedLabRow(str);

        //right border
        Console.BackgroundColor = border;
        Console.Write("   "); 
        Console.ResetColor();

        Console.WriteLine();
    }

    //bottom border
    Console.BackgroundColor = border;
    for (var i=0; i< algoBoard.Board[0].Count + 2; i++){
        Console.Write("   "); 
    }
    Console.ResetColor();
    Console.WriteLine();
	Thread.Sleep(200);
}
