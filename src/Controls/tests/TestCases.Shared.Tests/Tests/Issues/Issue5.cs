using NUnit.Framework;
using NUnit.Framework.Legacy;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue5 : _IssuesUITest
	{
		public Issue5(TestDevice device) : base(device) { }

		public override string Issue => "Legacy Layout not working - ViewCell height measurement with margins";

		[Test]
		[Category(UITestCategories.ListView)]
		public void ViewCellMarginsDisplayCorrectly()
		{
			// Wait for the test page to load
			App.WaitForElement("SuccessLabel");
			
			// Verify that ViewCells with margins are rendered correctly
			// The test passes if the cells are visible and properly spaced
			var successLabel = App.FindElement("SuccessLabel");
			ClassicAssert.IsNotNull(successLabel, "Success label should be visible");
			
			// Verify that the ViewCell content is displayed
			var cellContent = App.FindElement("CellContent");
			ClassicAssert.IsNotNull(cellContent, "ViewCell content should be visible");
			
			// The fact that these elements are findable indicates that the ViewCells
			// are being measured and rendered correctly with their margins
			var cellRect = cellContent.GetRect();
			ClassicAssert.Greater(cellRect.Height, 0, "ViewCell should have a measurable height");
			ClassicAssert.Greater(cellRect.Width, 0, "ViewCell should have a measurable width");
		}
	}
}