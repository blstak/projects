# Conway's Game of Life (C#)

A console simulator for Conway's Game of Life, the cellular automaton in which a grid of cells lives, dies or is born depending on its neighbours. Written in C# with no external libraries.

## What it does
When you start the program, it asks for three numbers:

1. Width of the field
2. Height of the field
3. Number of living cells to start with

It scatters that many living cells at random across the grid, then advances one generation per second and redraws the field. Living cells are shown as `|` and dead cells as `-`. The simulation keeps running until you close the window.

## Run it

Keep the width below the width of your console window, or the rows will wrap and the picture will look broken. A good first try is a 40 x 20 field with 200 living cells.

## The rules
Each generation, every cell looks at its eight neighbours:

1. A living cell with fewer than 2 living neighbours dies (underpopulation).
2. A living cell with 2 or 3 living neighbours survives.
3. A living cell with more than 3 living neighbours dies (overpopulation).
4. A dead cell with exactly 3 living neighbours becomes alive (reproduction).

The edges of the field are walls: cells outside the grid count as dead, so patterns do not wrap around.

## How it works
- The field is a two-dimensional `bool[,]` array, where `true` means alive.
- Each generation builds a new array from the old one, so all cells update at the same time.
- Neighbours are counted with explicit handling for the corners and edges of the grid.
- The screen is cleared and redrawn after every step.

## Limitations
- The starting pattern is random only, with no way to enter a specific pattern such as a glider
- The speed is fixed at one generation per second
- There is no generation counter, and the program does not stop when the pattern dies out or becomes stable
- Input is not validated, so text or a living-cell count above width x height is not handled
