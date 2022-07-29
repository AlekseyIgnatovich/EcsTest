using Shared;
using UnityEngine;

namespace Client
{
	public class ClientTimeProvider : ITimeProvider
	{
		public float DeltaTime => Time.deltaTime;
	}
}
