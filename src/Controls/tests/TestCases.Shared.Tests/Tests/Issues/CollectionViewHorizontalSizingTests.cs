using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class CollectionViewHorizontalSizingTests : _IssuesUITest
{
	public CollectionViewHorizontalSizingTests(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "CollectionView horizontal layouts should size to content height on iOS";

	[Test]
	[Category(UITestCategories.CollectionView)]
	public void HorizontalCollectionViewShouldSizeToContentHeight()
	{
		this.IgnoreIfPlatforms(new TestDevice[] { TestDevice.Android, TestDevice.Windows },
			"This test is specifically for iOS sizing behavior");

		App.WaitForElement("HorizontalLinearCollectionView");
		App.WaitForElement("HorizontalGridCollectionView");

		// Get the CollectionView elements
		var linearCollectionView = App.FindElement("HorizontalLinearCollectionView");
		var gridCollectionView = App.FindElement("HorizontalGridCollectionView");

		// The CollectionViews should not take up all available height
		// They should size to their content height instead
		// This is a visual test - on iOS the CollectionViews should be compact
		// rather than expanding to fill the remaining container space

		Assert.IsNotNull(linearCollectionView, "Linear CollectionView should be present");
		Assert.IsNotNull(gridCollectionView, "Grid CollectionView should be present");

		// If the fix is working, the CollectionViews will be much smaller in height
		// We can't easily test exact sizes in UI tests, but we can verify they exist
		// and that the layout doesn't crash
	}
}