using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue26644 : _IssuesUITest
	{
		public Issue26644(TestDevice device) : base(device) { }

		public override string Issue => "[iOS] Label with a fixed WidthRequest has wrong height";

		[Test]
		[Category(UITestCategories.Label)]
		public void LabelWithBackgroundAndWidthRequestShouldNotClipText()
		{
			App.WaitForElement("Issue26644ReferenceWithBg");
			var referenceHeight = App.FindElement("Issue26644ReferenceWithBg").GetRect().Height;

			App.Tap("Issue26644ButtonWithBg");
			App.WaitForElement("Issue26644ButtonWithBg");

			var testHeight = App.FindElement("Issue26644TestLabelWithBg").GetRect().Height;

			Assert.That(testHeight, Is.GreaterThanOrEqualTo(referenceHeight).Within(3),
				$"Label with BackgroundColor and WidthRequest=200 clipped after text update. " +
				$"Reference: {referenceHeight}px, Actual: {testHeight}px.");
		}

		[Test]
		[Category(UITestCategories.Label)]
		public void LabelWithoutBackgroundAndWidthRequestShouldNotClipText()
		{
			App.WaitForElement("Issue26644ReferenceNoBg");
			var referenceHeight = App.FindElement("Issue26644ReferenceNoBg").GetRect().Height;

			App.Tap("Issue26644ButtonNoBg");
			App.WaitForElement("Issue26644ButtonNoBg");

			var testHeight = App.FindElement("Issue26644TestLabelNoBg").GetRect().Height;

			Assert.That(testHeight, Is.GreaterThanOrEqualTo(referenceHeight).Within(3),
				$"Label with WidthRequest=200 clipped after text update. " +
				$"Reference: {referenceHeight}px, Actual: {testHeight}px.");
		}
	}
}
