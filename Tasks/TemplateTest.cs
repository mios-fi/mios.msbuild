using System.Collections.Generic;
using Xunit;

namespace Mios.MSBuild.Tasks {
	public class TemplateTest {
		[Fact]
		public void ShouldReplaceMarkerInSourceWithParameter() {
			var templ = new Template("This is {name} a template");
			Assert.Equal("This is really a template", templ.Execute(new Dictionary<string, string> { { "name", "really" } }));
		}
		[Fact]
		public void ShouldReplaceIndexedMarkerInSourceWithMatchingIndexedParameter() {
			var templ = new Template("This is {left} a {right}");
			Assert.Equal("This is really a template", templ.Execute(new Dictionary<string,string> {{"left","really"},{"right","template"}}));
		}
	}
}