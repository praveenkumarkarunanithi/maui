using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue36766 : _IssuesUITest
{
	public Issue36766(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "SwipeItem IconImageSource not visible on dark SwipeItem background (dark mode)";

#if TEST_FAILS_ON_WINDOWS // Cannot open a SwipeView programmatically on Windows.
	[Test]
	[Category(UITestCategories.SwipeView)]
	public void SwipeItemIconIsVisibleOnDarkBackground()
	{
		App.WaitForElement("OpenSwipeButton");
		App.Tap("OpenSwipeButton");

		VerifyScreenshot();
	}
#endif
}
