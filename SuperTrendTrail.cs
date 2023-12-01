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
	public class SuperTrendTrail : Strategy
	{
		private double SuperTrendLong;
		private double SuperTrendShort;

		private TSSuperTrend TSSuperTrend1;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "SuperTrendTrail";
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
				PositionSize					= 1;
				LongTarget					= 1000;
				ShortTarget					= -1000;
				SuperTrendLong					= 1;
				SuperTrendShort					= 1;
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{				
				TSSuperTrend1				= TSSuperTrend(Close, SuperTrendMode.ATR, 14, 2.618, MovingAverageType.HMA, 14, false, false, false);
				TSSuperTrend1.Plots[0].Brush = Brushes.Green;
				TSSuperTrend1.Plots[1].Brush = Brushes.Red;
				AddChartIndicator(TSSuperTrend1);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			if (CurrentBars[0] < 1)
				return;

			 // Set 1
			if ((Close[0] >= TSSuperTrend1.UpTrend[0])
				 && (TSSuperTrend1.UpTrend[0] != 0)
				 && (TSSuperTrend1.DownTrend[0] == 0)
				 && (TSSuperTrend1.DownTrend[1] != 0))
			{
				EnterLong(Convert.ToInt32(PositionSize), @"EntryLong");
				SuperTrendLong = TSSuperTrend1.UpTrend[0];
				ExitLongStopMarket(Convert.ToInt32(Position.Quantity), SuperTrendLong, @"StopLong", @"EntryLong");
				ExitLongLimit(Convert.ToInt32(Position.AveragePrice), (Position.AveragePrice + (LongTarget * TickSize)) , @"TargetLong", @"EntryLong");
			}
			
			 // Set 2
			if ((Position.MarketPosition == MarketPosition.Long)
				 && (GetCurrentAsk(0) > TSSuperTrend1.UpTrend[0])
				 && (GetCurrentBid(0) > TSSuperTrend1.UpTrend[0])
				 && (TSSuperTrend1.UpTrend[0] > SuperTrendLong))
			{
				SuperTrendLong = TSSuperTrend1.UpTrend[0];
			}
			
			 // Set 3
			if (Position.MarketPosition == MarketPosition.Long)
			{
				ExitLongStopMarket(Convert.ToInt32(Position.Quantity), SuperTrendLong, @"StopLong", @"EntryLong");
				ExitLongLimit(Convert.ToInt32(DefaultQuantity), (Position.AveragePrice + (LongTarget * TickSize)) , @"TargetLong", @"EntryLong");
			}
			
			 // Set 4
			if ((Close[0] <= TSSuperTrend1.DownTrend[0])
				 && (TSSuperTrend1.DownTrend[0] != 0)
				 && (TSSuperTrend1.UpTrend[0] == 0)
				 && (TSSuperTrend1.UpTrend[1] != 0))
			{
				EnterShort(Convert.ToInt32(PositionSize), @"EntryShort");
				SuperTrendShort = TSSuperTrend1.DownTrend[0];
				ExitShortStopMarket(Convert.ToInt32(Position.Quantity), SuperTrendShort, @"StopShort", @"EntryShort");
				ExitShortLimit(Convert.ToInt32(Position.Quantity), (Position.AveragePrice + (ShortTarget * TickSize)) , @"TargetShort", @"EntryShort");
			}
			
			 // Set 5
			if ((Position.MarketPosition == MarketPosition.Short)
				 && (GetCurrentAsk(0) < TSSuperTrend1.DownTrend[0])
				 && (GetCurrentBid(0) < TSSuperTrend1.DownTrend[0])
				 && (TSSuperTrend1.DownTrend[0] < SuperTrendShort))
			{
				SuperTrendShort = TSSuperTrend1.DownTrend[0];
			}
			
			 // Set 6
			if (Position.MarketPosition == MarketPosition.Short)
			{
				ExitShortStopMarket(Convert.ToInt32(Position.Quantity), SuperTrendShort, @"StopShort", @"EntryShort");
				ExitShortLimit(Convert.ToInt32(Position.Quantity), (Position.AveragePrice + (ShortTarget * TickSize)) , @"TargetShort", @"EntryShort");
			}
			
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="PositionSize", Order=1, GroupName="Parameters")]
		public int PositionSize
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="LongTarget", Order=2, GroupName="Parameters")]
		public int LongTarget
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="ShortTarget", Order=3, GroupName="Parameters")]
		public int ShortTarget
		{ get; set; }
		#endregion

	}
}