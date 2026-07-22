namespace Maui.Controls.Sample.Models
{
	/// <summary>
	/// Represents a chat message in the conversation.
	/// Demonstrates WebView sizing behavior with HTML content.
	/// </summary>
	public class ChatMessage
	{
		/// <summary>
		/// HTML content to be rendered in the WebView
		/// </summary>
		public string? HtmlContent { get; set; }

		/// <summary>
		/// Indicates if this is a user message (true) or bot response (false)
		/// </summary>
		public bool IsUser { get; set; }

		/// <summary>
		/// Unique identifier for each message
		/// </summary>
		public int Id { get; set; }
	}
}
