using System;

namespace Eto.Forms
{
	public interface IMenu : IInstanceWidget
	{
	}

	public abstract class Menu : CommandBase
	{
		//IMenu inner;

		protected Menu (Generator g, Type type, bool initialize = true)
			: base (g, type, initialize)
		{
			//inner = (IMenu)base.Handler;
		}

	}
}