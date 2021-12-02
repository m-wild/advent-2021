create table input_2 (i text not null);
.import 2.input.txt input_2

-- part 1
with cte1 as (
    select 
        substr(i, 1, instr(i,' ') - 1) as actn,
        cast(substr(i, instr(i, ' ') + 1) as integer) as abs_amnt
    from input_2
    order by ROWID
)
, cte2 as (
    select
        case when actn in ('up', 'down') then 'depth' else 'horz' end as dir,
        case when actn = 'up' then -1 * abs_amnt else abs_amnt end as amnt -- 'up' means reduce depth
    from cte1
)
, cte3 as (
    select
        dir,
        sum(amnt) as total_amnt
    from cte2
    group by dir
)
select 
    depth.total_amnt * horz.total_amnt as final_pos
from cte3 depth, 
     cte3 horz
where depth.dir = 'depth'
and horz.dir = 'horz';

-- 1580000


-- part 2
with cte1 as (
    select
        ROWID,
        substr(i, 1, instr(i,' ') - 1) as actn,
        cast(substr(i, instr(i, ' ') + 1) as integer) as abs_amnt
    from input_2
    order by ROWID
)
, cte2 as (
    select
        ROWID,
        case when actn in ('up', 'down') then 'aim' else 'horz' end as dir,
        case when actn = 'up' then -1 * abs_amnt else abs_amnt end as amnt -- 'up' means reduce depth
    from cte1
    order by ROWID
)
, cte3 as (
    select
        *,
        sum(case when dir = 'aim' then amnt else 0 end) over (rows unbounded preceding) as current_aim
    from cte2
    order by ROWID
)
, cte4 as (
    select 
        *,
        -- increases your horizontal position by X units.
        sum(case when dir = 'horz' then amnt else 0 end) over (rows unbounded preceding) as current_horz,
        
        -- increases your depth by your aim multiplied by X.
        sum(case when dir = 'horz' then current_aim * amnt else 0 end) over (rows unbounded preceding) as current_depth
    from cte3
    order by ROWID
)
select
    current_horz * current_depth as final_pos
from cte4
order by ROWID desc
limit 1;

