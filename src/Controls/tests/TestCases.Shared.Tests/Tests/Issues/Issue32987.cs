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
	public void ModalDialogWindowIsEdgeToEdge()
	{
		// Bug only affects Android API 30-34. API 35+ already applies edge-to-edge to
		// dialog windows by default, so there is nothing to verify there.
		var deviceApiLevel = (long?)((AppiumApp)App).Driver.Capabilities.GetCapability("deviceApiLevel")
			?? throw new InvalidOperationException("deviceApiLevel capability is missing or null.");
		if (deviceApiLevel < 30 || deviceApiLevel >= 35)
		{
			Assert.Ignore($"Issue32987 only reproduces on Android API 30-34. Current device API level: {deviceApiLevel}.");
		}

		App.WaitForElement("ShowModalButton");
		App.Tap("ShowModalButton");
		App.WaitForElement("Issue32987NavBarColorLabel");

		Assert.That(App.FindElement("Issue32987NavBarColorLabel").GetText(), Is.EqualTo("NavBar=0x00000000"));
		Assert.That(App.FindElement("Issue32987StatusBarColorLabel").GetText(), Is.EqualTo("StatusBar=0x00000000"));
		Assert.That(App.FindElement("Issue32987NavBarContrastLabel").GetText(), Is.EqualTo("NavBarContrast=False"));
		Assert.That(App.FindElement("Issue32987StatusBarContrastLabel").GetText(), Is.EqualTo("StatusBarContrast=False"));
	}
}
#endif
