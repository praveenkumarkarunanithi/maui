// Crash is Android-specific: RenderThread GL functor receives zero-area Skia canvas when ClipBounds=(0,0,0,0) at (w>0,h=0)
#if ANDROID
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue35771 : _IssuesUITest
{
	public Issue35771(TestDevice device) : base(device) { }

	public override string Issue => "Android SIGSEGV crash with multiple auto-sizing WebViews in ScrollView on navigated page";

	[Test]
	[Category(UITestCategories.WebView)]
	public void MultipleAutoSizingWebViewsInScrollViewShouldNotCrash()
	{
		App.WaitForElement("Issue35771NavigateButton");
		App.Tap("Issue35771NavigateButton");
		App.WaitForElement("Issue35771Ready");
	}
}
#endif
