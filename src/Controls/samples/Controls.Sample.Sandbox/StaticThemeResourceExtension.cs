namespace Maui.Controls.Sample
{

    // All the code in this file is included in all platforms.
    [RequireService(new Type[]{typeof(IServiceProvider) })]
    public class StaticThemeResourceExtension : IMarkupExtension
    {
        /// <summary>
        /// Gets or sets the Resource Key value of the Markup Extension. This Resource Key is used to get the extended key's object.
        /// </summary>
        public string? ResourceKey { get; set; }

        /// <summary>
        /// Returns the object created for Resource Key from the Resource Dictionary. If the Resource Key is not found in the Resource Dictionary, an exception will be thrown.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object? ProvideValue(IServiceProvider serviceProvider)
        {
            if (ResourceKey == null || serviceProvider == null)
                return null;

            if (serviceProvider.GetService(typeof(IRootObjectProvider)) is IRootObjectProvider rootObjectProvider)
            {
                if (rootObjectProvider.RootObject is ResourceDictionary themeResourceDictionary)
                {
                    var mergedDictionaries = themeResourceDictionary.MergedDictionaries ?? Enumerable.Empty<ResourceDictionary>();

                    foreach (var item in mergedDictionaries)
                    {
                        if (item.Keys.Contains(ResourceKey))
                        {
                            return item[ResourceKey];
                        }
                    }

                    if (themeResourceDictionary.Keys.Contains(ResourceKey))
                        return themeResourceDictionary[ResourceKey];
                    else
                        throw new KeyNotFoundException("The resource '" + ResourceKey + "' is not present in the dictionary.");
                }
            }

            return null;
        }
    }
}
