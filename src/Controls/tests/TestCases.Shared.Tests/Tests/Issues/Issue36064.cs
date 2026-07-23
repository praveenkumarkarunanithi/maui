using System;
using System.Threading;
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue36064 : _IssuesUITest
{
	public Issue36064(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "WebView does not measure and size correctly across platforms";

	[Test]
	[Category(UITestCategories.WebView)]
	public void ChatWebViewsRenderCorrectly()
	{
		App.WaitForElement("Issue36064CollectionView", timeout: TimeSpan.FromSeconds(30));
		// WebView content renders asynchronously; brief settle so HTML is laid out
		// before the screenshot.
		Thread.Sleep(500);
		VerifyScreenshot();
	}
}
