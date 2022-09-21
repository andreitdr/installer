public static class TableFunctions
{
    private static int SpaceBetweenColumns = 3;
    internal enum TableFormat { CENTER_EACH_COLUMN_BASED, CENTER_OVERALL_LENGTH, DEFAULT }
    /// <summary>
    /// A way to create a table based on input data
    /// </summary>
    /// <param name="data">The List of arrays of strings that represent the rows.</param>
    internal static void FormatAndAlignTable(List<string[]> data, TableFormat format)
    {
        if (format == TableFormat.CENTER_EACH_COLUMN_BASED)
        {
            char tableLine = '-';
            char tableCross = '+';
            char tableWall = '|';

            int[] len = new int[data[0].Length];
            foreach (var line in data)
                for (int i = 0; i < line.Length; i++)
                    if (line[i].Length > len[i])
                        len[i] = line[i].Length;


            foreach (string[] row in data)
            {
                if (row[0][0] == tableLine)
                    Console.Write(tableCross);
                else
                    Console.Write(tableWall);
                for (int l = 0; l < row.Length; l++)
                {
                    if (row[l][0] == tableLine)
                    {
                        for (int i = 0; i < len[l] + 4; ++i)
                            Console.Write(tableLine);
                    }
                    else if (row[l].Length == len[l])
                    {
                        Console.Write("  ");
                        Console.Write(row[l]);
                        Console.Write("  ");
                    }
                    else
                    {
                        int lenHalf = row[l].Length / 2;
                        for (int i = 0; i < ((len[l] + 4) / 2 - lenHalf); ++i)
                            Console.Write(" ");
                        Console.Write(row[l]);
                        for (int i = (len[l] + 4) / 2 + lenHalf + 1; i < len[l] + 4; ++i)
                            Console.Write(" ");
                        if (row[l].Length % 2 == 0)
                            Console.Write(" ");
                    }

                    Console.Write(row[l][0] == tableLine ? tableCross : tableWall);
                }

                Console.WriteLine(); //end line
            }

            return;
        }

        if (format == TableFormat.CENTER_OVERALL_LENGTH)
        {
            int maxLen = 0;
            foreach (string[] row in data)
                foreach (string s in row)
                    if (s.Length > maxLen)
                        maxLen = s.Length;

            int div = (maxLen + 4) / 2;

            foreach (string[] row in data)
            {
                Console.Write("\t");
                if (row[0] == "-")
                    Console.Write("+");
                else
                    Console.Write("|");

                foreach (string s in row)
                {
                    if (s == "-")
                    {
                        for (int i = 0; i < maxLen + 4; ++i)
                            Console.Write("-");
                    }
                    else if (s.Length == maxLen)
                    {
                        Console.Write("  ");
                        Console.Write(s);
                        Console.Write("  ");
                    }
                    else
                    {
                        int lenHalf = s.Length / 2;
                        for (int i = 0; i < div - lenHalf; ++i)
                            Console.Write(" ");
                        Console.Write(s);
                        for (int i = div + lenHalf + 1; i < maxLen + 4; ++i)
                            Console.Write(" ");
                        if (s.Length % 2 == 0)
                            Console.Write(" ");
                    }

                    if (s == "-")
                        Console.Write("+");
                    else
                        Console.Write("|");
                }

                Console.WriteLine(); //end line
            }

            return;
        }

        if (format == TableFormat.DEFAULT)
        {
            int[] widths = new int[data[0].Length];
            int space_between_columns = SpaceBetweenColumns;
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j].Length > widths[j])
                        widths[j] = data[i][j].Length;
                }
            }

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == "-")
                        data[i][j] = " ";
                    Console.Write(data[i][j]);
                    for (int k = 0; k < widths[j] - data[i][j].Length + 1 + space_between_columns; k++)
                        Console.Write(" ");
                }

                Console.WriteLine();
            }

            return;
        }

        throw new Exception("Unknown type of table");
    }
}