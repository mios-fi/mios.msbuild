using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;

namespace Mios.MSBuild.Tasks {
	public class ApplyTemplate : Microsoft.Build.Utilities.Task {
		private Template template;
		private Regex pattern;
		private List<ITaskItem> output;

		[Output]
		public ITaskItem[] Output { get; set; }
		public ITaskItem[] Input { get; set; }
		public string Template { get; set; }
		public string TemplateFile { get; set; }
		public string Pattern { get; set; }

		public override bool Execute() {
			if(Input==null) {
				return true;
			}
			template = LoadTemplate();
			if (template == null) {
				return false;
			}
			output = new List<ITaskItem>();
			var success = Enumerable
				.Range(0, Input.Length)
				.All(ExecuteItem);
			if(success) {
				Output = output.ToArray();
			}
			return success;
		}

		private Template LoadTemplate() {
			if(TemplateFile!=null) {
				if(!File.Exists(TemplateFile)) {
					Log.LogError("Template file '{0}' not found", TemplateFile);
					return null;
				}
				return new Template(File.ReadAllText(TemplateFile));
			} 
			return new Template(Template??String.Empty);
		}

		private bool ExecuteItem(int i) {
			var result = Output == null ? Input[i] : Output[i];
			var resultName = result.ItemSpec;
			var targetName = Input[i].ItemSpec;
			if(!File.Exists(targetName)) {
				Log.LogError("Missing target file '{0}'", Input);
				return false;
			}
			pattern = new Regex(Pattern??String.Empty);
			var parameters = new Parameters(pattern, targetName);
			if(parameters.Count==0) {
				Log.LogMessage("Skipping {0} since it does not match pattern", targetName);
				return true;
			}
			parameters.Add("body", File.ReadAllText(targetName));
			File.WriteAllText(resultName??targetName,template.Execute(parameters));
			Log.LogMessage("Applied template {0} -> {1}", targetName, resultName);

			output.Add(result);
			return true;
		}
	}
}
