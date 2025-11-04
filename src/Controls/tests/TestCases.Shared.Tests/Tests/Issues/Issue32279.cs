using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue32279 : _IssuesUITest
{
	public Issue32279(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "TapGestureRecognizer not working on ContentView without background on Windows";

	[Test]
	[Category(UITestCategories.Gestures)]
	public void TapGestureRecognizerWorksOnContentViewWithoutBackground()
	{
		App.WaitForElement("TestContentView");
				
		App.Tap("TestContentView");
		
		var result = App.FindElement("Result").GetText();
		Assert.That(result, Is.EqualTo("Tapped!"));
	}
}
