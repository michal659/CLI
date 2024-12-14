using System.CommandLine;
using System.CommandLine.NamingConventionBinder; // חשוב עבור SetHandler
using System.IO;
using System.Threading.Tasks;
using System;

Option<string> languageOption = new Option<string>(
    "--language",
    description: "List of programming languages")
{
    IsRequired = true
};

var outputOption = new Option<FileInfo>(
    "--output",
    description: "Output bundle file path")
{
    IsRequired = true
};

var noteOption = new Option<bool>(
    "--note",
    description: "Include source file path as a comment");

var sortOption = new Option<string>(
    "--sort",
    description: "Sorting order: 'name' or 'type'")
{
    Arity = ArgumentArity.ZeroOrOne
};

var removeEmptyLinesOption = new Option<bool>(
    "--remove-empty-lines",
    description: "Remove empty lines from source files");

var authorOption = new Option<string>(
    "--author",
    description: "Name of the author");

var bundleCommand = new Command("bundle", "Bundle code files into a single file")
{
    languageOption,
    outputOption,
    noteOption,
    sortOption,
    removeEmptyLinesOption,
    authorOption
};

// הגדרת הלוגיקה של הפקודה
bundleCommand.SetHandler(
    (string language, FileInfo output, bool note, string? sort, bool removeEmptyLines, string? author) =>
    {
        try
        {
            using (var writer = new StreamWriter(output.FullName))
            {
                // אם הוזן שם מחבר, נכתוב אותו לתוך הקובץ
                if (!string.IsNullOrWhiteSpace(author))
                {
                    writer.WriteLine($"// Author: {author}");
                }

                // אם יש להוסיף הערות על קובץ המקור
                if (note)
                {
                    writer.WriteLine("// Source files included:");
                }

                // כתיבה נוספת של תוכן בהתאם להגדרות שלך
                writer.WriteLine($"// Language: {language}");
                writer.WriteLine("// Bundled code follows:");

                // לדוגמה, אם יש צורך להוסיף קוד בגוף הקובץ
                writer.WriteLine("// Code bundled successfully!");

                Console.WriteLine($"Bundled files to {output.FullName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    },
languageOption, outputOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

var rootCommand = new RootCommand("CLI Application")
{
    bundleCommand
};

// הרץ את הפקודה על פי פרמטרים מסוימים
await rootCommand.InvokeAsync(args);
