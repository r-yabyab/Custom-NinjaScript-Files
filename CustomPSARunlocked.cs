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
				// Calculate									= Calculate.OnEachTick;
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
				BollingerBands_period = 10;

				tick_size = 6;
				stopLoss_tick_size = 7;
				profitTarget_tick_size = 7;


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
		// protected override void OnMarketData(MarketDataEventArgs marketDataUpdate)
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

				// matches MNQ tick sizes
				double psarValRound = Math.Round(psarVal / 0.25) * 0.25;


				// Values[1][0] = TrendLines(TrendLines_strength, TrendLines_numberOfTrendLines, TrendLines_oldTrendOpacity, TrendLines_alertOnBreak)[0];
		
				bool cross_above = CrossAbove(ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep), Bollinger(BollingerBands_numStdDev, BollingerBands_period).Lower, 1);
				bool cross_below = CrossBelow(ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep), Bollinger(BollingerBands_numStdDev, BollingerBands_period).Upper, 1);

				// if (Close[0] > ParabolicSAR(Psar_acceleration, Psar_accelerationMax, Psar_accelerationStep)[0] + 2*TickSize) {
				// if (Close[0] == psarValRound + tick_size*TickSize) {
				// if ((Close[0] < psarValRound + tick_size*TickSize) && (Close[0] > psarValRound + 1*TickSize)) {
				// if (Close[0] == psarValRound + tick_size*TickSize) {	// executes Long on next bar open if current bar has this
				if (Close[0] > bollingerUpper && Close[0] > psarVal) {
					// if ((Close[0] > psarVal + 1*TickSize)) {
						Print("===========Condition met. Entering Long at price: " + Close[0] + "=========================");
						Print("PSAR val offset: " + (psarValRound + tick_size*TickSize) + " //TICKOFFSET" + (tick_size*TickSize/.25) + " //TICKSIZE: " + TickSize);
					// EnterLong("Enter Long");
					EnterShort("Enter Short");

					SetStopLoss(CalculationMode.Ticks, stopLoss_tick_size);
        			SetProfitTarget(CalculationMode.Ticks, profitTarget_tick_size);
					// SetParabolicStop(CalculationMode.Ticks, stopLoss_tick_size);
					// }

					// SetTrailStop(CalculationMode.Ticks, 4);

				}

				

				// if (cross_above) {
				// 	EnterLong();

				// 	SetStopLoss(CalculationMode.Ticks, 4);
        		// 	SetProfitTarget(CalculationMode.Ticks, 4);
					
				// 	// SetTrailStop(CalculationMode.Ticks, 3);

				// } 
				// if (cross_below) {
				// 	EnterShort();

				// 	SetStopLoss(CalculationMode.Ticks, 4);
        		// 	SetProfitTarget(CalculationMode.Ticks, 4);
					
				// 	// SetTrailStop(CalculationMode.Ticks, 6);
				// }
		
		}

		protected override void OnExecutionUpdate(Execution execution, string executionId, double price, int quantity, MarketPosition marketPosition, string orderId, DateTime time)
{
	if (execution.Order.Name == "Enter Long") {
		Print("@#$@)#($#)@($@#)()@$(@)# ENTER LONG @ " + price);
	}
	if (execution.Order.Name == "Enter Short") {
		Print("@#$@)#($#)@($@#)()@$(@)# ENTER SHORT @ " + price);
	}
	if (execution.Order.Name == "Profit target")
	{
		// the profit target has had a fill or part fill
		Print("=========PROFIT HIT @" + price);
	}

	if (execution.Order.Name == "Stop loss")
	{
		// the stop loss has had a fill or part fill
		Print("=========STOPLOSS HIT @" + price);

	}

	if (execution.Order.Name == "Trail stop")
	{
		Print("=======TRAILSTOP HIT @" + price);
		// the trailing stop loss has had a fill or part fill
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
		[Range(0, double.MaxValue)] // default 2
		[Display(Name="BollingerBands_numStdDev", Order=8, GroupName="Parameters")]
		public double BollingerBands_numStdDev
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]	// default 14
		[Display(Name="BollingerBands_period", Order=9, GroupName="Parameters")]
		public int BollingerBands_period
		{ get; set; }

		[NinjaScriptProperty]
		[Range(2, double.MaxValue)]
		[Display(Name="tick_size", Order=10, GroupName="Parameters")]
		public double tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(4, double.MaxValue)]
		[Display(Name="stopLoss_tick_size", Order=11, GroupName="Parameters")]
		public double stopLoss_tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(6, double.MaxValue)]
		[Display(Name="profitTarget_tick_size", Order=12, GroupName="Parameters")]
		public double profitTarget_tick_size
		{ get; set; }
		#endregion

	}
}
