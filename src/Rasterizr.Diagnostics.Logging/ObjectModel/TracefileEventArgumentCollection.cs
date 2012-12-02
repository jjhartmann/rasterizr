using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class TracefileEventArgumentCollection : Collection<object>
	{
		public TracefileEventArgumentCollection(IList<object> list)
			: base(list)
		{
			
		}

		public TracefileEventArgumentCollection()
		{
			
		}

		public T Get<T>(int index)
		{
			var type = typeof(T);
			if (this[index].GetType() != type)
			{
				if (type.IsEnum)
					type = typeof(int);
				return (T) Convert.ChangeType(this[index], type);
			}
			return (T) this[index];
		}
	}
}