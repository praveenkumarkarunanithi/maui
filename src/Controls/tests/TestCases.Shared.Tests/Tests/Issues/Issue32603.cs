using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue32603 : _IssuesUITest
{
	public Issue32603(TestDevice device) : base(device) { }

	public override string Issue => "NET10 - Content in WebView on iOS too small now";

	[Test]
	[Category(UITestCategories.WebView)]
	public void WebViewShouldUseDeviceWidthViewport()
	{
		// This is an iOS-only regression: without a viewport meta tag, iOS .NET 10
		// defaults to a 980px desktop viewport, causing HTML content to appear
		// ~1.5x smaller than a native MAUI Label at the same font size.
		if (Device != TestDevice.iOS)
			Assert.Ignore("This is an iOS-only regression (#32603).");

		App.WaitForElement("TestWebView");

		// Wait for JS evaluation to complete and populate the status label.
		App.WaitForTextToBePresentInElement("ViewportStatusLabel", "Viewport");

		var status = App.FindElement("ViewportStatusLabel").GetText();

		// When fixed: WebView uses device-width viewport (innerWidth < 500px) → "ViewportCorrect"
		// When bug is present: WebView uses 980px default viewport → "ViewportTooWide"
		Assert.That(status, Is.EqualTo("ViewportCorrect"),
			"WebView should use device-width viewport. A 980px default viewport causes HTML content to appear smaller than native MAUI controls.");
	}
}
