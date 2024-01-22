Custom NinjaScript Files

The goal of these NinjaTrader strategies (trading algos) is to perform under tight risk management found in prop firm accounts. Strategies have files for backtesting and live environments. Use NT8's optimizer to search for the best params for payout.

For historical testing, go to Connections > Playback Connections > Historical, then click Enable

NinjaScript method values (e.g. CurrentBar, BarsRequiredToTrade) are based on Chart Strategy params when adding a strategy in NT8.

NinjaScript Library:
https://ninjatrader.com/support/helpGuides/nt8/NT%20HelpGuide%20English.html?language_reference_wip.htm

Historical testing works on a bar by bar basis, values will be calculated on bar-close and if the condition passes, order executes on the first tick of the next bar.
For Market Replay used with Calculate.OnEachTick, logic fires on each tick. Cannot run optimizations if using Market Replay data.
Historical provides lv 1 data by default. Not sure how to go about getting DOM data.

For tick by tick replay:
    Connections -> Simulated Data Feed
    Tools -> Historical Data -> Get Market Replay Data (only one day at a time)
    Connections -> (disconnect) Simulated Data Feed -> (connect) Playback

For bar by bar backtesting, set order method to OnBarUpdate()
For live env, set order methods to OnMarketData(MarketDataEventArgs marketDataUpdate), also change order execution methods if needed