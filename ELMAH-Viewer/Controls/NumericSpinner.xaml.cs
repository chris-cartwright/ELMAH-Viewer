﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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

		static NumericSpinner()
		{
			MinValueProperty = DependencyProperty.RegisterAttached("MinValue", typeof(long), typeof(NumericSpinner),
				new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });

			MaxValueProperty = DependencyProperty.RegisterAttached("MaxValue", typeof(long), typeof(NumericSpinner),
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

		public NumericSpinner()
		{
			MinValue = 0;
			MaxValue = 100;
			InitializeComponent();
		}

		private void Spinner_OnSpin(object sender, SpinEventArgs e)
		{
			ButtonSpinner spinner = (ButtonSpinner)sender;
			TextBlock textBlock = (TextBlock)spinner.Content;

			long value = String.IsNullOrEmpty(textBlock.Text) ? 0 : Convert.ToInt64(textBlock.Text);

			if (e.Direction == SpinDirection.Increase)
			{
				value++;
				value = value > MaxValue ? MaxValue : value;
			}
			else
			{
				value--;
				value = value < MinValue ? MinValue : value;
			}

			textBlock.Text = value.ToString(CultureInfo.InvariantCulture);
		}
	}
}