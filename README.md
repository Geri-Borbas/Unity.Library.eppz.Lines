# eppz! `Lines`
> part of [**Unity.Library.eppz**](https://github.com/eppz/Unity.Library.eppz)

Lightweight OpenGL line rendering for Unity. Like `Debug.DrawLine` in Game view. 

![Lightweight OpenGL line rendering for Unity. Like Debug.DrawLine in Game view.](https://github.com/eppz/Unity.Library.eppz.Lines/raw/Documentation/Documentation/EPPZ.Lines_444px.gif)

Simple drawing API goes like below (see example scene in [`Scenes`](Scenes) for more).

```
DrawLine(Vector2.up, Vector2.down, Color.white);
DrawCircle(Vector2.zero, 10.0f, Color.yellow);
```

## Usage

1. Add a `LineRendererCamera` script to your camera.
2. Hook up a material.
3. **A. Inspector only.** Add a `CachedLineRenderer` to a `GameObject` to define lines in inspector directly. 
3. **B. Code.** Implement `DirectLineRenderer` in a new script.

> See example scenes in [`Scenes`](Scenes) for more.

## Under the hood

The library uses **OpenGL immediate mode** drawing in the `Camera.OnPostRender()` template metod. While the lines in the scene gets batches into a single draw call, this library is not really meant for production use, **mainly useful for debugging purposes**.

```C#
...
GL.Begin(GL.LINES);
foreach (EPPZ.Lines.Line eachLine in lineBatch)
{
    GL.Color(eachLine.color);
    GL.Vertex(eachLine.from);
    GL.Vertex(eachLine.to);
}
GL.End();
...
```

The camera also calls `Debug.DrawLine` calls on `Update()`, so you can see your drawings in the scene view as well.

## Implement `DirectLineRenderer`

There is a single `OnDraw()` template method you can override, and impement your drawing code there.

> This method will be invoked from `LineRendererCamera`, batch you line definitions for the given frame, then draw them in `OnPostRender()` (see above).

```C#
public class Line : DirectLineRenderer
{
	protected override void OnDraw()
	{
		DrawLine(Vector2.up, Vector2.down, Color.white);
	}
}
```

Find more drawing code template `DrawPoints()`, `DrawRect()`, `DrawCircle()`, and `DrawPointsWithTransform()`, `DrawRectWithTransform()`, `DrawCircleWithTransform()` in [`LineRendererBase.cs`](LineRendererBase.cs). You can see there how to make you own drawing methods.

## Example

The code for the Lissajous curve above goes like below. It uses the same `DrawLine()` used before, the rest is trigonometry math. 

```C#
using UnityEngine;
using System.Collections;


public class Lissajous : EPPZ.Lines.DirectLineRenderer
{


	float t = 0.0f;

	const float resolution = 0.01f;
	const float speed = 0.2f;


	protected override void OnDraw ()
	{
		// Draw Lissajous curve segments.
		Vector2 from = Function(t, 0.0f);
		for (float s = resolution; s < Mathf.PI * (2.0 + resolution); s += resolution)
		{
			Vector2 to = Function(t, s);

			// Direct draw.
			DrawLine(from, to, Color.white);

			from = to;
		}

		// Step.
		t += speed;
	}

	Vector2 Function(float t, float s)
	{
		// More at https://en.wikipedia.org/wiki/Lissajous_curve
		return new Vector2(
			Mathf.Sin (5 * s + t),
			Mathf.Sin (4 * s + t)
		);
	}
}
```

## License

> Licensed under the [MIT license](http://en.wikipedia.org/wiki/MIT_License).