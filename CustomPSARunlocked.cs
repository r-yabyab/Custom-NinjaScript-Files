#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
	public class CustomPSARunlocked : Strategy
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "CustomPSARunlocked";
				Calculate									= Calculate.OnBarClose;
				EntriesPerDirection							= 1;
				EntryHandling								= EntryHandling.AllEntries;
				IsExitOnSessionCloseStrategy				= true;
				ExitOnSessionCloseSeconds					= 30;
				IsFillLimitOnTouch							= false;
				MaximumBarsLookBack							= MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution							= OrderFillResolution.Standard;
				Slippage									= 0;
				StartBehavior								= StartBehavior.WaitUntilFlat;
				TimeInForce									= TimeInForce.Gtc;
				TraceOrders									= false;
				RealtimeErrorHandling						= RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling							= StopTargetHandling.PerEntryExecution;
				BarsRequiredToTrade							= 20;
				// Disable this property for performance gains in Strategy Analyzer optimizations
				// See the Help Guide for additional information
				IsInstantiatedOnEachOptimizationIteration	= true;
				Psar_acceleration					= 0.02;
				Psar_accelerationMax					= 0.2;
				Psar_accelerationStep					= 0.02;

				AddPlot(new Stroke(Brushes.Red), PlotStyle.Dot, "ParabolicSAR");
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			double psarVal = ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep)[0];
				Values[0][0] = ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep)[0];

//			Draw.Dot(this, "ParabolicSAR", true, 0, psarVal, Brushes.Blue);
			Draw.Dot(this, "ParabolicSARDot", false, 0, psarVal, Brushes.Red);
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(0.02, double.MaxValue)]
		[Display(Name="Psar_acceleration", Order=1, GroupName="Parameters")]
		public double Psar_acceleration
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0.2, double.MaxValue)]
		[Display(Name="Psar_accelerationMax", Order=2, GroupName="Parameters")]
		public double Psar_accelerationMax
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0.02, double.MaxValue)]
		[Display(Name="Psar_accelerationStep", Order=3, GroupName="Parameters")]
		public double Psar_accelerationStep
		{ get; set; }
		#endregion

	}
}
