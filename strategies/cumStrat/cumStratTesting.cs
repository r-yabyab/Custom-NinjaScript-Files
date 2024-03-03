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
	public class volMAStratTestingSHORT : Strategy
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "volMAStratTestingSHORT";
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

				tick_size = 2;
				stopLoss_tick_size = 10;
				profitTarget_tick_size = 20;

				SMA_med = 50;

				AddPlot(Brushes.DarkViolet, "SMA_med");


			}
			else if (State == State.Configure)
			{
                // // A 1 tick data series must be added to the OnStateChange() as this indicator runs off of tick data
                // AddDataSeries(Data.BarsPeriodType.Tick, 1);

			}
		}

		// market order lags behind 1 bar if not tick replay
		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0 || CurrentBars[0] < 3) 
				return;

            // positive is green, neg red
            double firstCum = CumDelta.DeltaOpen[0] - CumDelta.DeltaClose[0];
            double secondCum = CumDelta.DeltaOpen[1] - CumDelta.DeltaClose[1];
            double thirdCum = CumDelta.DeltaOpen[2] - CumDelta.DeltaClose[2];            
            // double fourthCum = CumDelta.DeltaOpen[3] - CumDelta.DeltaClose[3];            

			double SMA_medVal = SMA(SMA_med)[0];


            // if previous two bars are each RED & at least twice the size of 3rd GREEN bar
            // if (Close[0] > Vol_UD_Val) 
            if ((firstCum > 0) && (secondCum > 0) && (thirdCum > 0))
            {
                Print("Delta Close: " + CumDelta.DeltaClose[0]);
                Print("---------------");

                EnterShort("Enter Long");
                SetStopLoss(CalculationMode.Ticks, stopLoss_tick_size);
                SetProfitTarget(CalculationMode.Ticks, profitTarget_tick_size);
            }

            // // Probably don't need this????
            // else if (BarsInProgress == 1)
            // {
            //     // We have to update the secondary series of the cached indicator to make sure the values we get in BarsInProgress == 0 are in sync
            //     OrderFlowCumulativeDelta(BarsArray[0], CumulativeDeltaType.BidAsk, CumulativeDeltaPeriod.Session, 0).Update(OrderFlowCumulativeDelta(BarsArray[0], CumulativeDeltaType.BidAsk, CumulativeDeltaPeriod.Session, 0).BarsArray[1].Count - 1, 1);
            // }


        }
        #region Properties
        [NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="tick_size", Order=1, GroupName="Parameters")]
		public double tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="stopLoss_tick_size", Order=2, GroupName="Parameters")]
		public double stopLoss_tick_size
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="profitTarget_tick_size", Order=3, GroupName="Parameters")]
		public double profitTarget_tick_size
		{ get; set; }

		// SMA indicator
		[NinjaScriptProperty]
		[Range(10, int.MaxValue)]
		[Display(Name="SMA_med", Order=4, GroupName="Parameters")]
		public int SMA_med
		{ get; set; }
		#endregion

	}
}
