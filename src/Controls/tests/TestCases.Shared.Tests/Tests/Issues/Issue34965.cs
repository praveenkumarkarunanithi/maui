using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue34965 : _IssuesUITest
{
	public override string Issue =>
		"[iOS 26] CarouselView PreviousPosition/PreviousItem incorrect during animated ScrollTo in CarouselViewHandler2";

	public Issue34965(TestDevice device) : base(device) { }

	[TestCase(false, false)]
	[TestCase(false, true)]
	[TestCase(true, false)]
	[TestCase(true, true)]
	[Category(UITestCategories.CarouselView)]
	public void ProgrammaticNavigationPreservesPreviousPositionAndItem(bool loop, bool animate)
	{
		ConfigureCarousel(loop, animate);

		AssertNavigationPath("ScrollToLastButton", "ScrollTo1Button", 9, "Item 10");
		ResetToStart();

		AssertNavigationPath("ScrollToElementLastButton", "ScrollToElement1Button", 9, "Item 10");
		ResetToStart();

		AssertNavigationPath("SetPosition3Button", "SetPosition1Button", 3, "Item 4");
		ResetToStart();

		AssertNavigationPath("SetCurrentItemLastButton", "SetCurrentItem1Button", 9, "Item 10");
	}

	void ConfigureCarousel(bool loop, bool animate)
	{
		// if (App is not AppiumIOSApp iosApp || !HelperExtensions.IsIOS26OrHigher(iosApp))
		// {
		// 	Assert.Ignore("Issue 34965 reproduces only on iOS 26 and later.");
		// }

		App.WaitForElement("LoopOffButton");
		App.Tap(loop ? "LoopOnButton" : "LoopOffButton");
		App.Tap(animate ? "AnimateOnButton" : "AnimateOffButton");
		ResetToStart();
	}

	void ResetToStart()
	{
		App.Tap("SetPosition0Button");
		AssertCurrentState(0, "Item 1");
	}

	void AssertNavigationPath(string firstAction, string secondAction, int firstPosition, string firstItem)
	{
		App.Tap(firstAction);
		AssertCarouselState(firstPosition, firstItem, 0, "Item 1");

		App.Tap(secondAction);
		AssertCarouselState(1, "Item 2", firstPosition, firstItem);
	}

	void AssertCurrentState(int position, string item)
	{
		App.RetryAssert(() =>
		{
			Assert.That(App.FindElement("CurrentPositionLabel").GetText(), Is.EqualTo($"Current Position: {position}"));
			Assert.That(App.FindElement("CurrentItemLabel").GetText(), Is.EqualTo($"Current Item: {item}"));
		});
	}

	void AssertCarouselState(int position, string item, int previousPosition, string previousItem)
	{
		App.RetryAssert(() =>
		{
			Assert.That(App.FindElement("CurrentPositionLabel").GetText(), Is.EqualTo($"Current Position: {position}"));
			Assert.That(App.FindElement("CurrentItemLabel").GetText(), Is.EqualTo($"Current Item: {item}"));
			Assert.That(App.FindElement("PreviousPositionLabel").GetText(), Is.EqualTo($"Previous Position: {previousPosition}"));
			Assert.That(App.FindElement("PreviousItemLabel").GetText(), Is.EqualTo($"Previous Item: {previousItem}"));
		});
	}
}
