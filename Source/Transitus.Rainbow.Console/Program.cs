using System.Text;

namespace Transitus.Rainbow.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Console.SetWindowSize(150, 40);

			var folderDeserializer = TransitusProvider.FolderDeserializer;
			var templateFactory = TransitusProvider.TemplateFactory;
			var items = folderDeserializer.Deserialize(@"C:\Projects\transitus.rainbow\Files");
			var templates = templateFactory.Create(items);

			foreach (var template in templates)
			{
				var templateOutput = new StringBuilder();

				templateOutput.AppendLine($"{template.Name.PadRight(20, ' ')} | {template.Path}");
				templateOutput.AppendLine("Fields:");

				foreach (var field in template.LocalFields)
				{
					templateOutput.AppendLine($"{string.Empty.PadRight(20, ' ')} | {field.Name.PadRight(30, ' ')} | {field.TypeName}");
				}

				templateOutput.AppendLine(string.Empty.PadRight(140, '-'));

				System.Console.Write(templateOutput);
			}

			System.Console.ReadLine();
		}
	}
}
