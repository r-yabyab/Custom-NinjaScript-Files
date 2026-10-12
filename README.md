## NT8 Strategies
Trading strategies for NinjaTrader8 built using the [NinjaScript Library](https://ninjatrader.com/support/helpGuides/nt8/NT%20HelpGuide%20English.html?language_reference_wip.htm) [new](https://developer.ninjatrader.com/docs/desktop). Each strategy comes with a backtesting and live environment script with plotted indicators. Use with NT8's strategy optimizer to find the best profit factor.
<br/><br/>Trades stop from 2PM-3PM (local time) to mitigate overnight margin. I use rithmic through ApexTraderFunding which goes down at 3PM, so be aware of your brokerage hours as well.

![Screenshot](https://i.imgur.com/NgnvRUU.png)

## Backtesting through Strategy Analyzer
Backtesting works through historical price movements in NT8's Strategy Analyzer in 2 ways: Bar replay and Tick replay. 
- Bar replay is the quickest way to get started, however, it will do a 50% toss up for price movements on large bars, so expect more accurate results with lower timeframes (e.g. 1m, 5m).
- Tick replay provides more accurate results, but the strategy analyzer is slow and will occasionally crash the NT8 in the optimizer.

## Backtesting through Charts
Can be done in *Connections > Replay* using either  History or Market Replay.
* History: Same as backtesting through the strategy analyzer, but order executions show on the chart, along with plotted indicators.
* Market Replay: Same as history, but ticks will be loaded with the option to speed up time.

### Problems with backtesting
+ Testing with limit orders not available.
+ Sometimes the strategies will place orders at the start of the next bar, even on Market Replay. I handled this by mimicking this behavior with live accounts.
+ Backtesting data is sometimes incomplete if not using a live account to test.
+ Usually done with lv 1 data. Need to pay for lv 2 data (e.g. bid/ask, order flow).

### Problems with live accounts
+ If sending multiple orders on less liquid contracts like CL, some orders may not get filled. Use limit orders to handle this.
+ When dealing with overnight margin, exchanges from 

## Live Environment
Use live scripts, can be changed to place either market or limit orders.
<br/> If you need to fill orders manually, go to chart trader then ATM strategy to set PT/SL.
