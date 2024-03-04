// 
// Copyright (C) 2015, NinjaTrader LLC <www.ninjatrader.com>.
// NinjaTrader reserves the right to modify or overwrite this NinjaScript component with each release.
//
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
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
	public class SampleTimeFilter : Strategy
	{ 
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description					 = @"Sample strategy using time filter";
				Name						 = "Sample time filter";
				Calculate					 = Calculate.OnBarClose;
				BarsRequiredToTrade			 = 20;
				EntriesPerDirection			 = 1;
				EntryHandling				 = EntryHandling.AllEntries;
				IsExitOnSessionCloseStrategy = true;
          		ExitOnSessionCloseSeconds    = 30;
				MaximumBarsLookBack			 = MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution			 = OrderFillResolution.Standard;
				Slippage					 = 0;
				StartBehavior				 = StartBehavior.WaitUntilFlat;
				TimeInForce					 = TimeInForce.Gtc;
				RealtimeErrorHandling		 = RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling			 = StopTargetHandling.PerEntryExecution;
			}
		}

		protected override void OnBarUpdate()
		{
			if (Time[0].DayOfWeek != DayOfWeek.Monday && Time[0].DayOfWeek != DayOfWeek.Friday)
            {
                /* Checks to see if the time is during the busier hours (format is HHMMSS or HMMSS). Only allow trading if current time is during a busy period.
                The timezone used here is (GMT-05:00) EST. */
                if ((ToTime(Time[0]) >= 93000 && ToTime(Time[0]) < 120000) || (ToTime(Time[0]) >= 140000 && ToTime(Time[0]) < 154500))
                {
                    // Entry Signal: If current close greater than previous close, enter long
                    if (Close[0] > Close[1])
                        EnterLong();
                }
            }
             
            // Exit Signal: If position was established at least 5 bars ago, exit long
            if (BarsSinceEntryExecution() > 5)
                ExitLong();
		}
	}
}
