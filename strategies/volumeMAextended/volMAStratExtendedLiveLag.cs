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
	public class volMAStratExtendedLiveLag : Strategy
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "volMAStratExtendedLiveLag";
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
				profitTarget_tick_size = 19;
				valueMultiplier = 2.1;

				SMA_med = 50;

				vol_lookBack = 7;

				AddPlot(Brushes.DarkViolet, "SMA_med");
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
		// protected override void OnMarketData(MarketDataEventArgs marketDataUpdate)
		{
			// if (BarsInProgress != 0 || CurrentBars[0] < 3 || !IsFirstTickOfBar) 
			if (BarsInProgress != 0 || CurrentBars[0] < 11)  {
				Print("Waiting for more than 11 bars");
				return;
			}

			if (IsFirstTickOfBar && ((ToTime(Time[0]) >= 150000 && ToTime(Time[0]) < 245959) || (ToTime(Time[0]) >= 10000 && ToTime(Time[0]) < 140000)))
			{
			double Vol_UD_Val = VolumeUpDown()[0];
			bool secondBar_isRed = Close[2] < Open[2];
			bool thirdBar_isRed = Close[3] < Open[3];
			bool fourthBar_isGreen = Close[4] >= Open[4];

			double secondRED = VOL()[2];
			double thirdRED = VOL()[3];
			double fourthGREEN = VOL()[4];

			double fourthBarVOL_mult = VOL()[4] * valueMultiplier;

			double SMA_medVal = SMA(SMA_med)[0];
			Values[0][0] = SMA_medVal;

			double fifthBar = VOL()[5];
			double sixthBar = VOL()[6];
			double seventhBar = VOL()[7];
			double eighthBar = VOL()[8];
			double ninthBar = VOL()[9];
			double tenthBar = VOL()[10];
			double eleventhBar = VOL()[11];


			double[] values = {fifthBar, sixthBar, seventhBar, eighthBar, ninthBar, tenthBar, eleventhBar};
			double maxValue = values[0];

			for (int i = 0; i < vol_lookBack; i++) {
				maxValue = Math.Max(maxValue, values[i]);
			}


			// if 2nd and 3rd bar back is RED & 4th bar is green, if each 2nd and 3rd bar's vol times valueMultiplier's value are more than 4th bar's vol & is above SMA_medVal
			if ((fourthBar_isGreen && thirdBar_isRed && secondBar_isRed) 
				&& (VOL()[2] > VOL()[4]*valueMultiplier)
				&& (VOL()[3] > VOL()[4]*valueMultiplier)
				&& (Close[0] >= SMA_medVal)
				&& (VOL()[2] > maxValue || VOL()[3] > maxValue)
				
				) 
				{

					Print("The current Volume value is " + Vol_UD_Val.ToString());
					Print("2ndbarRED : " + secondRED.ToString());
					Print("3rdbarRED : " + thirdRED.ToString());
					Print("4thBarGREEN: ++" + fourthGREEN.ToString());
					Print("ValMultiplier: " + (VOL()[4]*valueMultiplier).ToString());
					Print("---------------");

				EnterLong("Enter Long");
				SetStopLoss(CalculationMode.Ticks, stopLoss_tick_size);
        		SetProfitTarget(CalculationMode.Ticks, profitTarget_tick_size);
				} 
			}


				// else if ((fourthBar_isGreen && thirdBar_isRed && secondBar_isRed) && Close[0] > SMA_medVal) {
				// 	Print("2nd & 3rd RED, 4th Green & Close / SMA" + Close[0].ToString() + " / " + SMA_medVal.ToString() );
				// }

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

		[NinjaScriptProperty]
		[Range(0, int.MaxValue)]
		[Display(Name="vol_lookBack", Order=7, GroupName="Parameters")]
		public int vol_lookBack
		{ get; set; }
		#endregion

	}
}
