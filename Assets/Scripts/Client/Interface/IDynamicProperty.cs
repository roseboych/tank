using System;

namespace Client
{
	public interface IDynamicProperty
	{
		void OnChangeProperty(int id, object oldValue, object newValue);
		PropertyItem GetProperty(int id);
	}
}

