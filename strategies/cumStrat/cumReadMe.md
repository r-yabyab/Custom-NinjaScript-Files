cum methods

		public Series<double> DeltaOpen

		public Series<double> DeltaHigh

		public Series<double> DeltaLow

		public Series<double> DeltaClose
		
https://ninjatrader.com/support/helpGuides/nt8//NT%20HelpGuide%20English.html?order_flow_cumulative_delta2.htm
- based on NT8's cum delta, but same methods but only BidAsk type.

How it works:
CumDelta val is always DeltaClose
Price is triggered two bar after confirming

![alt text](../../etc/img/cumstrat.png)