using UnityEngine;

namespace Shared
{
	public class Config
	{
		public ButtonConfig[] buttons;

		public Config()
		{
			buttons = new[]
			{
				new ButtonConfig()
				{
					configId = "Blue",
					position = new Vector3(-7.3f, 0, -12.03f),
					radius = 1
				},

				new ButtonConfig()
				{
					configId = "Red",
					position = new Vector3(8, 0, -12.03f),
					radius = 1
				}
			};
		}

		public class ButtonConfig
		{
			public string configId;
			public Vector3 position;
			public float radius;
		}
	}
}


