namespace Microsoft.Maui
{
	/// <summary>
	/// Provides functionality for the Page's SafeAreaInsets that may be changed in the future.
	/// </summary>
	/// <remarks>
	/// This interface is only recognized on the iOS/Mac Catalyst platforms; other platforms will ignore it.
	/// </remarks>
	internal interface ISafeAreaView2
	{
		/// <summary>
		/// Internal property for the Page's SafeAreaInsets Thickness that may be changed in the future.
		/// </summary>
		internal Thickness SafeAreaInsets { set; }

		/// <summary>
		/// Gets the safe area regions for the specified edge (0=Left, 1=Top, 2=Right, 3=Bottom).
		/// </summary>
		/// <param name="edge">The edge to get the behavior for (0=Left, 1=Top, 2=Right, 3=Bottom).</param>
		/// <returns>The SafeAreaRegions behavior for this edge.</returns>
		SafeAreaRegions GetSafeAreaRegionsForEdge(int edge);

		/// <summary>
		/// Returns true when this view explicitly opts out of the top inset and wants to
		/// prevent children from applying it as padding (true edge-to-edge).
		/// Default is false (pass the inset through to children unchanged).
		/// </summary>
		bool ShouldConsumeTopInsetForChildren() => false;
	}
}
