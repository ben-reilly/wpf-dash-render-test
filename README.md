# WPF Dash Render Test

It appears that WPF cannot render dashes smaller then **0.05** pixels before scaling, even if they are later scaled up beyond that limit.

This project demonstrates the bug by drawing groups of lines at different scales.

* Each line is the same length after scaling.
* Lines are first grouped by the value given to the `DashStyle`, scaled 1x, 10x, 100x, and 1000x.
* These groups are then clustered by the decimal log value of the base dash length, grouped by 1x, 2x, 4.5x, and 5x.
* Clusters have base dash lengths of 0.1px, 1px, 10px, and 100px.

The result is that within each group of four lines, **all four lines *should* be drawn the same**, and across groups in each cluster of 16 lines, the dashes should get progressively longer. (See the last cluster for a mostly correct example.)

### Colour reference
Lines with dashes that are **0.05px or longer** before scaling are rendered **green** to highlight the correct rendering.

Lines with dashes that are **shorter than 0.05px** before scaling are rendered **red** to highlight the incorrect rendering.

**Gray** vertical lines are spaced 10px apart to help with eyeball measurements.

### Bugs
There are at least three bugs here:

1. Dashes are rendered a minimum of 0.05 pixels *before scaling*.
2. When scrolling horizontally, the last dash in a line may blink in and out.
3. The last dash in a line may be longer than the other dashes in the same line (e.g. cluster 4, group 3, line 4).

