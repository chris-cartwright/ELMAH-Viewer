using System.Windows;
using Xceed.Wpf.Toolkit;

namespace ELMAH_Viewer.Controls
{
	/// <summary>
	/// Interaction logic for NumericSpinner.xaml
	/// </summary>
	public partial class NumericSpinner
	{
		public static readonly DependencyProperty MinValueProperty;
		public static readonly DependencyProperty MaxValueProperty;
		public static readonly DependencyProperty ValueProperty;

		static NumericSpinner()
		{
			MinValueProperty = DependencyProperty.RegisterAttached("MinValue", typeof(long), typeof(NumericSpinner),
				new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });

			MaxValueProperty = DependencyProperty.RegisterAttached("MaxValue", typeof(long), typeof(NumericSpinner),
				new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });

			ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(long), typeof(NumericSpinner),
				new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });
		}

		public long MinValue
		{
			get { return (long)GetValue(MinValueProperty); }
			set { SetValue(MinValueProperty, value); }
		}

		public long MaxValue
		{
			get { return (long)GetValue(MaxValueProperty); }
			set { SetValue(MaxValueProperty, value); }
		}

		public long Value
		{
			get { return (long)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public NumericSpinner()
		{
			MinValue = 0;
			MaxValue = 100;
			Value = 0;
			InitializeComponent();

			DataContext = this;
		}

		private void Spinner_OnSpin(object sender, SpinEventArgs e)
		{
			if (e.Direction == SpinDirection.Increase)
			{
				Value++;
				Value = Value > MaxValue ? MaxValue : Value;
			}
			else
			{
				Value--;
				Value = Value < MinValue ? MinValue : Value;
			}
		}
	}
}
