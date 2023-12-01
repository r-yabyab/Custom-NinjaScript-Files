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
				
				TrendLines_strength = 10;
				TrendLines_numberOfTrendLines = 3;
				TrendLines_oldTrendOpacity = 25;
				TrendLines_alertOnBreak = false;

				BollingerBands_numStdDev = 2;
				BollingerBands_period = 14;


				AddPlot(new Stroke(Brushes.LimeGreen), PlotStyle.Dot, "ParabolicSAR");
				// AddPlot(Brushes.Blue, "TrendLines"); // all trend lines connect, needs to be fixed
				AddPlot(Brushes.Blue, "BollingerBandsUpper");
				AddPlot(Brushes.Red, "BollingerBandsLower");
				AddPlot(Brushes.MintCream, "BollingerBandsZero");
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
				double bollingerUpper = Bollinger(BollingerBands_numStdDev, BollingerBands_period).Upper[0];
				double bollingerLower = Bollinger(BollingerBands_numStdDev, BollingerBands_period).Lower[0];
				double bollingerZero = Bollinger(0, BollingerBands_period)[0];
				Values[0][0] = psarVal;
				Values[1][0] = bollingerUpper;
				Values[2][0] = bollingerLower;
				Values[3][0] = bollingerZero;

				// Values[1][0] = TrendLines(TrendLines_strength, TrendLines_numberOfTrendLines, TrendLines_oldTrendOpacity, TrendLines_alertOnBreak)[0];
		
				bool cross_above = CrossAbove(ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep), Bollinger(BollingerBands_numStdDev, BollingerBands_period).Lower, 1);
				bool cross_below = CrossBelow(ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep), Bollinger(BollingerBands_numStdDev, BollingerBands_period).Upper, 1);

				if (cross_above) {
					EnterLong();
				} else if (cross_below) {
					EnterShort();
				}
		
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

		[NinjaScriptProperty]
		[Range(10, int.MaxValue)]
		[Display(Name="TrendLines_strength", Order=4, GroupName="Parameters")]
		public int TrendLines_strength
		{ get; set; }

		[NinjaScriptProperty]
		[Range(3, int.MaxValue)]
		[Display(Name="TrendLines_numberOfTrendLines", Order=5, GroupName="Parameters")]
		public int TrendLines_numberOfTrendLines
		{ get; set; }

		[NinjaScriptProperty]
		[Range(25, int.MaxValue)]
		[Display(Name="TrendLines_oldTrendOpacity", Order=6, GroupName="Parameters")]
		public int TrendLines_oldTrendOpacity
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="TrendLines_alertOnBreak", Order=7, GroupName="Parameters")]
		public bool TrendLines_alertOnBreak
		{ get; set; }

		[NinjaScriptProperty]
		[Range(2, double.MaxValue)]
		[Display(Name="BollingerBands_numStdDev", Order=8, GroupName="Parameters")]
		public double BollingerBands_numStdDev
		{ get; set; }

		[NinjaScriptProperty]
		[Range(14, int.MaxValue)]
		[Display(Name="BollingerBands_period", Order=9, GroupName="Parameters")]
		public int BollingerBands_period
		{ get; set; }
		#endregion

	}
}
