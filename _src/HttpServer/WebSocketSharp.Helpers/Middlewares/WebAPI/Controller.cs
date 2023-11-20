namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.WebAPI
{
	/// <summary>Base class for WebAPI controllers of SmallServer</summary>
	public class Controller
	{
		/// <summary>Context for the current request</summary>
		public WebSocketSharpMiddlewareContext				Context { get; internal set; }
	}
}
