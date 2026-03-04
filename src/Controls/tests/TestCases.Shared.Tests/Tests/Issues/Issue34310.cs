using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue34310 : _IssuesUITest
{
	public Issue34310(TestDevice device) : base(device) { }

	public override string Issue => "Loaded event not called for MAUI View added to native View";

	[Test]
	[Category(UITestCategories.LifeCycle)]
	public void LoadedEventFiresWhenViewAddedToNativeView()
	{
		App.WaitForElement("LoadedStatus");

		// Allow the 2-second delay inside the page to pass so all Loaded events have a
		// chance to fire even on slow emulators/simulators before we read the label.
		Task.Delay(3000).Wait();

		var status = App.FindElement("LoadedStatus").GetText();

		Assert.That(status, Is.EqualTo("Grid=True,Label=True,Button=True"),
			$"Loaded events did not fire for all controls. Status: {status}");
	}
}
