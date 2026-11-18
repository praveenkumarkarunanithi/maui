using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platform;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	// Regression tests for https://github.com/dotnet/maui/issues/34380
	// [iOS] VoiceOver does not correctly describe a View with GestureRecognizers when it has
	// SemanticProperties.Hint and child Labels — should synthesize an accessibility label from
	// children, promote the layout to an accessibility element, and route VoiceOver activation
	// to the layout's TapGestureRecognizer.
	public partial class AccessibilityTests
	{
		[Category(TestCategory.Accessibility)]
		[Collection(ControlsHandlerTestBase.RunInNewWindowCollection)]
		public class Issue34380Tests : ControlsHandlerTestBase
		{
			void SetupBuilder()
			{
				EnsureHandlerCreated(builder =>
				{
					builder.ConfigureMauiHandlers(handlers =>
					{
						handlers.AddMauiControlsHandlers();
						handlers.AddHandler(typeof(Window), typeof(WindowHandlerStub));
					});
				});
			}

			[Fact("Layout with Hint and child Labels synthesizes a combined AccessibilityLabel")]
			public async Task LayoutWithHintAndChildLabels_SynthesizesAccessibilityLabel()
			{
				SetupBuilder();

				var layout = new VerticalStackLayout();
				layout.Add(new Label { Text = "First line" });
				layout.Add(new Label { Text = "Second line" });
				SemanticProperties.SetHint(layout, "Activates the action");

				var page = new ContentPage { Content = layout };

				await CreateHandlerAndAddToWindow<IWindowHandler>(page, async (handler) =>
				{
					await Task.Delay(100);

					var platformView = (UIView)layout.Handler.PlatformView;

					Assert.True(platformView.IsAccessibilityElement,
						"Layout with Hint should be promoted to an accessibility element.");

					Assert.False(string.IsNullOrEmpty(platformView.AccessibilityLabel),
						"AccessibilityLabel should be synthesized from child Labels.");

					Assert.Contains("First line", platformView.AccessibilityLabel, StringComparison.Ordinal);
					Assert.Contains("Second line", platformView.AccessibilityLabel, StringComparison.Ordinal);

					Assert.Equal("Activates the action", platformView.AccessibilityHint);
				});
			}

			[Fact("Layout with TapGestureRecognizer (no Hint) sets ShouldGroupAccessibilityChildren")]
			public async Task LayoutWithTapGesture_SetsShouldGroupAccessibilityChildren()
			{
				SetupBuilder();

				var layout = new VerticalStackLayout();
				layout.Add(new Label { Text = "Tap me" });

				var tap = new TapGestureRecognizer();
				layout.GestureRecognizers.Add(tap);

				var page = new ContentPage { Content = layout };

				await CreateHandlerAndAddToWindow<IWindowHandler>(page, async (handler) =>
				{
					await Task.Delay(100);

					var platformView = (UIView)layout.Handler.PlatformView;

					Assert.True(platformView.ShouldGroupAccessibilityChildren,
						"Layout with TapGestureRecognizer should group accessibility children so the tap target is reachable by VoiceOver.");
				});
			}

			[Fact("AccessibilityActivate on a layout with TapGestureRecognizer fires the gesture")]
			public async Task AccessibilityActivate_InvokesTapGesture()
			{
				SetupBuilder();

				var layout = new VerticalStackLayout();
				layout.Add(new Label { Text = "Tap" });
				SemanticProperties.SetHint(layout, "Tap to act");

				bool tapped = false;
				var tap = new TapGestureRecognizer();
				tap.Tapped += (s, e) => tapped = true;
				layout.GestureRecognizers.Add(tap);

				var page = new ContentPage { Content = layout };

				await CreateHandlerAndAddToWindow<IWindowHandler>(page, async (handler) =>
				{
					await Task.Delay(100);

					var platformView = (UIView)layout.Handler.PlatformView;
					var activated = platformView.AccessibilityActivate();

					Assert.True(activated, "AccessibilityActivate should report that the activation was handled.");
					Assert.True(tapped, "TapGestureRecognizer.Tapped should fire when VoiceOver activates the layout.");
				});
			}
		}

		// https://github.com/dotnet/maui/issues/33612 — participation is resolved by
		// AccessibilityRoleResolver from semantics, AutomationProperties, gestures, handler
		// defaults and live content, instead of being written by each mapper independently.
		[Category(TestCategory.Accessibility)]
		[Collection(ControlsHandlerTestBase.RunInNewWindowCollection)]
		public class Issue33612Tests : ControlsHandlerTestBase
		{
			void SetupBuilder()
			{
				EnsureHandlerCreated(builder =>
				{
					builder.ConfigureMauiHandlers(handlers =>
					{
						handlers.AddMauiControlsHandlers();
						handlers.AddHandler(typeof(Window), typeof(WindowHandlerStub));
					});
				});
			}

			static Border CreateTappableItem(string text, string description = null)
			{
				var border = new Border { Content = new Label { Text = text } };
				if (description is not null)
				{
					SemanticProperties.SetDescription(border, description);
					SemanticProperties.SetHint(border, "Double tap to activate");
				}

				border.GestureRecognizers.Add(new TapGestureRecognizer());
				return border;
			}

			static UIView PlatformViewOf(VisualElement element) => (UIView)element.Handler.PlatformView;

			Task RunInWindow(View content, Action action) =>
				CreateHandlerAndAddToWindow<IWindowHandler>(new ContentPage { Content = content }, async _ =>
				{
					await Task.Delay(100);
					action();
				});

			[Fact("Container Description keeps actionable children reachable as a labelled group")]
			public async Task ContainerWithActionableChildren_IsLabelledSemanticGroup()
			{
				SetupBuilder();

				var item = CreateTappableItem("First item", "First item");
				var container = new Controls.ContentView { Content = new VerticalStackLayout { item } };
				SemanticProperties.SetDescription(container, "Suggestions");

				await RunInWindow(container, () =>
				{
					var containerView = PlatformViewOf(container);
					Assert.False(containerView.IsAccessibilityElement, "Container must not collapse actionable children.");
					Assert.Equal("Suggestions", containerView.AccessibilityLabel);
					Assert.Equal((nint)(long)UIAccessibilityContainerType.SemanticGroup, ((Foundation.NSNumber)containerView.ValueForKey(new Foundation.NSString("accessibilityContainerType"))).NIntValue);

					var itemView = PlatformViewOf(item);
					Assert.True(itemView.IsAccessibilityElement, "Tappable item with semantics must be an element.");
					Assert.Equal("First item", itemView.AccessibilityLabel);
					Assert.Equal("Double tap to activate", itemView.AccessibilityHint);
					Assert.True((itemView.AccessibilityTraits & UIAccessibilityTrait.Button) == UIAccessibilityTrait.Button);
				});
			}

			[Fact("Container Description with decorative content stays a single element")]
			public async Task ContainerWithPassiveContent_IsElement()
			{
				SetupBuilder();

				var container = new Controls.ContentView { Content = new Image() };
				SemanticProperties.SetDescription(container, "Logo");

				await RunInWindow(container, () =>
				{
					var containerView = PlatformViewOf(container);
					Assert.True(containerView.IsAccessibilityElement, "Description must remain reachable when content is not actionable.");
					Assert.Equal("Logo", containerView.AccessibilityLabel);
				});
			}

			[Fact("Content and gesture changes after connection re-evaluate the container")]
			public async Task DynamicContentAndGestures_ReevaluateContainer()
			{
				SetupBuilder();

				var child = new Border { Content = new Label { Text = "Item" } };
				var container = new Controls.ContentView { Content = child };
				SemanticProperties.SetDescription(container, "Suggestions");

				await RunInWindow(container, () =>
				{
					var containerView = PlatformViewOf(container);
					Assert.True(containerView.IsAccessibilityElement);

					var tap = new TapGestureRecognizer();
					child.GestureRecognizers.Add(tap);
					Assert.False(containerView.IsAccessibilityElement, "Adding a gesture to a child must expose it.");

					child.GestureRecognizers.Remove(tap);
					Assert.True(containerView.IsAccessibilityElement, "Removing the gesture must collapse the container again.");

					container.Content = new VerticalStackLayout { CreateTappableItem("New item") };
					Assert.False(containerView.IsAccessibilityElement, "Replacing content must re-evaluate the container.");
				});
			}

			[Fact("IsInAccessibleTree=false wins over Description and is restored when cleared")]
			public async Task IsInAccessibleTreeOverride_WinsAndRestores()
			{
				SetupBuilder();

				var view = CreateTappableItem("Item", "Item");
				AutomationProperties.SetIsInAccessibleTree(view, false);
				SemanticProperties.SetHeadingLevel(view, SemanticHeadingLevel.Level1);

				await RunInWindow(view, () =>
				{
					var platformView = PlatformViewOf(view);
					Assert.False(platformView.IsAccessibilityElement, "Explicit override must win over semantics and gestures.");
					Assert.True((platformView.AccessibilityTraits & UIAccessibilityTrait.Header) == 0, "Excluded view must not be a heading.");

					SemanticProperties.SetDescription(view, "Changed");
					Assert.False(platformView.IsAccessibilityElement, "A later semantics change must not undo the override.");

					view.ClearValue(AutomationProperties.IsInAccessibleTreeProperty);
					Assert.True(platformView.IsAccessibilityElement, "Clearing the override must restore the resolved role.");
				});
			}

			[Fact("RadioButton keeps its handler-established accessibility element")]
			public async Task RadioButtonWithDescription_RemainsElement()
			{
				SetupBuilder();

				var radioButton = new RadioButton { Content = "Option" };
				SemanticProperties.SetDescription(radioButton, "Option description");

				await RunInWindow(new VerticalStackLayout { radioButton }, () =>
				{
					var platformView = PlatformViewOf(radioButton);
					Assert.True(platformView.IsAccessibilityElement);
					Assert.Equal("Option description", platformView.AccessibilityLabel);
				});
			}
		}
	}
}

