#nullable disable
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Xaml;

namespace Microsoft.Maui.Controls
{
	/// <summary>
	/// Sets a property value within a <see cref="Style"/> or <see cref="TriggerBase"/>.
	/// </summary>
	[ContentProperty(nameof(Value))]
	[ProvideCompiled("Microsoft.Maui.Controls.XamlC.SetterValueProvider")]
	[RequireService(
		[typeof(IValueConverterProvider),
		 typeof(IXmlLineInfoProvider)])]
	public sealed class Setter : IValueProvider
	{
		// When Value is an Element (e.g. a nested settings object in a VSM state), the same instance
		// may be shared across multiple controls. We register a unique closure-based listener per
		// target so that SetParent's RemoveResourcesChangedListener (which uses the method reference
		// directly) never accidentally removes our registration from other targets.
		Dictionary<BindableObject, Action<object, ResourcesChangedEventArgs>> _appliedElementListeners;

		/// <summary>
		/// Gets or sets the name of the element to which the setter applies.
		/// </summary>
		public string TargetName { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="BindableProperty"/> to set.
		/// </summary>
		public BindableProperty Property { get; set; }

		/// <summary>
		/// Gets or sets the value to apply to the property.
		/// </summary>
		public object Value { get; set; }

		object IValueProvider.ProvideValue(IServiceProvider serviceProvider)
		{
			if (Property == null)
				throw new XamlParseException("Property not set", serviceProvider);
			var valueconverter = serviceProvider.GetService(typeof(IValueConverterProvider)) as IValueConverterProvider;

			MemberInfo minforetriever()
			{
				MemberInfo minfo = null;
				try
				{
					minfo = Property.DeclaringType.GetRuntimeProperty(Property.PropertyName);
				}
				catch (AmbiguousMatchException e)
				{
					throw new XamlParseException($"Multiple properties with name '{Property.DeclaringType}.{Property.PropertyName}' found.", serviceProvider, innerException: e);
				}
				if (minfo != null)
					return minfo;
				try
				{
					return Property.DeclaringType.GetRuntimeMethod("Get" + Property.PropertyName, new[] { typeof(BindableObject) });
				}
				catch (AmbiguousMatchException e)
				{
					throw new XamlParseException($"Multiple methods with name '{Property.DeclaringType}.Get{Property.PropertyName}' found.", serviceProvider, innerException: e);
				}
			}

			object value = valueconverter.Convert(Value, Property.ReturnType, minforetriever, serviceProvider);
			Value = value;
			return this;
		}

		internal void Apply(BindableObject target, SetterSpecificity specificity)
		{
			if (target == null)
				throw new ArgumentNullException(nameof(target));

			var targetObject = target;

			if (!string.IsNullOrEmpty(TargetName) && target is Element element)
				targetObject = element.FindByName(TargetName) as BindableObject ?? throw new XamlParseException($"Cannot resolve '{TargetName}' as Setter Target for '{target}'.");

			if (Property == null)
				return;

			if (Value is BindingBase binding)
				targetObject.SetBinding(Property, binding.Clone(), specificity);
			else if (Value is DynamicResource dynamicResource)
				targetObject.SetDynamicResource(Property, dynamicResource.Key, specificity);
			else if (Value is IList<VisualStateGroup> visualStateGroupCollection)
				targetObject.SetValue(Property, visualStateGroupCollection.Clone(), specificity);
			else
			{
				targetObject.SetValue(Property, Value, specificity: specificity);

				// When the setter value is an Element with DynamicResource bindings (e.g. a nested
				// settings object in a VSM state), the same instance may be shared across multiple
				// controls. We register a UNIQUE closure-based listener per (Setter, target) pair
				// so that Element.SetParent's RemoveResourcesChangedListener calls (which use the
				// direct method reference) never accidentally remove our registration from other targets.
				if (Value is Element elementValue && targetObject is IElementDefinition targetDef)
				{
					System.Diagnostics.Debug.WriteLine($"[THEME-DBG-SETTER] Apply: Element({elementValue.GetHashCode()}) → Target({targetObject.GetHashCode()}:{targetObject.GetType().Name}): registering resource listener");

					// Remove any existing listener for this target first (handles re-apply case).
					if (_appliedElementListeners?.TryGetValue(targetObject, out var existingListener) == true)
					{
						targetDef.RemoveResourcesChangedListener(existingListener);
						_appliedElementListeners.Remove(targetObject);
					}

					// Create a closure unique to this (Setter instance, target) pair.
					// Using a closure rather than the method reference directly means SetParent's
					// RemoveResourcesChangedListener(OnParentResourcesChanged) won't match ours.
					Action<object, ResourcesChangedEventArgs> listener = (s, e) => elementValue.OnParentResourcesChanged(s, e);
					targetDef.AddResourcesChangedListener(listener);
					(_appliedElementListeners ??= new Dictionary<BindableObject, Action<object, ResourcesChangedEventArgs>>())[targetObject] = listener;

					System.Diagnostics.Debug.WriteLine($"[THEME-DBG-SETTER] Apply done: Element({elementValue.GetType().Name}/{elementValue.GetHashCode()}) listener registered on Target({targetObject.GetHashCode()})");
				}
			}
		}

		internal void UnApply(BindableObject target, SetterSpecificity specificity)
		{
			if (target == null)
				throw new ArgumentNullException(nameof(target));

			var targetObject = target;

			if (!string.IsNullOrEmpty(TargetName) && target is Element element)
				targetObject = element.FindByName(TargetName) as BindableObject ?? throw new ArgumentNullException(nameof(targetObject));

			if (Property == null)
				return;
			if (Value is BindingBase binding)
				targetObject.RemoveBinding(Property, specificity);
			else if (Value is DynamicResource dynamicResource)
				targetObject.RemoveDynamicResource(Property, specificity);

			// Undo the direct resource-listener registration added in Apply.
			if (Value is Element elementValue && targetObject is IElementDefinition targetDef)
			{
				System.Diagnostics.Debug.WriteLine($"[THEME-DBG-SETTER] UnApply: removing Element({elementValue.GetHashCode()}) listener from Target({targetObject.GetHashCode()})");
				if (_appliedElementListeners?.TryGetValue(targetObject, out var listener) == true)
				{
					targetDef.RemoveResourcesChangedListener(listener);
					_appliedElementListeners.Remove(targetObject);
				}
			}

			targetObject.ClearValue(Property, specificity);
		}
	}
}