#if IOS || MACCATALYST //Issue is specific to the CollectionView2 handler, which is available only on these platforms.
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	internal class Issue27667 : _IssuesUITest
	{
		public Issue27667(TestDevice device) : base(device) { }

		public override string Issue => "CollectionView with Horizontal GridItemsLayout Becomes Unresponsive with Increased Column Span Updates on iOS Platform";

		[Test]
		[Category(UITestCategories.CollectionView)]
		public void MeasureInvalidatedNotCalledExcessivelyOnCollectionViewSpanChange()
        {
			App.WaitForElement("CollectionViewWithGrid");
            App.Tap("UpdateColumnsButton");
            var countValue = App.FindElement("MeasureCountValue").GetText();
            Assert.That(int.Parse(countValue ?? "0"), Is.EqualTo(9), 
        "MeasureInvalidated should be called exactly 9 times");
        }
	}
}
#endif