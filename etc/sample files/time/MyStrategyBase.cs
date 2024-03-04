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
    abstract public class MyStrategyBase : Strategy
    {
        #region Misc Helpers - Processing Hours

        #region Globals

        private int                     nTradeBeginTime                 = 0;
        private int                     nTradeEndTime                   = 0;

        private int                     nBreakBeginTime                 = 0;
        private int                     nBreakEndTime                   = 0;

        #endregion

        /* --------------------------------------------------------------------------------------------------- */

        #region Properties

        [NinjaScriptProperty]
        [Range(0, 2359)]
        [Display(Name="1. Trade BeginTime", Order=1, GroupName="Processing Hours")]
        public int TradeBeginTime
        { get; set; }

        [NinjaScriptProperty]
        [Range(0, 2359)]
        [Display(Name="2. Trade EndTime", Order=2, GroupName="Processing Hours")]
        public int TradeEndTime
        { get; set; }

        [NinjaScriptProperty]
        [Range(0, 2359)]
        [Display(Name="3. Break BeginTime", Order=3, GroupName="Processing Hours")]
        public int BreakBeginTime
        { get; set; }

        [NinjaScriptProperty]
        [Range(0, 2359)]
        [Display(Name="4. Break EndTime", Order=4, GroupName="Processing Hours")]
        public int BreakEndTime
        { get; set; }

        #endregion

        /* --------------------------------------------------------------------------------------------------- */

        #region Methods

        // called during State.SetDefaults to initialize parameters
        protected virtual void ProcessHours_SetDefaults()
        {
            TradeBeginTime = 0;
            BreakBeginTime = 0;
            TradeEndTime = 0;
            BreakEndTime = 0;
        }

        /* --------------------------------------------------------------------------------------------------- */

        // called on or after State.Configure to initialize global variables
        protected virtual void ProcessHours_Configure()
        {
            nTradeBeginTime = TradeBeginTime * 100;
            nBreakBeginTime = BreakBeginTime * 100;

            nTradeEndTime = TradeEndTime * 100;
            nBreakEndTime = BreakEndTime * 100;
        }

        /* --------------------------------------------------------------------------------------------------- */

        // confirm processing time restrictions
        protected virtual bool ProcessHours_IsGoodTime()
        {
            bool status = true;

            if (nTradeBeginTime < nTradeEndTime)
                status = ToTime(Time[0]) >= nTradeBeginTime && ToTime(Time[0]) < nTradeEndTime;

            if (nTradeBeginTime > nTradeEndTime)
                status = ToTime(Time[0]) >= nTradeBeginTime || ToTime(Time[0]) < nTradeEndTime;

            if (status && nBreakBeginTime < nBreakEndTime)
                status = !(ToTime(Time[0]) >= nBreakBeginTime && ToTime(Time[0]) < nBreakEndTime);

            if (status && nBreakBeginTime > nBreakEndTime)
                status = !(ToTime(Time[0]) >= nBreakBeginTime || ToTime(Time[0]) < nBreakEndTime);

            return status;
        }

        #endregion

        #endregion

        /* --------------------------------------------------------------------------------------------------- */

        #region Misc Helpers - Utils

        protected virtual void PrintString(string format, params object[] args)
        {
            Print(string.Format(format, args));
            /* NinjaTrader.Code.Output.Process(string message, PrintTo outputTab); */
        }

        #endregion
    }
}
