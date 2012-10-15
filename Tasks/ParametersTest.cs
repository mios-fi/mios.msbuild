using System.Text.RegularExpressions;
using Xunit;

namespace Mios.MSBuild.Tasks {
	public class ParametersTest {
		[Fact]
		public void EmptyRegexpPatternMeansNoParameters() {
			var param = new Parameters(new Regex(""), "xyz");
			Assert.Equal(0, param.Count);
		}
		[Fact]
		public void ParsesSimpleGroupToParameterUsingIndexAsName() {
			var param = new Parameters(new Regex("x(.)z"), "xyz");
			Assert.Equal("y", param["0"]);
		}
		[Fact]
		public void ParsesMultipleGroupsToParametersUsingIndexAsName() {
			var param = new Parameters(new Regex("x(.)(.)"), "xyz");
			Assert.Equal("y", param["0"]);
			Assert.Equal("z", param["1"]);
		}
		[Fact]
		public void ParsesNamedGroupToParameterUsingGroupNameAsName() {
			var param = new Parameters(new Regex("x(?<name>.)z"), "xyz");
			Assert.Equal("y", param["name"]);
		}
	}
}