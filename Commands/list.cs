public static class ListCommand
{
    public static async Task<List<Module>> GetAllModules()
    {
        List<Module> modules = new();
        string arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString().ToLower();
        var items = await ServerCom.ReadTextFromURL("https://raw.githubusercontent.com/Wizzy69/installer/csharp-installer-console/Items");
        foreach (var item in items)
        {
            if (item.Length < 2) continue;
            string[] data = item.Split(',');
            if (data[1] == arch)
                modules.Add(new Module(data[0], data[1], data[2], Module.GetModuleTypeFromString(data[3])));
        }

        if (modules.Count == 0)
            throw new Exception("Found no modules for your architecture " + arch);
        return modules;


    }



    public static async Task ListAllModules(string type)
    {
        List<string[]> data = new();
        var modules = await GetAllModules();
        data.Add(new string[] { "-", "-", "-" });
        data.Add(new string[] { "Name", "Module Type", "Architecture" });
        data.Add(new string[] { "-", "-", "-" });
        foreach (var module in modules)
            if (Module.GetModuleTypeFromString(type) == module.Type)
                data.Add(new string[] { module.Name, module.Type.ToString().Replace('_', ' '), module.Architecture });
        data.Add(new string[] { "-", "-", "-" });
        TableFunctions.FormatAndAlignTable(data, TableFunctions.TableFormat.DEFAULT);

    }
    public static async Task ListAllModules()
    {
        List<string[]> data = new();
        var modules = await GetAllModules();
        data.Add(new string[] { "-", "-", "-" });
        data.Add(new string[] { "Name", "Module Type", "Architecture" });
        data.Add(new string[] { "-", "-", "-" });
        foreach (var module in modules)
            data.Add(new string[] { module.Name, module.Type.ToString().Replace('_', ' '), module.Architecture });
        data.Add(new string[] { "-", "-", "-" });
        TableFunctions.FormatAndAlignTable(data, TableFunctions.TableFormat.DEFAULT);

    }
}