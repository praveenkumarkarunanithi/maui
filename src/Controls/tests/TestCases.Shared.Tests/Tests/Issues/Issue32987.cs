#if ANDROID
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue32987 : _IssuesUITest
{
	public Issue32987(TestDevice device) : base(device) { }

	public override string Issue => "TabBar behavior inconsistent with API 36 and API 34 devices";

	[Test]
	[Category(UITestCategories.Navigation)]
	public void ActivityWindowDisablesSystemBarContrastScrim()
	{
		App.WaitForElement("Issue32987ActivityNavBarContrastLabel");

		// Page forces transparent bars to recreate the bug scenario.
		AssertLabel("Issue32987ActivityNavBarColorLabel", "ActivityNavBar=0x00000000",
			"Activity navigation bar must be transparent to recreate the bug scenario.");
		AssertLabel("Issue32987ActivityStatusBarColorLabel", "ActivityStatusBar=0x00000000",
			"Activity status bar must be transparent to recreate the bug scenario.");
		// Fix asserts: without the fix these would default back to True.
		AssertLabel("Issue32987ActivityNavBarContrastLabel", "ActivityNavBarContrast=False",
			"NavigationBarContrastEnforced must be False (OS would otherwise draw a contrast scrim).");
		AssertLabel("Issue32987ActivityStatusBarContrastLabel", "ActivityStatusBarContrast=False",
			"StatusBarContrastEnforced must be False (OS would otherwise draw a contrast scrim).");
	}

	void AssertLabel(string automationId, string expected, string failureMessage)
	{
		var actual = App.FindElement(automationId).GetText();
		Assert.That(actual, Is.EqualTo(expected), failureMessage);
	}
}
#endif
