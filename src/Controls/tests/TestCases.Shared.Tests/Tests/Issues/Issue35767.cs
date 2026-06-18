using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue35767 : _IssuesUITest
{
	public override string Issue => "SearchHandler.ShowsResults does not work dynamically";

	public Issue35767(TestDevice device) : base(device) { }

	// Searches for "B" — matches "Beagle", "Bulldog", "Bengal Cat"
	const string SearchQuery = "B";

	[Test]
	[Category(UITestCategories.Shell)]
	public void ShowsResultsInitialFalse_DoesNotShowResults()
	{
		App.WaitForElement("Issue35767ResetButton");
		App.Tap("Issue35767ResetButton");

		var searchHandler = App.GetShellSearchHandler();
		searchHandler.Tap();
		searchHandler.SendKeys(SearchQuery);

#if ANDROID
		Assert.That(App.FindElement("Issue35767StatusLabel").GetText(), Is.EqualTo("None"),
			"No item should be selected when ShowsResults=false");
#else
		App.WaitForNoElement("Issue35767ResultItem");
#endif
	}

	[Test]
	[Category(UITestCategories.Shell)]
	public void ShowsResultsDynamic_TrueShowsResults()
	{
		App.WaitForElement("Issue35767ResetButton");
		App.Tap("Issue35767ResetButton");
		App.Tap("Issue35767TrueButton");

		var searchHandler = App.GetShellSearchHandler();
		searchHandler.Tap();
		searchHandler.SendKeys(SearchQuery);

#if ANDROID
		var rect = searchHandler.GetRect();
		App.TapCoordinates(rect.X + 10, rect.Y + rect.Height + 10);
		Assert.That(App.FindElement("Issue35767StatusLabel").GetText(), Is.Not.EqualTo("None"),
			"An item should be selectable when ShowsResults=true");
#else
		App.WaitForElement("Issue35767ResultItem");
#endif
	}

	[Test]
	[Category(UITestCategories.Shell)]
	public void ShowsResultsDynamic_FalseHidesResults()
	{
		App.WaitForElement("Issue35767ResetButton");
		App.Tap("Issue35767ResetButton");
		App.Tap("Issue35767TrueButton");

#if ANDROID
		var searchHandler = App.GetShellSearchHandler();
		searchHandler.Tap();
		searchHandler.SendKeys(SearchQuery);

		App.Tap("Issue35767FalseButton");
		searchHandler.SendKeys("e");

		var rect = searchHandler.GetRect();
		App.TapCoordinates(rect.X + 10, rect.Y + rect.Height + 10);
		Assert.That(App.FindElement("Issue35767StatusLabel").GetText(), Is.EqualTo("None"),
			"No item should be selectable after ShowsResults toggled back to false");
#else
		App.Tap("Issue35767FalseButton");

		var searchHandler = App.GetShellSearchHandler();
		searchHandler.Tap();
		searchHandler.SendKeys(SearchQuery);

		App.WaitForNoElement("Issue35767ResultItem");
#endif
	}
}
