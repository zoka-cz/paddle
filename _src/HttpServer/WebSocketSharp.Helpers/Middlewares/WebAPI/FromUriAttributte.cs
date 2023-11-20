using System;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.WebAPI
{
	/// <summary>The parameter should be provided from uri</summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public class FromUriAttribute : ParameterSourceAttribute
	{
	}
}
