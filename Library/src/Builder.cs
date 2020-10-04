//********************************
// (c) 2020 Ada Maynek
// This software is released under the MIT License.
//********************************
using System.IO;
using System.Text.RegularExpressions;

namespace Maynek.Notesvel.Library
{
	public class Builder
	{
		//================================
		// Constants
		//================================
		private const string TagSearchPattern = @"<%(?<SCRIPT>.*?)%>";
		private const RegexOptions TagSearchOption = RegexOptions.Singleline;
		private const string WordPattern = @"(?<WORD>.+?)\((?<RUBY>.*?)\)";
		private const string WordReplace = @"｜${WORD}《${RUBY}》";
		private const RegexOptions WordOption = RegexOptions.Singleline;


		//================================
		// Fields
		//================================
		private readonly Regex TagRegex = new Regex(TagSearchPattern, TagSearchOption);
		private readonly Regex WordRegex = new Regex(WordPattern, WordOption);


		//================================
		// Properties
		//================================
		public Logger Logger { get; set; } = null;
		public Project Setting { get; set; } = null;
		public Catalog Catalog { get; set; } = null;


		//================================
		// Methods
		//================================
		public void Run()
		{
			if (this.Setting == null)
            {
				throw new NotesvelException();
            }

			if (this.Catalog == null)
            {
				throw new NotesvelException();
			}

			this.RunPreprocess();

			this.RunOperation();
		}

		//-------- Preprocess --------
		private void RunPreprocess()
		{
			if (!Directory.Exists(this.Setting.WorkDirectory))
			{
				var info = Directory.CreateDirectory(this.Setting.WorkDirectory);
				info.Attributes |= FileAttributes.Hidden;
			}

			this.RunPreprocessInternal(this.Catalog.Items, "c");
		}

		private void RunPreprocessInternal(CatalogItemList items, string fileBase)
		{
			foreach (var item in items)
			{
				var newFileBase = fileBase + "_" + item.Index.ToString();

				if (item is Contents contents)
				{
					contents.WorkFile = newFileBase + ".nvw";
					this.CreateWorkFile(contents);
				}
				else
				{
					this.RunPreprocessInternal(item.Items, newFileBase);
				}
			}
		}

		private void CreateWorkFile(Contents contents)
		{
			var contentsPath = Path.Combine(this.Setting.SourceDirectory, contents.File);
			var workPath = Path.Combine(this.Setting.WorkDirectory, contents.WorkFile);

			File.Copy(contentsPath, workPath, true);
		}

		//-------- Operation --------
		private void RunOperation()
		{
			foreach (var op in this.Setting.Operations.Values)
			{
				var sd = Path.Combine(this.Setting.RootDirectory, op.DestinationDirectory);
				if (!Directory.Exists(sd))
				{
					Directory.CreateDirectory(sd);
				}

				this.OperateItem(this.Catalog, sd);
			}
		}

		private void OperateItem(CatalogItem item, string dir)
		{
			if (item is Contents contents)
			{
				this.OperateContents(contents, dir);
			}
			else
			{
				foreach (var childItem in item.Items)
				{
					this.OperateItem(childItem, dir);
				}
			}
		}

		private void OperateContents(Contents contents, string dir)
		{		
			var srcText = string.Empty;
			var srcFile = Path.Combine(this.Setting.WorkDirectory, contents.WorkFile);
			using (var reader = new StreamReader(srcFile))
			{
				srcText = reader.ReadToEnd();
			}

			var dstText = this.TagRegex.Replace(srcText, this.TagEvaluator);
			var dstFile = Path.Combine(dir, contents.WorkFile);
			using (var writer = new StreamWriter(dstFile))
			{
				writer.Write(dstText);
			}
		}

		private string TagEvaluator(Match match)
		{
			return this.ResolveScript(match.Groups["SCRIPT"].ToString());
		}

		//-------- Resolve --------
		private string ResolveScript(string script)
		{
			return this.WordRegex.Replace(script, WordReplace);
		}
	}
}

