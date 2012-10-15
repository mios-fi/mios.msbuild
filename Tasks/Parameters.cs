using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mios.MSBuild.Tasks {
	public class Parameters : Dictionary<string,string> {
		private readonly Regex pattern;
		public Parameters(Regex pattern, string source) {
			this.pattern = pattern;
			var match = pattern.Match(source);
			if(!match.Success) return;
			for(var i=1; i<match.Groups.Count;i++ ) {
				Add(GroupName(i), match.Groups[i].Value);
			}
		}
		private string GroupName(int i) {
			var name = pattern.GroupNameFromNumber(i);
			if(name==i.ToString()) return (i - 1).ToString();
			return name;
		}
	}
}