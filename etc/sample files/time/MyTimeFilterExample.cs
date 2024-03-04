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

namespace NinjaTrader.NinjaScript.Strategies
{
    public class MyTimeFilterExample : MyStrategyBase
    {
        #region Globals

        private const string StrategyName = "MyTimeFilterExample";

        #endregion

        /* --------------------------------------------------------------------------------------------------- */

        #region NinjaScript Framework Methods

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description                                 = "Example time filtering using methods from base class";
                Name                                        = StrategyName;
                Calculate                                   = Calculate.OnBarClose;
                EntriesPerDirection                         = 1;
                EntryHandling                               = EntryHandling.AllEntries;
                IsExitOnSessionCloseStrategy                = true;
                ExitOnSessionCloseSeconds                   = 30;
                IsFillLimitOnTouch                          = false;
                MaximumBarsLookBack                         = MaximumBarsLookBack.TwoHundredFiftySix;
                OrderFillResolution                         = OrderFillResolution.Standard;
                Slippage                                    = 0;
                StartBehavior                               = StartBehavior.WaitUntilFlat;
                TimeInForce                                 = TimeInForce.Gtc;
                TraceOrders                                 = false;
                RealtimeErrorHandling                       = RealtimeErrorHandling.StopCancelClose;
                StopTargetHandling                          = StopTargetHandling.PerEntryExecution;
                BarsRequiredToTrade                         = 20;
                // Disable this property for performance gains in Strategy Analyzer optimizations
                // See the Help Guide for additional information
                IsInstantiatedOnEachOptimizationIteration   = true;
                OnStateChange_SetDefaults();
            }
            else if (State == State.Configure)
            {
                OnStateChange_Configure();
            }
        }

        /* --------------------------------------------------------------------------------------------------- */

        public override string DisplayName
        {
            get { return StrategyName; }
        }

        /* --------------------------------------------------------------------------------------------------- */

        private void OnStateChange_SetDefaults()
        {
            ProcessHours_SetDefaults();
        }

        /* --------------------------------------------------------------------------------------------------- */

        private void OnStateChange_Configure()
        {
            ProcessHours_Configure();
        }

        /* --------------------------------------------------------------------------------------------------- */

        protected override void OnBarUpdate()
        {
            if (BarsInProgress != 0) 
                return;

            if (CurrentBars[0] < 0)
                return;

            if (ProcessHours_IsGoodTime())
                BackBrush = Brushes.LightSkyBlue;
        }

        #endregion
    }
}
