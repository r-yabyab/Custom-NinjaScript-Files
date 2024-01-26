## General Information
Longs at price reversals by detecting short term increases in volume.
<br />Works mostly for ES in 1m, 5m.
<br />Backtesting gives good PF, but the strategy won't work on some months.

### Problems
More than half the time, orders will not execute if logic is met in live env.
<br/>Backtesting lags market order by one bar unless tick replay is on.