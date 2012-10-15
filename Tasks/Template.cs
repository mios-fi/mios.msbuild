using System.Collections.Generic;
using System.Linq;

namespace Mios.MSBuild.Tasks {
	public class Template {
		private readonly string source;
		public Template(string source) {
			this.source = source;
		}
		public string Execute(IDictionary<string,string> parameters) {
			return parameters.Aggregate(source, (t,p) => t.Replace("{"+p.Key+"}",p.Value));
		}
	}
}