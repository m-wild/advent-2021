create table input_1 (i integer not null);

.import 1.input.txt input_1

-- part 1
with cte as (
	select i, lag(i, 1) over (order by ROWID) as prev 
	from input_1
)
select sum( case when prev < i then 1 else 0 end )
from cte;

-- part 2
with cte1 as ( 
	select ROWID, i, sum(i) over (order by ROWID rows between current row and 2 following) as wnd 
	from input_1
)
, cte2 as (
	select wnd, lag(wnd, 1) over (order by ROWID) as prev_wnd
	from cte1
)
select sum( case when prev_wnd < wnd then 1 else 0 end )
from cte2;