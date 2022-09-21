string arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString().ToLower();
string OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
string RuntimeIdentifier = System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;
string csharpVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
Console.Title = "Installer - Wizzy#9181";
Console.WriteLine("OS: " + OSDescription);
Console.WriteLine("Runtime: " + RuntimeIdentifier);
Console.WriteLine("Architecture: " + arch);
Console.WriteLine("C# Version: " + csharpVersion);
Console.WriteLine("Use this tool to install any program that you need. \nUse help command to check what you can do");
while (true)
{
    string input = Console.ReadLine();
    string[] iargs = input.Split(' ');
    switch (iargs[0])
    {
        case "exit":
            return;
        case "help":
            Console.WriteLine("help - show this message");
            Console.WriteLine("exit - exit the program");
            Console.WriteLine("install [program name] - install a program");
            Console.WriteLine("list <type> - list all programs <of specified type>");
            break;
        case "list":
            try
            {
                if (iargs.Length > 1)
                {
                    string text = string.Join(' ', iargs, 1, iargs.Length - 1);
                    await ListCommand.ListAllModules(text);
                    break;
                }

                await ListCommand.ListAllModules();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "install":
            try
            {
                var module = await Module.GetModule(string.Join(' ', iargs, 1, iargs.Length - 1));
                await module.InstallModule();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        default:
            Console.WriteLine("Unknown command");
            goto case "help";
    }
}