Custom NinjaScript Files

The goal of these NinjaTrader strategies (trading algos) is to perform under tight risk management found in prop firm accounts.

The main challenge of creating these scripts is optimizing for live data. The tools that NinjaTrader has only works on bar update, not tick update, which makes it hard to test for sudden price movements indicated by long candle wicks.

For historical testing, go to Connections > Playback Connections > Historical, then click Enable

NinjaScript method values (e.g. CurrentBar, BarsRequiredToTrade) are based on Chart Strategy params when adding a strategy in NT8.

NinjaScript Library:
https://ninjatrader.com/support/helpGuides/nt8/NT%20HelpGuide%20English.html?language_reference_wip.htm

Historical testing works on a bar by bar basis, values will be calculated on bar-close and if the condition passes, order executes on the first tick of the next bar.
For Market Replay used with Calculate.OnEachTick, logic fires on each tick. Cannot run optimizations if using Market Replay data.
Historical provides lv 1 data by default. Not sure how to go about getting DOM data.

Try for psar 30 sec after 5-6pm PST


Ideas: confirm if bounce once off psar 4 ticks above then after bar close set limit for 4 ticks down