using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using ELMAH_Viewer.Common.Annotations;

namespace ELMAH_Viewer.Common
{
	public class NotifyProxy<T> : DynamicObject, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public T Original { get; set; }

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private PropertyInfo GetMember(string name)
		{
			return Original.GetType().GetProperty(name);
		}

		public NotifyProxy(T original)
		{
			Original = original;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			PropertyInfo pi = GetMember(binder.Name);
			result = pi.GetValue(Original);
			return true;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			GetMember(binder.Name).SetValue(Original, value);
			OnPropertyChanged(binder.Name);
			return true;
		}
	}
}
