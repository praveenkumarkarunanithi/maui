using System.Collections.ObjectModel;
using Maui.Controls.Sample.Models;

namespace Maui.Controls.Sample.ViewModels
{
	/// <summary>
	/// ViewModel for the chat demonstration page.
	/// Manages a collection of chat messages to display WebView sizing behavior.
	/// </summary>
	public class ChatViewModel
	{
		public ObservableCollection<ChatMessage> Messages { get; set; }

		public ChatViewModel()
		{
			Messages = new ObservableCollection<ChatMessage>();
			LoadSampleMessages();
		}

		/// <summary>
		/// Populates sample messages with varying HTML content lengths.
		/// This demonstrates different WebView sizing scenarios:
		/// - Short content
		/// - Multi-line content
		/// - Long content with formatting
		/// </summary>
		private void LoadSampleMessages()
		{
			// User message 1: Short content
			Messages.Add(new ChatMessage
			{
				Id = 1,
				IsUser = true,
				HtmlContent = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 0; padding: 8px; font-size: 14px; color: #333; }
                            p { margin: 0; line-height: 1.4; }
                        </style>
                    </head>
                    <body>
                        <p>Hello! How are you?</p>
                    </body>
                    </html>"
			});

			// Bot message 1: Multi-line content
			Messages.Add(new ChatMessage
			{
				Id = 2,
				IsUser = false,
				HtmlContent = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 0; padding: 8px; font-size: 14px; color: #333; }
                            p { margin: 0; line-height: 1.4; }
                        </style>
                    </head>
                    <body>
                        <p>I'm doing great, thanks for asking!<br/>
                        This is a multi-line response.<br/>
                        It contains multiple lines of text.</p>
                    </body>
                    </html>"
			});

			// User message 2: Medium content
			Messages.Add(new ChatMessage
			{
				Id = 3,
				IsUser = true,
				HtmlContent = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 0; padding: 8px; font-size: 14px; color: #333; }
                            p { margin: 0; line-height: 1.4; }
                        </style>
                    </head>
                    <body>
                        <p>Can you help me understand WebView sizing issues?<br/>
                        I'm trying to render dynamic HTML content.</p>
                    </body>
                    </html>"
			});

			// Bot message 2: Long content with formatting
			Messages.Add(new ChatMessage
			{
				Id = 4,
				IsUser = false,
				HtmlContent = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 0; padding: 8px; font-size: 14px; color: #333; }
                            p { margin: 0 0 6px 0; line-height: 1.4; }
                            strong { color: #1976d2; }
                        </style>
                    </head>
                    <body>
                        <p><strong>Absolutely!</strong> WebView sizing in MAUI can be tricky.</p>
                        <p>Common issues include:</p>
                        <p>• Incorrect height calculation<br/>
                        • Clipped content<br/>
                        • Extra empty space<br/>
                        • Layout not expanding as expected</p>
                    </body>
                    </html>"
			});

			// User message 3: Short with emphasis
			Messages.Add(new ChatMessage
			{
				Id = 5,
				IsUser = true,
				HtmlContent = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 0; padding: 8px; font-size: 14px; color: #333; }
                            p { margin: 0; line-height: 1.4; }
                        </style>
                    </head>
                    <body>
                        <p>Why does this happen?</p>
                    </body>
                    </html>"
			});

			// Bot message 3: Detailed explanation with multiple paragraphs
			Messages.Add(new ChatMessage
			{
				Id = 6,
				IsUser = false,
				HtmlContent = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: Arial, sans-serif; margin: 0; padding: 8px; font-size: 14px; color: #333; }
                            p { margin: 0 0 6px 0; line-height: 1.4; }
                            .reason { margin-left: 8px; }
                        </style>
                    </head>
                    <body>
                        <p><strong>Several factors contribute:</strong></p>
                        <div class=""reason"">
                            <p><strong>1. Layout Measurement:</strong> WebView must report its desired size to the parent layout, but HTML content size is not always known immediately.</p>
                            <p><strong>2. Async Rendering:</strong> WebView rendering happens asynchronously. The layout is calculated before content is fully rendered.</p>
                            <p><strong>3. Platform Differences:</strong> Android, iOS, and Windows handle WebView measurement differently.</p>
                        </div>
                    </body>
                    </html>"
			});
		}
	}
}
