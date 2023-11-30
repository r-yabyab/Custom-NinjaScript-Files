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
        // private double upperChannelOffset = 0.1;  // Adjust this value based on your requirements
        // private double supportOffset = 0.1;       // Adjust this value based on your requirements

        private double psar;
        // private double upperChannel;
        // private double support;

        private double trend;

        private Indicator TrendLines;

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
                // PSARPARAMS = [0.02, 0.2, 0.02]
                parabolicSar = ParabolicSAR(0.02, 0.2, 0.02);
            }
            else if (State == State.DataLoaded) {
                // upperChannel = Highs[1][0] + upperChannelOffset;
                // lowerChannel = Lows[1][0] - supportOffset;
            

                // upperChannel.Plots[0].Brush = Brushes.Goldenrod;
                // lowerChannel.Plots[0].Brush = Brushes.SeaGreen;

                // AddChartIndicator(upperChannel);
                // AddChartIndicator(lowerChannel);

                AddPlot(Brushes.Green, "parabolicSAR"); // stored as Plots[0] and Values[0]
                // AddPlot(Brushes.Blue, "lowerChannel"); // stored as Plots[1] and Values[1]

                // Values[0][0] = ParabolicSAR(parabolicSar)[0];
                Values[0][0] = ParabolicSAR(0.02, 0.2, 0.02)[0];

                // Values[1][0] = trend(lowerChannel)[0];

                // TrendLines(int strength, int numberOfTrendLines, int oldTrendsOpacity, bool alertOnBreak)
                // return val double

//                trend = TrendLines(5, 1, 3, false);
                AddPlot(Brushes.Blue, "trendLines");
                Values[1][0] = TrendLines(5, 1, 3, false)[0];

            }
        }

        protected override void OnBarUpdate()
        {

            if (CurrentBar < BarsRequiredToTrade) {
                return;
            }

                // psar = parabolicSar[0];

               // bool for above and below to trigger trades

            // if (IsRising(trend)) {
                EnterLong();
            // }
        }
    }
}