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
	public class volMAStratLiveLag : Strategy
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "volMAStratLiveLag";
				// Calculate									= Calculate.OnBarClose;
				Calculate									= Calculate.OnEachTick;
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

				Vol_UD_barsAgo = 0;
				tick_size = 2;
				stopLoss_tick_size = 18;
				profitTarget_tick_size = 23;
				valueMultiplier = 2.1;

				SMA_med = 50;

				AddPlot(Brushes.DarkViolet, "SMA_med");
			}
			else if (State == State.Configure)
			{
			}
		}

		// protected override void OnBarUpdate()
		protected override void OnMarketData(MarketDataEventArgs marketDataUpdate)
		{
			// if (BarsInProgress != 0 || CurrentBars[0] < 3 || !IsFirstTickOfBar) 
			if (BarsInProgress != 0 || CurrentBars[0] < 3) 
				return;

			double Vol_UD_Val = VolumeUpDown()[0];
			bool firstBar_isRed = Close[2] < Open[2];
			bool secondBar_isRed = Close[3] < Open[3];
			bool thirdBar_isGreen = Close[4] > Open[4];

			double first = VOL()[1];
			double second = VOL()[2];

			double thirdBarVOL_mult = VOL()[4] * valueMultiplier;

			double SMA_medVal = SMA(SMA_med)[0];

			// if previous two bars are each RED & at least twice the size of 3rd GREEN bar
			// if (Close[0] > Vol_UD_Val) 
			if ((thirdBar_isGreen && secondBar_isRed && firstBar_isRed) 
				&& (VOL()[2] > VOL()[4]*valueMultiplier)
				&& (VOL()[3] > VOL()[4]*valueMultiplier)
				&& Close[0] > SMA_medVal
				) 
				{
					// Print("The current Volume value is " + Vol_UD_Val.ToString());
					// Print("firstBar_isRed : " + first.ToString());
					// Print("secondBar_isRed : " + second.ToString());
					// Print("thirdBar_isGreen: ++" + VolumeUpDown()[3].ToString());
					// Print("thirdBar_isGreen:VOLVOL ++" + VOL()[3].ToString());
					// Print("---------------");

				EnterLong("Enter Long");
				SetStopLoss(CalculationMode.Ticks, stopLoss_tick_size);
        		SetProfitTarget(CalculationMode.Ticks, profitTarget_tick_size);
				}

		// 	// Sets SL/PT when limit order hits
		// 	protected override void OnOrderUpdate(Order order, double limitPrice, double stopPrice, int quantity, int filled, double averageFillPrice, OrderState orderState, DateTime time, ErrorCode error, string nativeError)
		// {
		// 	if (order.Name == "PSAR entry" && orderState != OrderState.Filled)
		// 	{
		// 		entryOrder = order;
		// 		SetStopLoss(CalculationMode.Ticks, stopLoss_tick_size);
		// 		SetProfitTarget(CalculationMode.Ticks, profitTarget_tick_size);

		// 	}

		// 	if (entryOrder != null && entryOrder == order)
		// 	{
		// 		if (order.OrderState == OrderState.Cancelled && order.Filled == 0)
		// 		{
		// 			entryOrder = null;
		// 		}
		// 		if (order.OrderState == OrderState.Filled)
		// 		{
		// 			entryOrder = null;
		// 		}
		// 	}
		// }



	}
		#region Properties
		[NinjaScriptProperty]
		[Range(0, int.MaxValue)]
		[Display(Name="Vol_UD_barsAgo", Order=1, GroupName="Parameters")]
		public int Vol_UD_barsAgo
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="tick_size", Order=2, GroupName="Parameters")]
		public double tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="stopLoss_tick_size", Order=3, GroupName="Parameters")]
		public double stopLoss_tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="profitTarget_tick_size", Order=4, GroupName="Parameters")]
		public double profitTarget_tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, double.MaxValue)]
		[Display(Name="valueMultiplier", Order=5, GroupName="Parameters")]
		public double valueMultiplier
		{ get; set; }

		// SMA indicator
		[NinjaScriptProperty]
		[Range(10, int.MaxValue)]
		[Display(Name="SMA_med", Order=6, GroupName="Parameters")]
		public int SMA_med
		{ get; set; }
		#endregion

	}
}
