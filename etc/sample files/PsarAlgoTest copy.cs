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

// This namespace holds Strategies in this folder and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Strategies
{
    public class PsarAlgoTest : Strategy
    {
        private double upperChannelOffset = 0.1;  // Adjust this value based on your requirements
        private double supportOffset = 0.1;       // Adjust this value based on your requirements

        private double psar;
        private double upperChannel;
        private double support;

        private Indicator parabolicSar;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                // ... (unchanged)
            }
            else if (State == State.Configure)
            {
                AddDataSeries(Data.BarsPeriodType.Tick, 1); // Assuming tick data for MNQ
                parabolicSar = ParabolicSAR(0.02, 0.2, 0.02);
            }
        }

        protected override void OnBarUpdate()
        {
            if (BarsInProgress == 1 && CurrentBars[0] >= 3)
            {
                psar = parabolicSar[0];
                upperChannel = Highs[1][0] + upperChannelOffset;
                support = Lows[1][0] - supportOffset;

                if (Position.MarketPosition == MarketPosition.Flat)
                {
                    if (Close[1] >= upperChannel && Close[0] <= support)
                    {
                        EnterLong("BuySignal");
                    }
                }
                else if (Position.MarketPosition == MarketPosition.Long)
                {
                    if (Close[0] >= upperChannel)
                    {
                        ExitLong("SellSignal");
                    }
                }
            }
        }
    }
}